using System;
using System.Collections.Generic;
using System.Linq;

namespace EOMM.Graph {
  public class NetworkGraph {
    public List<Node> Nodes { get; } = new();
    public List<Edge> Edges { get; } = new();

    public Node AddNode(Guid playerId) {
      var node = new Node(playerId);
      Nodes.Add(node);
      return node;
    }

    public Edge AddEdge(Guid from, Guid to, double weight, double retainWeight) {
      var first = Nodes.FirstOrDefault(x => x.PlayerId == from);
      var second = Nodes.FirstOrDefault(x => x.PlayerId == to);

      first ??= AddNode(from);

      second ??= AddNode(to);

      return AddEdge(first, second, weight, retainWeight);
    }

    public Edge AddEdge(Node from, Node to, double weight, double retainWeight) {
      var edge = new Edge(from, to, weight, retainWeight);
      Edges.Add(edge);
      return edge;
    }
  }
}
