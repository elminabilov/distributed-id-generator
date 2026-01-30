using Microsoft.EntityFrameworkCore;

namespace Snowflake.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UniqueId> UniqueIds => Set<UniqueId>();
}