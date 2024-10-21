using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> FindAll();
        Task<IEnumerable<ProductCategory>> FindAllCategories();
        Task<IEnumerable<Product>> FindProductsByCategoryId(Guid id);
        Task<IEnumerable<Product>> ProductFilter(
            string? name,
            decimal? priceOf,
            decimal? priceUpTo
        );
        Task<Product?> FindById(Guid id);
        Task<Product> Create(Product vo);
        Task<Product> Update(Product vo);
        Task<bool> Delete(Guid id);
    }
}
