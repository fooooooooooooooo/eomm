using System.Collections.Generic;

namespace EOMM {
  public abstract class Matchmaker {
    public abstract List<List<Player>> Run(IList<Player> players);
  }
}