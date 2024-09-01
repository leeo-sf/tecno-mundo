namespace TecnoMundo.CartAPI.Data.ValueObjects
{
    public class CartDetailVO
    {
        public Guid Id { get; set; }
        public Guid CartHeaderId { get; set; }
        public CartHeaderVO? CartHeader { get; set; }
        public Guid ProductId { get; set; }
        public ProductVO Product { get; set; }
        public int Count { get; set; }
    }
}
