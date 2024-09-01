﻿using GeekShopping.CartAPI.Model.Base;

namespace GeekShopping.CartAPI.Data.ValueObjects
{
    public class CartHeaderVO : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
    }
}
