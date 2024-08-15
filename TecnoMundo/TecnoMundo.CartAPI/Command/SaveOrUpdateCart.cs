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
            Cart cart = _mapper.Map<Cart>(vo);

            if (cart.CartDetails.FirstOrDefault().Count == 0) 
                throw new ArgumentException("Count invalid.");

            var productVO = await _productRepository.GetProductById(
                cart.CartDetails.FirstOrDefault().ProductId);

            if (productVO.Id == 0)
            {
                throw new ArgumentException("Product id invalid.");
            }

            var cartHeader = await _repository.FindCartHeaderById(cart.CartHeader.UserId);

            if (cartHeader is null)
            {
                await _repository.AddCartHeaders(cart.CartHeader);
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null;
                await _repository.AddCartDetails(cart.CartDetails.FirstOrDefault());
            }
            else
            {
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
                    cart.CartDetails.FirstOrDefault().Count = cartDetail.Count;
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
