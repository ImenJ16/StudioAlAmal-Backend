using Microsoft.EntityFrameworkCore;
using ContentService.Models;

namespace ContentService.Data;

public class ContentDbContext : DbContext
{
    public ContentDbContext(DbContextOptions<ContentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Promo> Promos { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<AboutUs> AboutUs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Promo entity
        modelBuilder.Entity<Promo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ImageUrl).HasMaxLength(500).IsRequired();
        });

        // Configure Photo entity
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ImageUrl).HasMaxLength(500).IsRequired();
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // Configure Video entity
        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.VideoUrl).HasMaxLength(500).IsRequired();
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // Configure AboutUs entity
        modelBuilder.Entity<AboutUs>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
        });
    }
}