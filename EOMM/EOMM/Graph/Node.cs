using System;

namespace EOMM.Graph {
  public class Node {
    public Node(Guid playerId) {
      PlayerId = playerId;
    }

    public Guid PlayerId { get; }
  }
}