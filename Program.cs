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
        .AddLogging()
        .AddSingleton<IPostgresWorker, PostgresWorker>()
        .AddSingleton<ISQLServerWorker, SQLServerWorker>()
        .BuildServiceProvider();

      // configure logging
      serviceProvider
        .GetService<ILoggerFactory>()
        .AddConsole(LogLevel.Debug);

      ILogger<Program> logger = serviceProvider
        .GetService<ILoggerFactory>()
        .CreateLogger<Program>();

      logger
        .LogDebug("Starting application");

      Console
        .WriteLine("hi");
    }
  }
}
