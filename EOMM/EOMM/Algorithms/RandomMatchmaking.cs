using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Matchmaking;
using EOMM.Models;

namespace EOMM.Algorithms {
  public class RandomMatchmaking : Matchmaker {
    public override string Name => "Random Matchmaking";

    public override List<List<Player>> Run(IList<Player>? players = null, PlayerGraph? playerGraph = null) {
      if (players is null) {
        throw new ArgumentException($"argument {nameof(players)} cannot be null for {nameof(RandomMatchmaking)}");
      }

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
