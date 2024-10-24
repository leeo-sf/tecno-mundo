using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Infra.Data.Context
{
    public class ApplicationDbContextCart : DbContext
    {
        public ApplicationDbContextCart(DbContextOptions<ApplicationDbContextCart> options)
            : base(options) { }

        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
    }
}
