using TecnoMundo.CartAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecnoMundo.CartAPI.Model
{
    [Table("cart_detail")]
    public class CartDetail : BaseEntity
    {
        public Guid CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public virtual CartHeader CartHeader { get; set; }
        public Guid ProductId { get; set; }
        [NotMapped]
        public virtual Product? Product { get; set; }
        [Column("count")]
        public int Count { get; set; }
    }
}
