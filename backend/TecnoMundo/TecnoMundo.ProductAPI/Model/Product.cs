using GeekShopping.ProductAPI.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.ProductAPI.Model
{
    [Table("product")]
    public class Product : BaseEntity
    {
        [Column("name")]
        [Required]
        [StringLength(150, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Name { get; set; }
        [Column("price")]
        [Required]
        [Range(1, 30000)]
        public decimal Price { get; set; }
        [Column("description")]
        [StringLength(5000, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Description { get; set; }
        [Column("color")]
        [StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Color { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("category")]
        public ProductCategory? Category { get; set; }
        [Column("image_url")]
        [StringLength(300)]
        public string ImageUrl { get; set; }
    }
}
