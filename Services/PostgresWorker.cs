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
        string userId = "6d9519c5-1c10-4ca5-8d26-dc8102c2294f";
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

        object artistIdentity = command.ExecuteScalar();
        artist.Id = (int)artistIdentity;

        if (artist.Image != null)
        {
          command = new NpgsqlCommand("insert into artist_images (data, created_at, artist_id) values (@data, @created_at, @artist_id)", connection);

          command.Parameters.AddWithValue("data", artist.Image.Data);
          command.Parameters.AddWithValue("created_at", createdAt);
          command.Parameters.AddWithValue("artist_id", artist.Id);

          command.ExecuteNonQuery();
        }

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

        foreach (Lyric lyric in artist.Lyrics)
        {
          bool isLyricApproved = lyric.IsApproved;

          if (isLyricApproved)
          {
            string lyricTitle = lyric.Title;
            string lyricBody = lyric.Body;
            string user_id = "6d9519c5-1c10-4ca5-8d26-dc8102c2294f";
            DateTime lyricCreatedAt = DateTime.UtcNow;
            bool isLyricDeleted = false;


            command = new NpgsqlCommand("insert into lyrics (title, body, user_id, created_at, is_deleted, is_approved, artist_id) values (@title, @body, @user_id, @created_at, @is_deleted, @is_approved, @artist_id) returning id", connection);

            command.Parameters.AddWithValue("title", lyricTitle);
            command.Parameters.AddWithValue("body", lyricBody);
            command.Parameters.AddWithValue("user_id", user_id);
            command.Parameters.AddWithValue("created_at", lyricCreatedAt);
            command.Parameters.AddWithValue("is_deleted", isLyricDeleted);
            command.Parameters.AddWithValue("is_approved", isLyricApproved);
            command.Parameters.AddWithValue("artist_id", artist.Id);

            object lyricIdentity = command.ExecuteScalar();
            lyric.Id = (int)lyricIdentity;

            foreach (LyricSlug lyricSlug in lyric.Slugs)
            {
              string lyricSlugName = lyricSlug.Name;
              bool isLyricSlugPrimary = true;
              DateTime lyricSlugCreatedAt = DateTime.UtcNow;
              bool isLyricSlugDeleted = false;

              command = new NpgsqlCommand("insert into lyric_slugs (name, is_primary, created_at, is_deleted, lyric_id) values (@name, @is_primary, @created_at, @is_deleted, @lyric_id)", connection);
              command.Parameters.AddWithValue("name", lyricSlugName);
              command.Parameters.AddWithValue("is_primary", isLyricSlugPrimary);
              command.Parameters.AddWithValue("created_at", lyricSlugCreatedAt);
              command.Parameters.AddWithValue("is_deleted", isLyricSlugDeleted);
              command.Parameters.AddWithValue("lyric_id", lyric.Id);

              command.ExecuteNonQuery();
            }
          }
        }
      }
    }
  }
}