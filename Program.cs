using System;
using Microsoft.Extensions.DependencyInjection;

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

      Console.WriteLine("hi");
    }
  }
}
