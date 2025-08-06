using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VConsole.Entity;
using VConsole.Util;

namespace VConsole.DBContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
    : DbContext(options)
{
    private readonly IConfiguration _configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(
                $"Data Source={_configuration["DBPath"]!}"
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VideoDetailRecord>().HasKey(v => v.Id);

        modelBuilder.Entity<VideoDetailRecord>()
            .HasMany(v => v.VideoGenres)
            .WithMany(g => g.VideoDetailRecord);

        modelBuilder.Entity<VideoDetailRecord>()
            .HasMany(v => v.VideoActor)
            .WithMany(a => a.VideoDetailRecord);
    }

    // 添加 VideoDetailRecord 的 DbSet

    public DbSet<VideoDetailRecord> VideoDetailRecords { get; set; }
}