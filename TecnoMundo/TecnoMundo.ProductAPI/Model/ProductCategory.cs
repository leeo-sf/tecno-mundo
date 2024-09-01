using TecnoMundo.ProductAPI.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoMundo.ProductAPI.Model
{
    [Table("category")]
    public class ProductCategory : BaseEntity
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Name { get; set; }
    }
}
