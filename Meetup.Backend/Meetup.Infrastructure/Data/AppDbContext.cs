using Meetup.Core.Entities;
using Meetup.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AuthUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    
    public DbSet<User> Users { get; set; }
    public DbSet<ApplicationUserRefreshTokens> RefreshTokens { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}