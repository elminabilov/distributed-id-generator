namespace Snowflake.Data;

public class UniqueId
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}