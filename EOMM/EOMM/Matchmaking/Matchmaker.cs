using System.Collections.Generic;
using EOMM.Models;

namespace EOMM.Matchmaking {
  public abstract class Matchmaker {
    public abstract List<List<Player>> Run(IList<Player> players);
  }
}