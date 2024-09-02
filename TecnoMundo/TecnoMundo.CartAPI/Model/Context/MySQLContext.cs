using Microsoft.EntityFrameworkCore;
using TecnoMundo.CartAPI.Utils;

namespace GeekShopping.CartAPI.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
    }
}
