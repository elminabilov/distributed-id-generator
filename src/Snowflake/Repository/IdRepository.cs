using Snowflake.Data;

namespace Snowflake.Repository;

public class IdRepository(AppDbContext dbContext) : IIdRepository
{
    public async Task SaveBatchAsync(IEnumerable<long> ids)
    {
        var entities = ids.Select(id => new UniqueId { Id = id });
        await dbContext.UniqueIds.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }
}