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
        [StringLength(150)]
        public string Name { get; set; }
        [Column("price")]
        [Required]
        [Range(1, 10000)]
        public float Price { get; set; }
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; }
        [Column("category_name")]
        [StringLength(50)]
        public string CategoryName { get; set; }
        [Column("image_url")]
        [StringLength(300)]
        public string ImageUrl { get; set; }
    }
}
