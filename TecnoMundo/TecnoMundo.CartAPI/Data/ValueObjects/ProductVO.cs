namespace GeekShopping.CartAPI.Data.ValueObjects
{
    public class ProductVO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public CategoryVO Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
