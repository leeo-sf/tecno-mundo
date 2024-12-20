﻿using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task<OrderHeader?> UpdateOrderPaymentStatus(Guid orderHeaderId, bool status);
        Task<List<OrderHeader>> GetAllOrder(Guid profileId);
    }
}
