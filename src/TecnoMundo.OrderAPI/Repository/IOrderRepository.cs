using GeekShopping.OrderAPI.Model;
using TecnoMundo.OrderAPI.Model;

namespace GeekShopping.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task UpdateOrderPaymentStatus(Guid orderHeaderId, bool status);
        Task<List<OrderHeader>> GetAllOrder(Guid profileId);
    }
}
