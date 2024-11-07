using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.ProductAPI.Caching;

namespace TecnoMundo.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICachingService _cache;
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IMapper mapper, ICachingService cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task AddCartDetails(CartDetailVO vo)
        {
            var cartDetail = _mapper.Map<CartDetail>(vo);
            await _repository.AddCartDetails(cartDetail);
        }

        public async Task AddCartHeaders(CartHeaderVO vo)
        {
            var cartHeader = _mapper.Map<CartHeader>(vo);
            await _repository.AddCartHeaders(cartHeader);
        }

        public async Task<bool> ApplyCoupon(
            Guid userId,
            string couponCode,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var cartWithCoupon = await _repository.ApplyCoupon(userId, couponCode);
            if (cartWithCoupon == null)
                return false;

            var cartInCache =
                await _cache.GetItemInCache<CartVO>(keyCache)
                ?? throw new ApplicationException(
                    "Error getting shopping cart when applying coupon."
                );
            cartInCache.CartHeader.CouponCode = couponCode;

            await _cache.UpdateItemInCache(cartInCache, keyCache, options);

            return true;
        }

        public async Task<bool> ClearCart(Guid userId, string keyCache)
        {
            var cleanCart = await _repository.ClearCart(userId);
            await _cache.RemoveItemInCache(keyCache);
            return cleanCart;
        }

        public async Task<CartVO?> FindCartByUserId(
            Guid userId,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var cart = await _cache.GetItemInCache<CartVO>(keyCache);

            if (cart == null)
            {
                var cartToBeAdded = await _repository.FindCartByUserId(userId);
                var cartVO = _mapper.Map<CartVO>(cartToBeAdded);
                cart = cartVO;
            }
            return cart;
        }

        public async Task<CartDetailVO> FindCartDetailByProductIdAndCartHeaderId(
            Guid productId,
            Guid cartHeaderId
        )
        {
            var cartDetail = await _repository.FindCartDetailByProductIdAndCartHeaderId(
                productId,
                cartHeaderId
            );
            return _mapper.Map<CartDetailVO>(cartDetail);
        }

        public async Task<CartHeaderVO> FindCartHeaderById(Guid id)
        {
            var cartHeader = await _repository.FindCartHeaderById(id);
            return _mapper.Map<CartHeaderVO>(cartHeader);
        }

        public async Task<bool> RemoveCoupon(
            Guid userId,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var cartWithoutCoupon = await _repository.RemoveCoupon(userId);
            if (cartWithoutCoupon == null)
                return false;

            var cartInCache =
                await _cache.GetItemInCache<CartVO>(keyCache)
                ?? throw new ApplicationException(
                    "Error getting shopping cart when remove coupon."
                );
            cartInCache.CartHeader.CouponCode = "";

            await _cache.UpdateItemInCache(cartInCache, keyCache, options);

            return true;
        }

        public async Task<bool> RemoveFromCart(
            Guid cartDetailsId,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var newCart = await _repository.RemoveFromCart(cartDetailsId);
            if (newCart == null)
                return false;

            var cartInCache = await _cache.GetItemInCache<CartVO>(
                $"{keyCache}-{newCart.CartHeader.UserId}"
            );
            if (cartInCache != null)
                await _cache.RemoveItemByIdInAnItem<CartVO, CartDetailVO>(
                    cartDetailsId,
                    nameof(cartInCache.CartDetails),
                    $"{keyCache}-{newCart.CartHeader.UserId}",
                    options
                );

            return true;
        }

        public async Task UpdateCartDetails(CartDetailVO vo)
        {
            var cartDetail = _mapper.Map<CartDetail>(vo);
            await _repository.UpdateCartDetails(cartDetail);
        }

        public async Task<CartVO> SaveOrUpdate(
            CartVO vo,
            ProductVO productVO,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            Cart? cart;
            var cartInCache = await _cache.GetItemInCache<CartVO>(keyCache);

            if (vo?.CartDetails?.FirstOrDefault()?.Count == 0)
                throw new ArgumentException("Count invalid.");

            var cartHeader = await _repository.FindCartHeaderById(vo.CartHeader.UserId);

            if (cartHeader.Id == Guid.Empty)
            {
                var cartHeaderToBeCreated = CartHeader.CreateCartHeader(
                    vo.CartHeader.UserId,
                    vo.CartHeader.CouponCode
                );
                var cartDetailToBeCreated = CartDetail.CreateCartDetail(
                    cartHeaderId: cartHeaderToBeCreated.Id,
                    cartHeader: cartHeaderToBeCreated,
                    productId: productVO.Id,
                    product: null,
                    count: vo.CartDetails.FirstOrDefault().Count
                );
                await _repository.AddCartHeaders(cartHeaderToBeCreated);
                await _repository.AddCartDetails(cartDetailToBeCreated);

                var listCartDetails = new List<CartDetail>();
                listCartDetails.Add(cartDetailToBeCreated);

                cart = new Cart(cartHeader: cartHeaderToBeCreated, cartDetails: listCartDetails);
            }
            else
            {
                cart = _mapper.Map<Cart>(vo);

                var cartDetail = await _repository.FindCartDetailByProductIdAndCartHeaderId(
                    cart.CartDetails.FirstOrDefault().ProductId,
                    cartHeader.Id
                );

                if (cartDetail.Id == Guid.Empty)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    await _repository.AddCartDetails(cart.CartDetails.FirstOrDefault());
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
                    await _repository.UpdateCartDetails(cart.CartDetails.FirstOrDefault());
                }
            }

            cart.CartDetails.FirstOrDefault().Product = _mapper.Map<Product>(productVO);
            if (cartInCache != null)
            {
                var cartVO = _mapper.Map<CartVO>(cart);
                var cartDetailsAreAlReadyInTheCache = cartInCache.CartDetails.Any(x =>
                    x.Id == cart?.CartDetails?.FirstOrDefault()?.Id
                );
                if (cartDetailsAreAlReadyInTheCache)
                {
                    await _cache.UpdateItemToAnItemList(
                        cartVO.CartDetails.FirstOrDefault(),
                        cartInCache,
                        nameof(cartVO.CartDetails),
                        keyCache,
                        options
                    );
                }
                else
                {
                    await _cache.AddItemToAnItemList<CartDetailVO, CartVO>(
                        cartVO.CartDetails.FirstOrDefault(),
                        cartInCache,
                        nameof(cartVO.CartDetails),
                        keyCache,
                        options
                    );
                }
            }

            return _mapper.Map<CartVO>(cartInCache);
        }

        public async Task AddCartVOInCache(
            CartVO vo,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            await _cache.AddItemInCache(vo, keyCache, options);
        }
    }
}
