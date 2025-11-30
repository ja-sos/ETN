using ETN.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ETN.Infrastructure;
public class EtnDbContext(DbContextOptions<EtnDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;
}