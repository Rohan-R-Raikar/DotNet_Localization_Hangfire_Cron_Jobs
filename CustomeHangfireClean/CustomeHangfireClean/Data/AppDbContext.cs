using CustomeHangfireClean.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomeHangfireClean.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<JobCounter> JobCounters { get; set; }
    }
}
