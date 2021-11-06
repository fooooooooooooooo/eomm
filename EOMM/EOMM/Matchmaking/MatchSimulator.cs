using System.Collections.Generic;
using System.IO;
using System.Linq;
using EOMM.Models;
using EOMM.QuickGraph;
using QuikGraph.Graphviz;
using static EOMM.Matchmaking.Matchmaking;

namespace EOMM.Matchmaking {
  public class MatchSimulator {
    private readonly PlayerGraph _playerGraph;
    private readonly List<PlayerVertex> _players;

    public MatchSimulator(int playerCount, PlayerGraph playerGraph) {
      _playerGraph = playerGraph;
      _players = new List<PlayerVertex>().Fill(playerCount, () => new PlayerVertex());
      _playerGraph.AddPlayers(_players);
      _playerGraph.GenerateEdges();
    }

    public double Run(IMatchmaker matchmaker) {
      var pairs = matchmaker.Run(_players, _playerGraph);

      return (from pair in pairs
        let retainWeight = 0d
        select pair.RetainWeight < 0
          ? GetRetainWeight(PredictPairChurn(pair.Source, pair.Target))
          : pair.RetainWeight
        into retainWeight
        select retainWeight / 100f).Sum();
    }
  }
}
