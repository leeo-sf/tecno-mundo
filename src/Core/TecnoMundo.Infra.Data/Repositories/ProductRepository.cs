using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.Infra.Data.Context;

namespace TecnoMundo.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContextProduct _context;

        public ProductRepository(ApplicationDbContextProduct context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> FindAll()
        {
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }

        public async Task<IEnumerable<ProductCategory>> FindAllCategories()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<Product?> FindById(Guid id)
        {
            return await _context
                .Products.Include(x => x.Category)
                .Where(p => p.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> Delete(Guid id)
        {
            try
            {
                Product? product = await _context
                    .Products.Where(p => p.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (product is null)
                    return null;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> FindProductsByCategoryId(Guid id)
        {
            return await _context
                .Products.Include(p => p.Category)
                .Where(p => p.CategoryId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> ProductFilter(
            string? name,
            decimal? priceOf,
            decimal? priceUpTo
        )
        {
            return await _context
                .Products.Include(p => p.Category)
                .Where(p => p.Name.Contains(name) && p.Price >= priceOf && p.Price <= priceUpTo)
                .ToListAsync();
        }
    }
}
