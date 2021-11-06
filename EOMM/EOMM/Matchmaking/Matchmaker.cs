using System.Collections.Generic;
using EOMM.Graph;
using EOMM.Models;

namespace EOMM.Matchmaking {
  public abstract class Matchmaker {
    public abstract string Name { get; }
    public abstract List<List<Player>> Run(List<Player>? players = null, PlayerGraph? playerGraph = null);
  }
}
