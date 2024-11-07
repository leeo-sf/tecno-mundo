using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Infra.Data.Context
{
    public class ApplicationDbContextIdentity : DbContext
    {
        public ApplicationDbContextIdentity(DbContextOptions<ApplicationDbContextIdentity> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                new MySqlServerVersion(new Version(8, 0, 35)),
                mySqlOptions => mySqlOptions.EnableStringComparisonTranslations()
            );
        }

        public DbSet<User> User { get; set; }
    }
}
