using Microsoft.EntityFrameworkCore;
using ResearchAgent.Models;

namespace ResearchAgent.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ResearchSession> Sessions => Set<ResearchSession>();
    public DbSet<ResearchStep> Steps => Set<ResearchStep>();
}
