using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Matchmaking;
using EOMM.Models;
using EOMM.QuickGraph;

namespace EOMM.Algorithms {
  public class SkillBasedMatchmaking : Matchmaker {
    public override string Name => "Skill Based Matchmaking";

    public override IEnumerable<PlayerEdge> Run(List<PlayerVertex>? players = null, PlayerGraph? playerGraph = null) {
      if (players is null) {
        throw new ArgumentException($"argument {nameof(players)} cannot be null for {nameof(SkillBasedMatchmaking)}");
      }

      var pairCount = players.Count / 2;
      var pairs = new List<PlayerEdge>();

      var sortedPlayers = players.OrderBy(x => x.Mmr).ToList();

      for (var i = 0; i < pairCount; i++) {
        var pair = new PlayerEdge(sortedPlayers.Pop(), sortedPlayers.Pop(), -1, -1);

        pairs.Add(pair);
      }

      return pairs;
    }
  }
}
