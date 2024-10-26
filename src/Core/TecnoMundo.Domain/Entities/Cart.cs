namespace TecnoMundo.Domain.Entities
{
    public class Cart
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetail> CartDetails { get; set; }

        public Cart() { }

        public Cart(CartHeader cartHeader, IEnumerable<CartDetail> cartDetails)
        {
            CartHeader = cartHeader;
            CartDetails = cartDetails;
        }
    }
}
