using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repository
{
    public interface IProductRepository
    {
        Task<ProductVO> GetProductById(Guid productId);
    }
}
