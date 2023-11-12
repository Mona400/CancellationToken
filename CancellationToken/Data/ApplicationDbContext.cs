
using CancellationTokensimple.Model;
using Microsoft.EntityFrameworkCore;

namespace CancellationTokensimple.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext()
        {
                
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TestTables> TestTables { set; get; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
