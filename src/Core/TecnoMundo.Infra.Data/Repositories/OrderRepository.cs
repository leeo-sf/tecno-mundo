using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.Infra.Data.Context;

namespace TecnoMundo.Infra.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContextOrder> _context;

        public OrderRepository(DbContextOptions<ApplicationDbContextOrder> context)
        {
            _context = context;
        }

        public async Task<bool> AddOrder(OrderHeader header)
        {
            if (header == null)
                return false;

            await using var _db = new ApplicationDbContextOrder(_context);
            _db.Headers.Add(header);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<OrderHeader?> UpdateOrderPaymentStatus(Guid orderHeaderId, bool status)
        {
            await using var _db = new ApplicationDbContextOrder(_context);
            var header = await _db
                .Headers.Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderHeaderId);
            if (header != null)
            {
                header.PaymentStatus = status;
                await _db.SaveChangesAsync();
            }
            return header;
        }

        public async Task<List<OrderHeader>> GetAllOrder(Guid profileId)
        {
            await using var _db = new ApplicationDbContextOrder(_context);
            var orderHeaders = await _db
                .Headers.Include(x => x.OrderDetails)
                .Where(o => o.UserId == profileId)
                .OrderByDescending(x => x.DateTime)
                .ToListAsync();
            return orderHeaders;
        }
    }
}
