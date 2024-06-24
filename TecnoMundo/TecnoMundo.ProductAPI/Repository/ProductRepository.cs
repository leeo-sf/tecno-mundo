using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(
            MySQLContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductVO>> FindAll()
        {
            var products = await _context.Products.Include(x => x.Category).ToListAsync();
            return _mapper.Map<List<ProductVO>>(products);
        }

        public async Task<IEnumerable<CategoryVO>> FindAllCategories()
        {
            var categories = await _context.ProductCategories.ToListAsync();
            return _mapper.Map<List<CategoryVO>>(categories);
        }

        public async Task<ProductVO> FindById(long id)
        {
            var product = await _context.Products
                .Include(x => x.Category)
                .Where(p => p.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Create(ProductVO vo)
        {
            Product product = _mapper.Map<Product>(vo);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Update(ProductVO vo)
        {
            Product product = _mapper.Map<Product>(vo);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Product product = await _context.Products
                    .Where(p => p.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (product is null) return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductVO>> FindProductsByCategoryId(int id)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == id)
                .ToListAsync();

            return _mapper.Map<List<ProductVO>>(products);
        }

        public async Task<IEnumerable<ProductVO>> FindProductsByName(string name)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(name))
                .ToListAsync();

            return _mapper.Map<List<ProductVO>>(products);
        }
    }
}
