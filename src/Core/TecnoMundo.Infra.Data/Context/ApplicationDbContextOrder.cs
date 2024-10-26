using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Infra.Data.Context
{
    public class ApplicationDbContextOrder : DbContext
    {
        public ApplicationDbContextOrder(DbContextOptions<ApplicationDbContextOrder> options)
            : base(options) { }

        public DbSet<OrderDetail> Details { get; set; }
        public DbSet<OrderHeader> Headers { get; set; }
    }
}
