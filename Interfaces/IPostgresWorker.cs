using System.Collections.Generic;

public interface IPostgresWorker
{
  void SaveArtists(List<Artist> artists);
}
