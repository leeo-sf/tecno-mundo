using Microsoft.Extensions.Caching.Distributed;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddOrder(
            OrderHeader header,
            string keyCache,
            DistributedCacheEntryOptions options
        );
        Task UpdateOrderPaymentStatus(
            Guid orderHeaderId,
            bool status,
            string keyCache,
            DistributedCacheEntryOptions options
        );
        Task<List<OrderHeader>> GetAllOrder(
            Guid profileId,
            string keyCache,
            DistributedCacheEntryOptions options
        );
    }
}
