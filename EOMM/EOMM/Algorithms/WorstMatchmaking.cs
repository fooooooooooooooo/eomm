using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Matchmaking;
using EOMM.Models;

namespace EOMM.Algorithms {
  public class WorstMatchmaking : Matchmaker {
    public override string Name => "Worst Matchmaking";

    public override List<List<Player>> Run(List<Player>? players = null, PlayerGraph? playerGraph = null) {
      if (players is null) {
        throw new ArgumentException($"argument {nameof(players)} cannot be null for {nameof(SkillBasedMatchmaking)}");
      }

      if (playerGraph is null) {
        throw new ArgumentException(
          $"argument {nameof(playerGraph)} cannot be null for {nameof(SkillBasedMatchmaking)}");
      }

      var nodeMatches = playerGraph.MaxWeightMatching(players);

      var matches = PlayerGraph.NodePairsToPlayerPairs(nodeMatches, players);

      return matches.ToList();
    }
  }
}
