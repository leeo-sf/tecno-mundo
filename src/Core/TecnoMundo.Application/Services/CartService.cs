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

        public CartService(ICartRepository repository,
            IMapper mapper,
            ICachingService cache)
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

        public async Task<bool> ApplyCoupon(Guid userId, string couponCode, string keyCache, DistributedCacheEntryOptions options)
        {
            var cartWithCoupon = await _repository.ApplyCoupon(userId, couponCode);
            if (cartWithCoupon == null) return false;

            await _cache.UpdateItemInCache(cartWithCoupon, keyCache, options);

            return true;
        }

        public async Task<bool> ClearCart(Guid userId, string keyCache)
        {
            var cleanCart = await _repository.ClearCart(userId);
            await _cache.RemoveItemInCache(keyCache);
            return cleanCart;
        }

        public async Task<CartVO?> FindCartByUserId(Guid userId, string keyCache, DistributedCacheEntryOptions options)
        {
            var cart = await _cache.GetItemInCache<CartVO>(keyCache);

            if (cart == null)
            {
                var cartToBeAdded = await _repository.FindCartByUserId(userId);
                var cartToBeAddedVO = _mapper.Map<CartVO>(cartToBeAdded);
                cart = await _cache.AddItemInCache(cartToBeAddedVO, keyCache, options);
            }
            return cart;
        }

        public async Task<CartDetailVO> FindCartDetailByProductIdAndCartHeaderId(Guid productId, Guid cartHeaderId)
        {
            var cartDetail = await _repository.FindCartDetailByProductIdAndCartHeaderId(productId, cartHeaderId);
            return _mapper.Map<CartDetailVO>(cartDetail);
        }

        public async Task<CartHeaderVO> FindCartHeaderById(Guid id)
        {
            var cartHeader = await _repository.FindCartHeaderById(id);
            return _mapper.Map<CartHeaderVO>(cartHeader);
        }

        public async Task<bool> RemoveCoupon(Guid userId, string keyCache, DistributedCacheEntryOptions options)
        {
            var cartWithoutCoupon = await _repository.RemoveCoupon(userId);
            if (cartWithoutCoupon == null) return false;

            await _cache.UpdateItemInCache(cartWithoutCoupon, keyCache, options);

            return true;
        }

        public async Task<bool> RemoveFromCart(Guid cartDetailsId, string keyCache, DistributedCacheEntryOptions options)
        {
            var newCart = await _repository.RemoveFromCart(cartDetailsId);
            if (newCart == null) return false;

            await _cache.AddItemInCache(newCart, $"{keyCache}-{newCart.CartHeader.UserId}", options);
            
            return true;
        }

        public async Task UpdateCartDetails(CartDetailVO vo)
        {
            var cartDetail = _mapper.Map<CartDetail>(vo);
            await _repository.UpdateCartDetails(cartDetail);
        }

        public async Task<CartVO> SaveOrUpdate(CartVO vo, ProductVO productVO, string keyCache, DistributedCacheEntryOptions options)
        {
            Cart? cart;

            if (vo?.CartDetails?.FirstOrDefault()?.Count == 0)
                throw new ArgumentException("Count invalid.");

            var cartHeader = await _repository.FindCartHeaderById(vo.CartHeader.UserId);

            if (cartHeader is null)
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

                if (cartDetail is null)
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
            var cartVO = _mapper.Map<CartVO>(cart);

            await _cache.UpdateItemInCache(cartVO, keyCache, options);

            return cartVO;
        }
    }
}
