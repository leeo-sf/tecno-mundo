using GeekShopping.ProductAPI.Data.ValueObjects;

namespace GeekShopping.ProductAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductVO>> FindAll();
        Task<IEnumerable<CategoryVO>> FindAllCategories();
        Task<IEnumerable<ProductVO>> FindProductsByCategoryId(int id);
        Task<IEnumerable<ProductVO>> ProductFilter(string? name, decimal? priceOf, decimal? priceUpTo);
        Task<ProductVO> FindById(long id);
        Task<ProductVO> Create(ProductVO vo);
        Task<ProductVO> Update(ProductVO vo);
        Task<bool> Delete(long id);
    }
}
