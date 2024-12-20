﻿using TecnoMundo.Domain.Entities.Base;

namespace TecnoMundo.Application.DTOs
{
    public class CheckoutHeaderVO : BaseMessage
    {
        public Guid UserId { get; set; }
        public string CouponCode { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string FistrName { get; set; }
        public string LastName { get; set; }
        public DateTime DateTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpireMonthYear { get; set; }
        public int CartTotalItens { get; set; }
        public IEnumerable<CartDetailVO> CartDetails { get; set; }

        public CheckoutHeaderVO(
            Guid userId,
            string couponCode,
            decimal purchaseAmount,
            decimal discountAmount,
            string fistrName,
            string lastName,
            DateTime dateTime,
            string phone,
            string email,
            string cardNumber,
            string cVV,
            string expireMonthYear,
            int cartTotalItens,
            IEnumerable<CartDetailVO> cartDetails
        )
        {
            UserId = userId;
            CouponCode = couponCode;
            PurchaseAmount = purchaseAmount;
            DiscountAmount = discountAmount;
            FistrName = fistrName;
            LastName = lastName;
            DateTime = dateTime;
            Phone = phone;
            Email = email;
            CardNumber = cardNumber;
            CVV = cVV;
            ExpireMonthYear = expireMonthYear;
            CartTotalItens = cartTotalItens;
            CartDetails = cartDetails;
        }
    }
}
