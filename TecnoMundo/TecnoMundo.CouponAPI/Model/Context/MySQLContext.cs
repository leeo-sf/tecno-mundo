using GeekShopping.CouponAPI.Model;
using Microsoft.EntityFrameworkCore;
using TecnoMundo.CouponAPI.Utils;

namespace GeekShopping.CouponAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        public DbSet<Coupon> Coupon { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>()
                .Property(p => p.Id)
                .HasValueGenerator<RandomIdValueGenerator>();

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 104819,
                CouponCode = "GEEK_SHOPPING_10",
                DiscountAmount = 10
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 907498,
                CouponCode = "GEEK_SHOPPING_15",
                DiscountAmount = 15
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
