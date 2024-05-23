using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UNOSChat.AuthenticationAPI.Models;

namespace UNOSChat.AuthenticationAPI.Repository;

public class RepositoryContext :IdentityDbContext
{
    public RepositoryContext(DbContextOptions options):base(options)
    {
        
    }

    public DbSet<IdentityUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
