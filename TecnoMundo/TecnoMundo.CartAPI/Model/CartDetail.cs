using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.CartAPI.Model.Base;

namespace GeekShopping.CartAPI.Model
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

        public CartDetail() { }

        public CartDetail(
            Guid cartHeaderId,
            CartHeader cartHeader,
            Guid productId,
            Product? product,
            int count
        )
        {
            Id = Guid.NewGuid();
            CartHeaderId = cartHeaderId;
            CartHeader = cartHeader;
            ProductId = productId;
            Product = product;
            Count = count;
        }

        public CartDetail(
            Guid id,
            Guid cartHeaderId,
            CartHeader cartHeader,
            Guid productId,
            Product? product,
            int count
        )
        {
            Id = id;
            CartHeaderId = cartHeaderId;
            CartHeader = cartHeader;
            ProductId = productId;
            Product = product;
            Count = count;
        }

        public static CartDetail CreateCartDetail(
            Guid cartHeaderId,
            CartHeader cartHeader,
            Guid productId,
            Product? product,
            int count
        )
        {
            return new CartDetail(
                cartHeaderId: cartHeaderId,
                cartHeader: cartHeader,
                productId: productId,
                product: product,
                count: count
            );
        }
    }
}
