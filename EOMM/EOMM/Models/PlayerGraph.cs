using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Graph;

namespace EOMM.Models {
  public class PlayerGraph {
    private readonly NetworkGraph _graph;

    public PlayerGraph(NetworkGraph graph) {
      _graph = graph;
    }

    public PlayerGraph() {
      _graph = new NetworkGraph();
    }


    public void LoadPlayers(List<Player> players) {
      AddPlayers(players);
      CalculateEdgeWeights(players);
    }

    private void AddPlayers(List<Player> players) {
      foreach (var player in players) {
        _graph.AddNode(player.Id);
      }
    }

    private void CalculateEdgeWeights(IReadOnlyList<Player> players) {
      for (var i = 0; i < players.Count; i++) {
        var first = players[i];

        for (var j = i + 1; j < players.Count; j++) {
          var second = players[j];

          var churnRate = Matchmaking.Matchmaking.PredictPairChurn(first, second);

          _graph.AddEdge(first.Id, second.Id, churnRate, 200 - churnRate);
        }
      }
    }

    public Dictionary<(Guid, Guid), double> GetRetainWeights() {
      var dictionary =
        _graph.Edges.ToDictionary(edge => (edge.From.PlayerId, edge.To.PlayerId), edge => edge.RetainWeight);

      return dictionary;
    }

    public List<Edge> GetEdges() {
      return _graph.Edges;
    }

    public static Edge? GetEdgeForNodes(IEnumerable<Edge> edges, Node first, Node second) {
      return edges.FirstOrDefault(e => e.From == first && e.To == second);
    }

    public static Node? GetNodeForPlayer(IEnumerable<Node> nodes, Player player) {
      return nodes.FirstOrDefault(n => n.PlayerId == player.Id);
    }

    public static Edge? GetEdgeForPlayers(IEnumerable<Edge> edges, Player first, Player second) {
      return edges.FirstOrDefault(e => e.From.PlayerId == first.Id && e.To.PlayerId == second.Id);
    }

    public static List<Node> MaxWeightPairing(List<Node> nodes, List<Player> players) {
      var highestWeight = 0.0;

      var highestPair = new List<Node>();
      for (var i = 0; i < nodes.Count; i++) {
        var n1 = nodes[i];
        for (var j = i + 1; j < nodes.Count; j++) {
          var n2 = nodes[j];

          var p1 = Player.FindById(players, n1.PlayerId);
          var p2 = Player.FindById(players, n2.PlayerId);

          if (p1 is null || p2 is null) {
            continue;
          }

          var w = Matchmaking.Matchmaking.PredictPairChurn(p1, p2);

          if (!(w > highestWeight)) continue;

          highestWeight = w;
          highestPair = new List<Node> {n1, n2};
        }
      }

      return highestPair;
    }

    public IEnumerable<List<Node>> MaxWeightMatching(List<Player> players) {
      var pairings = new List<List<Node>>();
      var tempNodes = new List<Node>(_graph.Nodes);

      while (tempNodes.Count > 1) {
        var pairing = MaxWeightPairing(tempNodes, players);

        pairings.Add(pairing);
        tempNodes.Remove(pairing[0]);
        tempNodes.Remove(pairing[1]);
      }

      return pairings;
    }

    public static IEnumerable<Player> NodesToPlayers(IEnumerable<Node> nodes, IEnumerable<Player> players) {
      // construct a list of players by id from node ids
      var playerIds = nodes.Select(n => n.PlayerId);
      return players.Where(p => playerIds.Contains(p.Id)).ToList();
    }

    public static IEnumerable<List<Player>> NodePairsToPlayerPairs(IEnumerable<List<Node>> nodePairs,
      List<Player> players) {
      return nodePairs.Select(np => NodesToPlayers(np, players).ToList()).ToList();
    }
  }
}
