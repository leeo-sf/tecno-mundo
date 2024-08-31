using Microsoft.EntityFrameworkCore;
using TecnoMundo.IdentityAPI.Utils;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .HasValueGenerator<RandomIdValueGenerator>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
