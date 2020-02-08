using DataMigrator.Models;
using System.Collections.Generic;

public interface ISQLServerWorker
{
  List<Artist> GetArtists();

  List<Lyric> GetLyricForArtist(int artistId);
}
