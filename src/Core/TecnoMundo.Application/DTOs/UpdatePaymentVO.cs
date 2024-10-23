using GeekShopping.MessageBus;

namespace TecnoMundo.Application.DTOs
{
    public class UpdatePaymentVO : BaseMessage
    {
        public Guid OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
