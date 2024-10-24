using TecnoMundo.Application.DTOs;

namespace TecnoMundo.CartAPI.Service
{
    public interface IServiceProduct
    {
        Task<ProductVO> GetProductById(Guid productId);
        Task<CartVO> GetProductsByListCart(CartVO vo);
    }
}
