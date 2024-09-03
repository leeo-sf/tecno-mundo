using Microsoft.EntityFrameworkCore;
using TecnoMundo.IdentityAPI.Utils;

namespace TecnoMundo.Identity.Model.Context
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

        public DbSet<User> Users { get; set; }
    }
}
