using DataMigrator.Models;
using System.Collections.Generic;

public class Artist
{
  public int Id { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public List<ArtistSlug> Slugs { get; set; }

  public List<Lyric> Lyrics { get; set; }
}