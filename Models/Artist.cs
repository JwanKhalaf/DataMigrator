using DataMigrator.Models;
using System.Collections.Generic;

public class Artist
{
  public string FirstName { get; set; }

  public string LastName { get; set; }

  public List<ArtistSlug> Slugs { get; set; }
}