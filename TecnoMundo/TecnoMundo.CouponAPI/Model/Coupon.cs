using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TecnoMundo.CouponAPI.Model.Base;

namespace TecnoMundo.CouponAPI.Model
{
    [Table("coupon")]
    public class Coupon : BaseEntity
    {
        [Column("coupon_code")]
        [Required]
        [StringLength(30)]
        public string CouponCode { get; set; }

        [Column("discount_amount")]
        [Required]
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
