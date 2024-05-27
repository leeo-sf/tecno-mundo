using System.ComponentModel.DataAnnotations;

namespace GeekShopping.ProductAPI.Data.ValueObjects
{
    public class ProductVO
    {
        public long Id { get; set; }
        [StringLength(150, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Name { get; set; }
        [Range(1, 30000, ErrorMessage = "{0} should be between R${1} and R${2}")]
        public decimal Price { get; set; }
        [StringLength(5000, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Description { get; set; }
        [StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public CategoryVO Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
