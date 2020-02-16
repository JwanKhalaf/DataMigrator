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
          artist.Id = Convert.ToInt32(reader[0]);
          artist.FirstName = Convert.ToString(reader[1]).Trim();
          artist.LastName = Convert.ToString(reader[2]).Trim();
          artist.IsApproved = Convert.ToBoolean(reader[5]);

          ArtistSlug artistSlug = new ArtistSlug();
          artistSlug.Name = Convert.ToString(reader[3]).Trim();

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
    }

    return artists;
  }

  public List<Lyric> GetLyricForArtist(int artistId)
  {
    List<Lyric> lyrics = new List<Lyric>();

    using (SqlConnection connection = new SqlConnection(_databaseOptions.SqlServerConnectionString))
    {
      string query = "select * from Lyrics where ArtistId = @artistId;";

      // create the Command and Parameter objects.
      SqlCommand command = new SqlCommand(query, connection);
      command.Parameters.AddWithValue("@artistId", artistId);

      // open the connection in a try/catch block. 
      // create and execute the DataReader, writing the result
      // set to the console window.
      try
      {
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
          Lyric lyric = new Lyric();
          lyric.Id = Convert.ToInt32(reader[0]);
          lyric.Title = Convert.ToString(reader[1]).Trim();
          lyric.Body = Convert.ToString(reader[3]).Trim();
          lyric.IsApproved = Convert.ToBoolean(reader[6]);

          LyricSlug lyricSlug = new LyricSlug();
          lyricSlug.Name = Convert.ToString(reader[2]).Trim();

          lyric.Slugs = new List<LyricSlug>
          {
            lyricSlug
          };

          lyrics.Add(lyric);
        }

        reader.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    return lyrics;
  }
}