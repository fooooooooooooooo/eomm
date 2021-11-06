using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Matchmaking;
using EOMM.Models;
using EOMM.QuickGraph;

namespace EOMM.Algorithms {
  public class RandomMatchmaking : IMatchmaker {
    public string Name => "Random Matchmaking";

    public IEnumerable<PlayerEdge> Run(List<PlayerVertex>? players = null, PlayerGraph? playerGraph = null) {
      if (players is null) {
        throw new ArgumentException($"argument {nameof(players)} cannot be null for {nameof(RandomMatchmaking)}");
      }

      var pairCount = players.Count / 2;

      List<PlayerEdge> pairs = new();

      // shuffle players
      var random = new Random();
      var shuffledPlayers = players.OrderBy(_ => random.Next()).ToList();

      for (var i = 0; i < pairCount; i++) {
        PlayerEdge pair = new(shuffledPlayers.Pop(),
          shuffledPlayers.Pop(), -1, -1);

        pairs.Add(pair);
      }

      return pairs;
    }
  }
}
