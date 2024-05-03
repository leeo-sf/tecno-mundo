using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Identity.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(new MySqlServerVersion(new Version(8, 0, 35)),
                mySqlOptions => mySqlOptions
                .EnableStringComparisonTranslations());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
