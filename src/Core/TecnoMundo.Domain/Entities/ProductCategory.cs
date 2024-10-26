using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TecnoMundo.Domain.Entities.Base;

namespace TecnoMundo.Domain.Entities
{
    [Table("category")]
    public class ProductCategory : BaseEntity
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Name { get; set; }
    }
}
