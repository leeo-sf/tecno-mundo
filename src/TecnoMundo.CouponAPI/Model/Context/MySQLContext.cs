using Microsoft.EntityFrameworkCore;
using TecnoMundo.CouponAPI.Model;
using TecnoMundo.CouponAPI.Utils;

namespace TecnoMundo.CouponAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options)
            : base(options) { }

        public DbSet<Coupon> Coupon { get; set; }
    }
}
