namespace TecnoMundo.CouponAPI.Data.ValueObjects
{
    public class CouponVO
    {
        public Guid Id { get; set; }
        public string CouponCode { get; set; }
        public float DiscountAmount { get; set; }
    }
}
