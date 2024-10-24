using System.ComponentModel.DataAnnotations.Schema;
using TecnoMundo.Domain.Entities.Base;

namespace TecnoMundo.Domain.Entities
{
    [Table("cart_header")]
    public class CartHeader : BaseEntity
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("coupon_code")]
        public string CouponCode { get; set; }

        public CartHeader() { }

        public CartHeader(Guid userId, string couponCode)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CouponCode = couponCode;
        }

        public CartHeader(Guid id, Guid userId, string couponCode)
        {
            Id = id;
            UserId = userId;
            CouponCode = couponCode;
        }

        public static CartHeader CreateCartHeader(Guid userId, string couponCode)
        {
            return new CartHeader(userId: userId, couponCode: couponCode);
        }
    }
}
