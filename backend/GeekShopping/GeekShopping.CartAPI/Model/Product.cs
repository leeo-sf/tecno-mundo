using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeekShopping.CartAPI.Model
{
    [Table("product")]
    public class Product
    {
        //Espera que passe o id e não gere com o auto_increment
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public long Id { get; set; }
        [Column("name")]
        [Required]
        [StringLength(150, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Name { get; set; }
        [Column("price")]
        [Required]
        [Range(1, 30000, ErrorMessage = "{0} should be between R${1} and R${2}")]
        public decimal Price { get; set; }
        [Column("description")]
        [StringLength(5000, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Description { get; set; }
        [StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Color { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        public ProductCategory? Category { get; set; }
        [Column("image_url")]
        [StringLength(300)]
        public string ImageUrl { get; set; }
    }
}
