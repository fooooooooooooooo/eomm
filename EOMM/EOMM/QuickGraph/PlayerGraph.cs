using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Models;
using QuikGraph;
using static EOMM.Matchmaking.Matchmaking;

namespace EOMM.QuickGraph {
  public class PlayerGraph : AdjacencyGraph<PlayerVertex, PlayerEdge> {
    public void AddPlayers(IEnumerable<PlayerVertex> players) {
      AddVertexRange(players);
    }

    public void GenerateEdges() {
      var vertices = Vertices.ToList();

      for (var i = 0; i < vertices.Count; i++) {
        var firstVertex = vertices[i]!;

        for (var j = i + 1; j < vertices.Count; j++) {
          var secondVertex = vertices[j];

          var churnRate = PredictPairChurn(firstVertex, secondVertex);

          var edge = new PlayerEdge(firstVertex, secondVertex, churnRate,
            GetRetainWeight(churnRate));
          AddEdge(edge);
        }
      }
    }

    public static IEnumerable<PlayerEdge> MaxMatching<TKey>(PlayerGraph graph, Func<PlayerEdge, TKey> matchFunc) {
      var edgeList = graph.Edges.ToList();

      var ordered = edgeList.OrderByDescending(matchFunc);
      var matchedVertices = new List<PlayerVertex>();

      var output = new List<PlayerEdge>();

      bool Unmatched(PlayerEdge edge) {
        return !matchedVertices.Contains(edge.Source) && !matchedVertices.Contains(edge.Target);
      }

      foreach (var edge in ordered.Where(Unmatched)) {
        output.Add(edge);
        matchedVertices.Add(edge.Source);
        matchedVertices.Add(edge.Target);
      }

      return output;
    }
  }
}
