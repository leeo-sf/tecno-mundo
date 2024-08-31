using GeekShopping.OrderAPI.Model;
using TecnoMundo.OrderAPI.Model;

namespace GeekShopping.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task UpdateOrderPaymentStatus(long orderHeaderId, bool status);
        Task<List<OrderHeader>> GetAllOrder(string profileId);
    }
}
