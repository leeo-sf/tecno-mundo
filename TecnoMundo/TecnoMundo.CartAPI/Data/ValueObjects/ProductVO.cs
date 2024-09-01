namespace TecnoMundo.CartAPI.Data.ValueObjects
{
    public class ProductVO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryVO Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
