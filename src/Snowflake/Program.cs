using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Data;
using Snowflake.Repository;
using Snowflake.SnowflakeAlgorithm;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection")));
services.AddScoped<IIdRepository, IdRepository>();

var serviceProvider = services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
context.Database.Migrate();

int dataCenterId = Convert.ToInt32(configuration.GetSection("Snowflake:DataCenterId").Value);
int machineId = Convert.ToInt32(configuration.GetSection("Snowflake:MachineId").Value);
var generator = new SnowflakeIdGenerator(dataCenterId, machineId);

var ids = new List<long>(100_000);
for (int i = 0; i < 100_000; i++)
{
    ids.Add(generator.NextId());
}

var repository = serviceProvider.GetRequiredService<IIdRepository>();
await repository.SaveBatchAsync(ids);

Console.WriteLine("✅ 100,000 IDs generated and saved successfully!");
Console.ReadLine();