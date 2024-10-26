using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Infra.Data.Context
{
    public class ApplicationDbContextCoupon : DbContext
    {
        public ApplicationDbContextCoupon(DbContextOptions<ApplicationDbContextCoupon> options)
            : base(options) { }

        public DbSet<Coupon> Coupon { get; set; }
    }
}
