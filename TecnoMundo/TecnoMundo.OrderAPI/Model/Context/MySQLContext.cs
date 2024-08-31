using Microsoft.EntityFrameworkCore;
using TecnoMundo.OrderAPI.Utils;

namespace GeekShopping.OrderAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        public DbSet<OrderDetail> Details { get; set; }
        public DbSet<OrderHeader> Headers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .Property(p => p.Id)
                .HasValueGenerator<RandomIdValueGenerator>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderHeader>()
                .Property(p => p.Id)
                .HasValueGenerator<RandomIdValueGenerator>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
