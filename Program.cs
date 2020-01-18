using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DataMigrator
{
  public class Program
  {
    static void Main(string[] args)
    {
      //setup up dependency injection
      ServiceProvider serviceProvider = new ServiceCollection()
        .AddLogging(logging =>
        {
          logging.AddConsole();
        })
        .AddSingleton<IPostgresWorker, PostgresWorker>()
        .AddSingleton<ISQLServerWorker, SQLServerWorker>()
        .BuildServiceProvider();

      ILogger<Program> logger = serviceProvider.GetService<ILogger<Program>>();

      logger
        .LogDebug("Starting application");

      Console
        .WriteLine("hi");
    }
  }
}
