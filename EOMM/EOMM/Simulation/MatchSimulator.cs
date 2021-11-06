using System.Collections.Generic;
using EOMM.Matchmaking;
using EOMM.Models;

namespace EOMM.Simulation {
  public class MatchSimulator {
    private readonly PlayerGraph _playerGraph;
    private readonly List<Player> _players;

    public MatchSimulator(int playerCount, PlayerGraph playerGraph) {
      _playerGraph = playerGraph;
      _players = new List<Player>().Fill(playerCount, () => new Player());
      _playerGraph.LoadPlayers(_players);
    }

    public double Run(Matchmaker matchmaker) {
      var retainedPlayers = 0f;

      var pairs = matchmaker.Run(_players, _playerGraph);

      var retain = _playerGraph.GetRetainWeights();

      foreach (var pair in pairs) {
        var (first, second) = (pair[0], pair[1]);

        if (!retain.ContainsKey((first.Id, second.Id))) {
          (first, second) = (pair[1], pair[0]);
        }

        retainedPlayers += (float) retain[(first.Id, second.Id)] / 100f;
      }

      return retainedPlayers;
    }
  }
}
