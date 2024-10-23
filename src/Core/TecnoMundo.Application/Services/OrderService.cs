using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;

namespace TecnoMundo.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddOrder(OrderHeader header)
        {
            var orderAdded = await _repository.AddOrder(header);
            return orderAdded;
        }

        public async Task<List<OrderHeader>> GetAllOrder(Guid profileId)
        {
            var orders = await _repository.GetAllOrder(profileId);
            return orders;
        }

        public async Task UpdateOrderPaymentStatus(Guid orderHeaderId, bool status)
        {
            await _repository.UpdateOrderPaymentStatus(orderHeaderId, status);
        }
    }
}
