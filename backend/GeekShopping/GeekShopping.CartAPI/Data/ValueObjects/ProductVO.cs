namespace GeekShopping.CartAPI.Data.ValueObjects
{
    public class ProductVO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
