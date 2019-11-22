namespace DownloadsMonitor.Models
{
    using Microsoft.EntityFrameworkCore;

    public class DownloadsContext : DbContext
    {
        public DownloadsContext()
        {
        }

        public DownloadsContext(DbContextOptions<DownloadsContext> options)
            : base(options)
        {
        }

        public DbSet<FileEntry> Entries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(LocalDB)\MSSQLLocalDB;Database=Downloads;Integrated Security=True;MultipleActiveResultSets=True");
            }
        }
    }
}