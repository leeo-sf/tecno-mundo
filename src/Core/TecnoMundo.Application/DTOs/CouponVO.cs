﻿namespace TecnoMundo.Application.DTOs
{
    public class CouponVO
    {
        public Guid Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
