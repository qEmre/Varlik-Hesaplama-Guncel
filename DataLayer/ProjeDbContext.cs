using Microsoft.EntityFrameworkCore;
using varlikHesaplama.Models;

namespace varlikHesaplama.DataLayer
{
    public class ProjeDbContext : DbContext
    {
        public ProjeDbContext(DbContextOptions<ProjeDbContext> options) : base(options)
        { 

        }
        public DbSet<kurBilgisi> kurBilgisiTablo { get; set; }
        public DbSet<ufeEndeks> ufeEndeksTablo { get; set; }
        public DbSet<varlik> varlikTablo { get; set; }
    }
}
