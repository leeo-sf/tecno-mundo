namespace TecnoMundo.Application.DTOs
{
    public class PaymentVO
    {
        public Guid OrderId { get; set; }
        public string Name { get; set; }
        public string CartNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonthYear { get; set; }
        public decimal PurchaseAmount { get; set; }
        public string Email { get; set; }
    }
}
