using CustomeHangfireJob.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomeHangfireJob.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Counter> Counters { get; set; }
    }
}
