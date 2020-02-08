using DataMigrator.Config;
using DataMigrator.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class SQLServerWorker : ISQLServerWorker
{
  private readonly DatabaseOptions _databaseOptions;

  public SQLServerWorker(IOptionsMonitor<DatabaseOptions> optionsAccessor)
  {
    _databaseOptions = optionsAccessor.CurrentValue;
  }

  public List<Artist> GetArtists()
  {
    List<Artist> artists = new List<Artist>();

    using (SqlConnection connection = new SqlConnection(_databaseOptions.SqlServerConnectionString))
    {
      string query = "select * from Artists;";

      // create the Command and Parameter objects.
      SqlCommand command = new SqlCommand(query, connection);

      // open the connection in a try/catch block. 
      // create and execute the DataReader, writing the result
      // set to the console window.
      try
      {
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          Artist artist = new Artist();
          artist.FirstName = reader[1].ToString().Trim();
          artist.LastName = reader[2].ToString().Trim();

          ArtistSlug artistSlug = new ArtistSlug();
          artistSlug.Name = reader[3].ToString().Trim();

          artist.Slugs = new List<ArtistSlug>
          {
            artistSlug
          };

          artists.Add(artist);
        }

        reader.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      Console.ReadLine();
    }

    return artists;
  }
}