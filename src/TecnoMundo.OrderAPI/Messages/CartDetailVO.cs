using TecnoMundo.Application.DTOs;

namespace GeekShopping.OrderAPI.Messages
{
    public class CartDetailVO
    {
        public Guid Id { get; set; }
        public Guid CartHeaderId { get; set; }
        public Guid ProductId { get; set; }
        public virtual ProductVO? Product { get; set; }
        public int Count { get; set; }
    }
}
