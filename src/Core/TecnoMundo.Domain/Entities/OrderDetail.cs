using System.ComponentModel.DataAnnotations.Schema;
using TecnoMundo.Domain.Entities.Base;

namespace TecnoMundo.Domain.Entities
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
        public decimal Price { get; set; }

        public OrderDetail() { }

        public OrderDetail(
            Guid orderHeaderId,
            Guid productId,
            int count,
            string productName,
            decimal price
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
            decimal price
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
