using DataMigrator.Config;
using DataMigrator.Models;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;

public class PostgresWorker : IPostgresWorker
{
  private readonly DatabaseOptions _databaseOptions;

  public PostgresWorker(IOptionsMonitor<DatabaseOptions> optionsAccessor)
  {
    _databaseOptions = optionsAccessor.CurrentValue;
  }

  public void SaveArtists(List<Artist> artists)
  {
    using (NpgsqlConnection connection = new NpgsqlConnection(_databaseOptions.PostgresConnectionString))
    {
      connection.Open();

      foreach (Artist artist in artists)
      {
        string firstName = artist.FirstName;
        string lastName = artist.LastName;
        string fullName = $"{firstName} {lastName}";
        bool isApproved = artist.IsApproved;
        string userId = "eb600251-1fdd-4c2e-bee0-a9dca87a271a";
        DateTime createdAt = DateTime.UtcNow;
        bool isDeleted = false;

        NpgsqlCommand command = new NpgsqlCommand("insert into artists (first_name, last_name, full_name, is_approved, user_id, created_at, is_deleted) VALUES (@first_name, @last_name, @full_name, @is_approved, @user_id, @created_at, @is_deleted) returning id", connection);

        command.Parameters.AddWithValue("first_name", firstName);
        command.Parameters.AddWithValue("last_name", lastName);
        command.Parameters.AddWithValue("full_name", fullName);
        command.Parameters.AddWithValue("is_approved", isApproved);
        command.Parameters.AddWithValue("user_id", userId);
        command.Parameters.AddWithValue("created_at", createdAt);
        command.Parameters.AddWithValue("is_deleted", isDeleted);

        object identity = command.ExecuteScalar();
        artist.Id = (int)identity;

        foreach (ArtistSlug artistSlug in artist.Slugs)
        {
          string artistSlugName = artistSlug.Name;
          bool isArtistSlugPrimary = true;
          DateTime artistSlugCreatedAt = DateTime.UtcNow;
          bool isArtistSlugDeleted = false;

          command = new NpgsqlCommand("insert into artist_slugs (name, is_primary, created_at, is_deleted, artist_id) values (@name, @is_primary, @created_at, @is_deleted, @artist_id)", connection);
          command.Parameters.AddWithValue("name", artistSlugName);
          command.Parameters.AddWithValue("is_primary", isArtistSlugPrimary);
          command.Parameters.AddWithValue("created_at", artistSlugCreatedAt);
          command.Parameters.AddWithValue("is_deleted", isArtistSlugDeleted);
          command.Parameters.AddWithValue("artist_id", artist.Id);

          command.ExecuteNonQuery();
        }
      }
    }
  }
}