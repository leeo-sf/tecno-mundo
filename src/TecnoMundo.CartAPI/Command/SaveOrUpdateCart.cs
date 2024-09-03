using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace GeekShopping.CartAPI.Command
{
    public class SaveOrUpdateCart : ISaveOrUpdateCart
    {
        private readonly ICartRepoository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public SaveOrUpdateCart(ICartRepoository repository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CartVO> Execute(CartVO vo)
        {
            Cart cart;

            if (vo.CartDetails.FirstOrDefault().Count == 0) 
                throw new ArgumentException("Count invalid.");

            var productVO = await _productRepository.GetProductById(
                vo.CartDetails.FirstOrDefault().ProductId);

            if (productVO.Id == Guid.Empty)
            {
                throw new ArgumentException("Product id invalid.");
            }

            var cartHeader = await _repository.FindCartHeaderById(vo.CartHeader.UserId);

            if (cartHeader is null)
            {
                var cartHeaderToBeCreated = CartHeader.CreateCartHeader(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
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
                    cartHeader.Id);

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
