using System.ComponentModel.DataAnnotations.Schema;
using TecnoMundo.Domain.Entities.Base;

namespace TecnoMundo.Domain.Entities
{
    [Table("order_header")]
    public class OrderHeader : BaseEntity
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("coupon_code")]
        public string CouponCode { get; set; }

        [Column("purchase_amount")]
        public decimal PurchaseAmount { get; set; }

        [Column("discount_amount")]
        public decimal DiscountAmount { get; set; }

        [Column("fistr_name")]
        public string FistrName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("purchase_date")]
        public DateTime DateTime { get; set; }

        [Column("order_time")]
        public DateTime OrderTime { get; set; }

        [Column("phone_number")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("card_number")]
        public string CardNumber { get; set; }

        [Column("cvv")]
        public string CVV { get; set; }

        [Column("expire_month_year")]
        public string ExpireMonthYear { get; set; }

        [Column("total_itens")]
        public int CartTotalItens { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        [Column("payment_status")]
        public bool PaymentStatus { get; set; }

        public OrderHeader() { }

        public OrderHeader(
            Guid userId,
            string couponCode,
            decimal purchaseAmount,
            decimal discountAmount,
            string fistrName,
            string lastName,
            DateTime dateTime,
            DateTime orderTime,
            string phone,
            string email,
            string cardNumber,
            string cVV,
            string expireMonthYear,
            int cartTotalItens,
            List<OrderDetail> orderDetails,
            bool paymentStatus
        )
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CouponCode = couponCode;
            PurchaseAmount = purchaseAmount;
            DiscountAmount = discountAmount;
            FistrName = fistrName;
            LastName = lastName;
            DateTime = dateTime;
            OrderTime = orderTime;
            Phone = phone;
            Email = email;
            CardNumber = cardNumber;
            CVV = cVV;
            ExpireMonthYear = expireMonthYear;
            CartTotalItens = cartTotalItens;
            OrderDetails = orderDetails;
            PaymentStatus = paymentStatus;
        }

        public OrderHeader(
            Guid id,
            Guid userId,
            string couponCode,
            decimal purchaseAmount,
            decimal discountAmount,
            string fistrName,
            string lastName,
            DateTime dateTime,
            DateTime orderTime,
            string phone,
            string email,
            string cardNumber,
            string cVV,
            string expireMonthYear,
            int cartTotalItens,
            List<OrderDetail> orderDetails,
            bool paymentStatus
        )
        {
            Id = id;
            UserId = userId;
            CouponCode = couponCode;
            PurchaseAmount = purchaseAmount;
            DiscountAmount = discountAmount;
            FistrName = fistrName;
            LastName = lastName;
            DateTime = dateTime;
            OrderTime = orderTime;
            Phone = phone;
            Email = email;
            CardNumber = cardNumber;
            CVV = cVV;
            ExpireMonthYear = expireMonthYear;
            CartTotalItens = cartTotalItens;
            OrderDetails = orderDetails;
            PaymentStatus = paymentStatus;
        }
    }
}
