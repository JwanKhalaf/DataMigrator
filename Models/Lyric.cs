using System.Collections.Generic;

namespace DataMigrator.Models
{
  public class Lyric
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public List<LyricSlug> Slugs { get; set; }
  }
}
