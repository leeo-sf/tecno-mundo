using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoMundo.CartAPI.Model.Base
{
    public class BaseEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
    }
}
