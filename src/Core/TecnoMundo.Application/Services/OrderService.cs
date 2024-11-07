using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.ProductAPI.Caching;

namespace TecnoMundo.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICachingService _cache;
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository, ICachingService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<bool> AddOrder(
            OrderHeader obj,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var orderAdded = await _repository.AddOrder(obj);
            var orders = await _cache.GetListCache<OrderHeader>(keyCache);

            if (orders.Count != 0)
            {
                await _cache.AddItemToExistingListInCache(obj, keyCache, options);
            }

            return orderAdded;
        }

        public async Task<List<OrderHeader>> GetAllOrder(
            Guid profileId,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var orders = await _cache.GetListCache<OrderHeader>(keyCache);

            if (orders.Count == 0)
            {
                var listOrders = await _repository.GetAllOrder(profileId);
                orders = await _cache.AddListInCache(listOrders, keyCache, options);
            }

            return orders;
        }

        public async Task UpdateOrderPaymentStatus(
            Guid orderHeaderId,
            bool status,
            string keyCache,
            DistributedCacheEntryOptions options
        )
        {
            var orderUpdated = await _repository.UpdateOrderPaymentStatus(orderHeaderId, status);
            var orders = await _cache.GetListCache<OrderHeader>(
                $"{keyCache}-{orderUpdated?.UserId}"
            );

            if (orders.Count != 0)
            {
                int indexOfTheItemToBeUpdatedPayment = orders.FindIndex(order =>
                    order.Id == orderHeaderId
                );
                if (indexOfTheItemToBeUpdatedPayment != -1)
                {
                    orders[indexOfTheItemToBeUpdatedPayment].PaymentStatus = status;
                    await _cache.AddListInCache(orders, $"{keyCache}-{orders[indexOfTheItemToBeUpdatedPayment].UserId}", options);
                }
            }
        }
    }
}
