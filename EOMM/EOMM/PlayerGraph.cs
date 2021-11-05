using System.Collections.Generic;
using System.Linq;

namespace EOMM {
  public class PlayerGraph {
    private readonly NetworkGraph _graph;

    public PlayerGraph(NetworkGraph graph) {
      _graph = graph;
    }

    public PlayerGraph() {
      _graph = new NetworkGraph();
    }


    public void LoadPlayers(IList<Player> players) {
      AddPlayers(players);
      CalculateEdgeWeights(players);
    }

    private void AddPlayers(IList<Player> players) {
      foreach (var player in players) {
        _graph.AddNode(player.Id);
      }
    }

    private void CalculateEdgeWeights(IList<Player> players) {
      for (var i = 0; i < players.Count; i++) {
        var first = players[i];

        for (var j = i + 1; j < players.Count; j++) {
          var second = players[j];

          var churnRate = Matchmaking.PredictPairChurn(first, second);

          _graph.AddEdge(first.Id, second.Id, churnRate, 200 - churnRate);
        }
      }
    }


    // get the retainWeight property of every edge in graph
    public IList<Edge> GetRetainWeights() {
      return _graph.Edges.ToList();
    }
  }
}