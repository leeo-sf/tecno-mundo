using TecnoMundo.CartAPI.Data.ValueObjects;

namespace TecnoMundo.CartAPI.Repository
{
    public interface IProductRepository
    {
        Task<ProductVO> GetProductById(Guid productId);
    }
}
