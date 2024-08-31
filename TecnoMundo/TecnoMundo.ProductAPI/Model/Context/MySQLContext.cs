using Microsoft.EntityFrameworkCore;
using TecnoMundo.ProductAPI.Utils;

namespace GeekShopping.ProductAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext() { }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .HasValueGenerator<RandomIdValueGenerator>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
