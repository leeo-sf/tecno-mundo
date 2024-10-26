using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Infra.Data.Context
{
    public class ApplicationDbContextProduct : DbContext
    {
        public ApplicationDbContextProduct() { }

        public ApplicationDbContextProduct(DbContextOptions<ApplicationDbContextProduct> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
