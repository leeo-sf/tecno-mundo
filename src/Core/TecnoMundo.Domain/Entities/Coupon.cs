using System.ComponentModel.DataAnnotations.Schema;
using TecnoMundo.Domain.Entities.Base;

namespace TecnoMundo.Domain.Entities
{
    [Table("coupon")]
    public class Coupon : BaseEntity
    {
        [Column("coupon_code")]
        public string CouponCode { get; set; }

        [Column("discount_amount")]
        public float DiscountAmount { get; set; }

        public Coupon(string couponCode, float discountAmount)
        {
            Id = Guid.NewGuid();
            CouponCode = couponCode;
            DiscountAmount = discountAmount;
        }

        public Coupon(Guid id, string couponCode, float discountAmount)
        {
            Id = id;
            CouponCode = couponCode;
            DiscountAmount = discountAmount;
        }
    }
}
