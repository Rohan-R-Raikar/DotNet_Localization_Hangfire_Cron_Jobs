using Microsoft.EntityFrameworkCore;
using LocalizationProject.Models;

namespace LocalizationProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<UserRegistrationModel> Users { get; set; }
    }
}
