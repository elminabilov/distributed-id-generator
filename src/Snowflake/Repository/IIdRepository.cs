namespace Snowflake.Repository;

public interface IIdRepository
{
    Task SaveBatchAsync(IEnumerable<long> ids);
}