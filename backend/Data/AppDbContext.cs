using Microsoft.EntityFrameworkCore;
using Ruleta.Models;
using System.Collections.Generic;

namespace Ruleta.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}
