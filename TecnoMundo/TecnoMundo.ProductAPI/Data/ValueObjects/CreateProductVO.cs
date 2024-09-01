namespace TecnoMundo.ProductAPI.Data.ValueObjects
{
    public class CreateProductVO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public Guid CategoryId { get; set; }
        public string ImageUrl { get; set; }
    }
}
