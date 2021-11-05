using System;

namespace EOMM {
  public class Node {
    public Node(Guid playerId) {
      PlayerId = playerId;
    }

    public Guid PlayerId { get; }
  }
}