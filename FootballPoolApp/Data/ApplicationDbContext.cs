using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FootballPoolApp.Models;

namespace FootballPoolApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Pick> Picks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationships
        builder.Entity<Pick>()
            .HasOne(p => p.Game)
            .WithMany(g => g.Picks)
            .HasForeignKey(p => p.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Pick>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure users can only pick once per game
        builder.Entity<Pick>()
            .HasIndex(p => new { p.UserId, p.GameId })
            .IsUnique();
    }
}
