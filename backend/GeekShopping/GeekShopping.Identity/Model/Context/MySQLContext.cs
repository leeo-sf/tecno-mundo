using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Identity.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
    }
}
