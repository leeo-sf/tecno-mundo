namespace TecnoMundo.Application.DTOs
{
    public class CartHeaderVO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? CouponCode { get; set; }
    }
}
