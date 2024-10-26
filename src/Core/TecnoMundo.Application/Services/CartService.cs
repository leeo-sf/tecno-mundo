using AutoMapper;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;

namespace TecnoMundo.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        public async Task<bool> ApplyCoupon(Guid userId, string couponCode)
        {
            var couponApplied = await _repository.ApplyCoupon(userId, couponCode);
            return couponApplied;
        }

        public async Task<bool> ClearCart(Guid userId)
        {
            var cleanCart = await _repository.ClearCart(userId);
            return cleanCart;
        }

        public async Task<CartVO> FindCartByUserId(Guid userId)
        {
            var cart = await _repository.FindCartByUserId(userId);
            if (cart.CartHeader.Id == Guid.Empty) return new CartVO();
            return _mapper.Map<CartVO>(cart);
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

        public async Task<bool> RemoveCoupon(Guid userId)
        {
            var removedCoupon = await _repository.RemoveCoupon(userId);
            return removedCoupon;
        }

        public async Task<bool> RemoveFromCart(Guid cartDetailsId)
        {
            var removed = await _repository.RemoveFromCart(cartDetailsId);
            return removed;
        }

        public async Task UpdateCartDetails(CartDetailVO vo)
        {
            var cartDetail = _mapper.Map<CartDetail>(vo);
            await _repository.UpdateCartDetails(cartDetail);
        }

        public async Task<CartVO> SaveOrUpdate(CartVO vo, ProductVO productVO)
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
            return _mapper.Map<CartVO>(cart);
        }
    }
}
