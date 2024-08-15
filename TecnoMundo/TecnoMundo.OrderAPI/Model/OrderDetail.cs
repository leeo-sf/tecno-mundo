using GeekShopping.OrderAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Model
{
    [Table("order_detail")]
    public class OrderDetail : BaseEntity
    {
        public int OrderHeaderId { get; set; }
        [Column("ProductId")]
        public int ProductId { get; set; }
        [Column("count")]
        public int Count { get; set; }
        [Column("product_name")]
        public string ProductName { get; set; }
        [Column("price")]
        public float Price { get; set; }
    }
}
