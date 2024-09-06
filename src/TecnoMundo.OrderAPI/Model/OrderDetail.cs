using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.OrderAPI.Model.Base;

namespace GeekShopping.OrderAPI.Model
{
    [Table("order_detail")]
    public class OrderDetail : BaseEntity
    {
        public Guid OrderHeaderId { get; set; }

        [Column("ProductId")]
        public Guid ProductId { get; set; }

        [Column("count")]
        public int Count { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("price")]
        public float Price { get; set; }

        public OrderDetail() { }

        public OrderDetail(
            Guid orderHeaderId,
            Guid productId,
            int count,
            string productName,
            float price
        )
        {
            Id = Guid.NewGuid();
            OrderHeaderId = orderHeaderId;
            ProductId = productId;
            Count = count;
            ProductName = productName;
            Price = price;
        }

        public OrderDetail(
            Guid id,
            Guid orderHeaderId,
            Guid productId,
            int count,
            string productName,
            float price
        )
        {
            Id = id;
            OrderHeaderId = orderHeaderId;
            ProductId = productId;
            Count = count;
            ProductName = productName;
            Price = price;
        }
    }
}
