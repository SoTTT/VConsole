using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VConsole.Entity;
using VConsole.Util;

namespace VConsole.DBContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(
                $"Data Source={ConfigurationUtil.Configuration["DBPath"]!}"
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VideoDetailRecord>().HasKey(v => v.Id);

        modelBuilder.Entity<VideoDetailRecord>()
            .HasMany(v => v.VideoGenres)
            .WithOne(g => g.VideoDetailRecord)
            .HasForeignKey(g => g.VideoDetailRecordId);

        modelBuilder.Entity<VideoDetailRecord>()
            .HasMany(v => v.VideoActor)
            .WithOne(a => a.VideoDetailRecord)
            .HasForeignKey(a => a.VideoDetailRecordId);
    }

    // 添加 VideoDetailRecord 的 DbSet

    public DbSet<VideoDetailRecord> VideoDetailRecords { get; set; }
}