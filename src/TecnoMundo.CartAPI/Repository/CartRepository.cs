﻿using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository
{
    public class CartRepository : ICartRepoository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public CartRepository(
            MySQLContext context,
            IMapper mapper,
            IProductRepository productRepository
        )
        {
            _context = context;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<bool> ApplyCoupon(Guid userId, string couponCode)
        {
            var header = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (header != null)
            {
                header.CouponCode = couponCode;
                _context.CartHeaders.Update(header);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ClearCart(Guid userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c =>
                c.UserId == userId
            );
            if (cartHeader != null)
            {
                _context.CartDetails.RemoveRange(
                    _context.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id)
                );
                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CartVO> FindCartByUserId(Guid userId)
        {
            Cart cart =
                new()
                {
                    CartHeader =
                        await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId)
                        ?? new CartHeader()
                };
            cart.CartDetails = await _context
                .CartDetails.Where(c => c.CartHeaderId == cart.CartHeader.Id)
                .ToListAsync();

            foreach (var cartItem in cart.CartDetails)
            {
                var product = await _productRepository.GetProductById(cartItem.ProductId);
                cartItem.Product = _mapper.Map<Product>(product);
            }

            return _mapper.Map<CartVO>(cart);
        }

        public async Task<bool> RemoveCoupon(Guid userId)
        {
            var header = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (header != null)
            {
                header.CouponCode = "";
                _context.CartHeaders.Update(header);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveFromCart(Guid cartDetailsId)
        {
            try
            {
                CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(c =>
                    c.Id == cartDetailsId
                );
                int total = _context
                    .CartDetails.Where(c => c.CartHeaderId == cartDetail.CartHeaderId)
                    .Count();

                _context.CartDetails.Remove(cartDetail);
                if (total == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(c =>
                        c.Id == cartDetail.CartHeaderId
                    );
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task AddCartDetails(CartDetail cartDetailsVO)
        {
            var cartDetail = _mapper.Map<CartDetail>(cartDetailsVO);

            _context.CartDetails.Add(cartDetail);
            await _context.SaveChangesAsync();
        }

        public async Task AddCartHeaders(CartHeader cartHeaderVO)
        {
            var cartHeader = _mapper.Map<CartHeader>(cartHeaderVO);

            _context.CartHeaders.Add(cartHeader);
            await _context.SaveChangesAsync();
        }

        public async Task<CartHeader> FindCartHeaderById(Guid id)
        {
            return await _context
                .CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == id);
        }

        public async Task<CartDetail> FindCartDetailByProductIdAndCartHeaderId(
            Guid productId,
            Guid cartHeaderId
        )
        {
            return await _context
                .CartDetails.AsNoTracking()
                .FirstOrDefaultAsync(p =>
                    p.ProductId == productId && p.CartHeaderId == cartHeaderId
                );
        }

        public async Task UpdateCartDetails(CartDetail cartDetail)
        {
            _context.CartDetails.Update(cartDetail);
            await _context.SaveChangesAsync();
        }
    }
}
