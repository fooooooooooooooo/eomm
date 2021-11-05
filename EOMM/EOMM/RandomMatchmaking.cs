using System;
using System.Collections.Generic;
using System.Linq;

namespace EOMM {
  public class RandomMatchmaking : Matchmaker {
    public override List<List<Player>> Run(IList<Player> players) {
      var pairCount = players.Count / 2;

      List<List<Player>> pairs = new();

      // shuffle players
      var random = new Random();
      var shuffledPlayers = players.OrderBy(p => random.Next()).ToList();

      for (var i = 0; i < pairCount; i++) {
        List<Player> pair = new() {
          shuffledPlayers.Pop(),
          shuffledPlayers.Pop()
        };

        pairs.Add(pair);
      }

      return pairs;
    }
  }
}