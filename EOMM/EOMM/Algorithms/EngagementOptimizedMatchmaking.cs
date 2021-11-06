using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Matchmaking;
using EOMM.Models;
using EOMM.QuickGraph;
using static EOMM.QuickGraph.PlayerGraph;

namespace EOMM.Algorithms {
  public class EngagementOptimizedMatchmaking : IMatchmaker {
    public string Name => "Engagement Optimized Matchmaking";

    public IEnumerable<PlayerEdge> Run(List<PlayerVertex>? players = null, PlayerGraph? playerGraph = null) {
      if (players is null) {
        throw new ArgumentException($"argument {nameof(players)} cannot be null for {nameof(SkillBasedMatchmaking)}");
      }

      if (playerGraph is null) {
        throw new ArgumentException(
          $"argument {nameof(playerGraph)} cannot be null for {nameof(SkillBasedMatchmaking)}");
      }

      var matches = MaxMatching(playerGraph, (edge => edge.RetainWeight));

      return matches;
    }
  }
}
