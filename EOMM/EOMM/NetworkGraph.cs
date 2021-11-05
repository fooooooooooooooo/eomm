using System;
using System.Collections.Generic;
using System.Linq;

namespace EOMM {
  public class NetworkGraph {
    public IList<Node> Nodes { get; } = new List<Node>();
    public IList<Edge> Edges { get; } = new List<Edge>();

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