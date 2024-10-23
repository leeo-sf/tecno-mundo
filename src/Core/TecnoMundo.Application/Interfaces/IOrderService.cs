using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddOrder(OrderHeader header);
        Task UpdateOrderPaymentStatus(Guid orderHeaderId, bool status);
        Task<List<OrderHeader>> GetAllOrder(Guid profileId);
    }
}
