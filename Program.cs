using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataMigrator.Config;
using DataMigrator.Models;
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

      ISQLServerWorker sqlServerWorker = serviceProvider.GetService<ISQLServerWorker>();

      List<Artist> artists = sqlServerWorker.GetArtists();

      foreach (Artist artist in artists)
      {
        List<Lyric> lyrics = sqlServerWorker.GetLyricForArtist(artist.Id);

        artist.Lyrics = lyrics;
      }

      foreach (Artist artist in artists)
      {
        string artistSlug = artist.Slugs.First().Name;

        string filePath = Directory.GetCurrentDirectory() + "\\Images\\" + artistSlug + ".jpg";

        if (File.Exists(filePath))
        {
          ArtistImage artistImage = new ArtistImage();
          artistImage.Data = File.ReadAllBytes(filePath);

          artist.Image = artistImage;
        } else
        {
          Console.WriteLine($"{artistSlug} has no photo!");
        }
      }

      // writing to postgres
      IPostgresWorker postgresWorker = serviceProvider.GetService<IPostgresWorker>();

      postgresWorker.SaveArtists(artists);

      Console.ReadLine();
    }
  }
}
