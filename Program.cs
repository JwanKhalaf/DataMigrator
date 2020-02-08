using System;
using DataMigrator.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DataMigrator
{
  public class Program
  {
    static void Main(string[] args)
    {
      // set up configuration
      IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
      
      // duplicate here any configuration sources you use.
      configurationBuilder.AddUserSecrets<Program>();

      IConfiguration configuration = configurationBuilder.Build();

      // set up dependency injection
      ServiceProvider serviceProvider = new ServiceCollection()
        .AddLogging(logging =>
        {
          logging.AddConsole();
        })
        .AddSingleton<IPostgresWorker, PostgresWorker>()
        .AddSingleton<ISQLServerWorker, SQLServerWorker>()
        .Configure<DatabaseOptions>(configuration)
        .BuildServiceProvider();

      ILogger<Program> logger = serviceProvider.GetService<ILogger<Program>>();

      var temp = serviceProvider.GetService<ISQLServerWorker>();

      temp.GetArtists();

      logger
        .LogDebug("Starting application");

      Console
        .WriteLine("hi");
    }
  }
}
