using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Matchmaking;
using EOMM.Models;

namespace EOMM.Algorithms {
  public class SkillBasedMatchmaking : Matchmaker {
    public override string Name => "Skill Based Matchmaking";

    public override List<List<Player>> Run(List<Player>? players = null, PlayerGraph? playerGraph = null) {
      if (players is null) {
        throw new ArgumentException($"argument {nameof(players)} cannot be null for {nameof(SkillBasedMatchmaking)}");
      }

      var pairCount = players.Count / 2;
      var pairs = new List<List<Player>>();

      var sortedPlayers = players.OrderBy(x => x.Mmr).ToList();

      for (var i = 0; i < pairCount; i++) {
        var pair = new List<Player> {
          sortedPlayers.Pop(),
          sortedPlayers.Pop()
        };
        pairs.Add(pair);
      }

      return pairs;
    }
  }
}
