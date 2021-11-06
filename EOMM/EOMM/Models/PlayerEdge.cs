using QuikGraph;

namespace EOMM.Models {
  public class PlayerEdge : Edge<PlayerVertex> {
    public double ChurnWeight { get; }
    public double RetainWeight { get; }

    public PlayerEdge(PlayerVertex source, PlayerVertex target, double churnWeight, double retainWeight) : base(source,
      target) {
      ChurnWeight = churnWeight;
      RetainWeight = retainWeight;
    }
  }
}
