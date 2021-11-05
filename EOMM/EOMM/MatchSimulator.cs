using System;
using System.Collections.Generic;
using System.Linq;

namespace EOMM {
  public class MatchSimulator {
    private readonly int _playerCount;
    private readonly PlayerGraph _playerGraph;
    private readonly IList<Player> _players;

    public MatchSimulator(int playerCount, PlayerGraph playerGraph) {
      _playerCount = playerCount;
      _playerGraph = playerGraph;
      _players = new List<Player>().Fill(playerCount, () => new Player());
      _playerGraph.LoadPlayers(_players);
    }

    public int Run(Matchmaker matchmaker) {
      var retainedPlayers = 0;

      var result = matchmaker.Run(_players);
      var retain = _playerGraph.GetRetainWeights();
      foreach (var pair in result) {
        var edge = retain.FirstOrDefault(x => x.From.PlayerId == pair[1].Id || x.To.PlayerId == pair[0].Id);

        if (edge is null) {
          Console.WriteLine("first try: cant find edge");
          pair.Reverse();
        }

        var newEdge = retain.FirstOrDefault(x => x.From.PlayerId == pair[0].Id || x.To.PlayerId == pair[1].Id);

        if (newEdge is null) {
          Console.WriteLine("second try: cant find edge");
          continue;
        }

        retainedPlayers += (int) (newEdge!.RetainWeight / 100f);
      }

      return retainedPlayers;
    }
  }
}
