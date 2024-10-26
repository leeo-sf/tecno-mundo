using TecnoMundo.Application.DTOs;

namespace TecnoMundo.Application.Interfaces
{
    public interface IProductService
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
