using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using TecnoMundo.OrderAPI.Model;

namespace GeekShopping.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<MySQLContext> _context;

        public OrderRepository(DbContextOptions<MySQLContext> context)
        {
            _context = context;
        }

        public async Task<bool> AddOrder(OrderHeader header)
        {
            if (header == null) return false;

            await using var _db = new MySQLContext(_context);
            _db.Headers.Add(header);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool status)
        {
            await using var _db = new MySQLContext(_context);
            var header = await _db.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);
            if (header != null)
            {
                header.PaymentStatus = status;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetAllOrder(string profileId)
        {
            await using var _db = new MySQLContext(_context);
            var orderList = new List<Order>();
            var orderHeader = await _db.Headers.Where(x => x.UserId == profileId).ToListAsync();
            foreach(var order in orderHeader)
            {
                var orderDetails = await _db.Details.Where(x => x.OrderHeaderId == order.Id).ToListAsync();
                orderList.Add(new Order(order, orderDetails));
            }

            return orderList;
        }
    }
}
