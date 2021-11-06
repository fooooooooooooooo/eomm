using System.Collections.Generic;
using EOMM.Models;
using EOMM.QuickGraph;

namespace EOMM.Matchmaking {
  public abstract class Matchmaker {
    public abstract string Name { get; }
    public abstract IEnumerable<PlayerEdge> Run(List<PlayerVertex>? players = null, PlayerGraph? playerGraph = null);
  }
}
