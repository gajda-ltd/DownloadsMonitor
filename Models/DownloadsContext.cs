namespace DownloadsMonitor.Models
{
    using Microsoft.EntityFrameworkCore;

    public class DownloadsContext : DbContext
    {
        public DbSet<FileEntry> Entries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(LocalDB)\MSSQLLocalDB;Database=Downloads;Integrated Security=True;MultipleActiveResultSets=True");
        }
    }
}