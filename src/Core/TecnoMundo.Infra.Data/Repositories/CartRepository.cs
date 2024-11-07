using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.Infra.Data.Context;

namespace TecnoMundo.Infra.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContextCart _context;

        public CartRepository(ApplicationDbContextCart context)
        {
            _context = context;
        }

        public async Task<Cart?> ApplyCoupon(Guid userId, string couponCode)
        {
            var header = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (header != null)
            {
                header.CouponCode = couponCode;
                _context.CartHeaders.Update(header);
                await _context.SaveChangesAsync();
                var cart = await FindCartByUserId(userId);
                return cart;
            }
            return null;
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

        public async Task<Cart> FindCartByUserId(Guid userId)
        {
            var cartHeader =
                await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId)
                ?? new CartHeader();

            var cartDetails = await _context
                .CartDetails.Where(c => c.CartHeaderId == cartHeader.Id)
                .ToListAsync();

            return new Cart { CartHeader = cartHeader, CartDetails = cartDetails };
        }

        public async Task<Cart?> RemoveCoupon(Guid userId)
        {
            var header = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (header != null)
            {
                header.CouponCode = "";
                _context.CartHeaders.Update(header);
                await _context.SaveChangesAsync();
                var cart = await FindCartByUserId(userId);
                return cart;
            }
            return null;
        }

        public async Task<CartDetail?> RemoveFromCart(Guid cartDetailsId)
        {
            try
            {
                CartDetail cartDetail =
                    await _context
                        .CartDetails.Include(x => x.CartHeader)
                        .FirstOrDefaultAsync(c => c.Id == cartDetailsId) ?? new CartDetail();
                int total = _context
                    .CartDetails.Where(c => c.CartHeaderId == cartDetail.CartHeaderId)
                    .Count();

                _context.CartDetails.Remove(cartDetail);
                if (total == 1)
                {
                    var cartHeaderToRemove =
                        await _context.CartHeaders.FirstOrDefaultAsync(c =>
                            c.Id == cartDetail.CartHeaderId
                        ) ?? new CartHeader();
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();

                return cartDetail;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task AddCartDetails(CartDetail cartDetail)
        {
            _context.CartDetails.Add(cartDetail);
            await _context.SaveChangesAsync();
        }

        public async Task AddCartHeaders(CartHeader cartHeader)
        {
            _context.CartHeaders.Add(cartHeader);
            await _context.SaveChangesAsync();
        }

        public async Task<CartHeader> FindCartHeaderById(Guid id)
        {
            return await _context
                    .CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.UserId == id) ?? new CartHeader();
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
                    ) ?? new CartDetail();
        }

        public async Task UpdateCartDetails(CartDetail cartDetail)
        {
            _context.CartDetails.Update(cartDetail);
            await _context.SaveChangesAsync();
        }
    }
}
