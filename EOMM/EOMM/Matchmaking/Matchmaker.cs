using System.Collections.Generic;
using EOMM.Models;
using EOMM.QuickGraph;

namespace EOMM.Matchmaking {
  public interface IMatchmaker {
    public string Name { get; }
    public IEnumerable<PlayerEdge> Run(List<PlayerVertex>? players = null, PlayerGraph? playerGraph = null);
  }
}
