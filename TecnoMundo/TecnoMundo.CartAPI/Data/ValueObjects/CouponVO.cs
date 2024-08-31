namespace GeekShopping.CartAPI.Data.ValueObjects
{
    public class CouponVO
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
