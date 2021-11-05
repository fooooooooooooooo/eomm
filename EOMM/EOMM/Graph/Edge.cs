namespace EOMM.Graph {
  public class Edge {
    public Edge(Node from, Node to, double weight, double retainWeight) {
      From = from;
      To = to;
      Weight = weight;
      RetainWeight = retainWeight;
    }

    public Node From { get; }
    public Node To { get; }
    public double Weight { get; }
    public double RetainWeight { get; }
  }
}