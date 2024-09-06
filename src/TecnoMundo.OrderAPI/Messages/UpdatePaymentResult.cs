namespace GeekShopping.OrderAPI.Messages
{
    public class UpdatePaymentResult
    {
        public Guid OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
