using TecnoMundo.ProductAPI.Data.ValueObjects;
using TecnoMundo.ProductAPI.Data.ValueObjects;

namespace TecnoMundo.ProductAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductVO>> FindAll();
        Task<IEnumerable<CategoryVO>> FindAllCategories();
        Task<IEnumerable<ProductVO>> FindProductsByCategoryId(Guid id);
        Task<IEnumerable<ProductVO>> ProductFilter(
            string? name,
            decimal? priceOf,
            decimal? priceUpTo
        );
        Task<ProductVO> FindById(Guid id);
        Task<ProductVO> Create(CreateProductVO vo);
        Task<ProductVO> Update(ProductVO vo);
        Task<bool> Delete(Guid id);
    }
}
