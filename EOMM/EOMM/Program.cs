using System;
using System.Collections.Generic;
using System.Linq;
using EOMM.Algorithms;
using EOMM.Matchmaking;
using EOMM.Models;
using EOMM.Simulation;

namespace EOMM {
  public static class Program {
    public static void Main() {
      Console.WriteLine("Hello, World!");

      List<double> randomResults = new();
      List<double> skillBasedResults = new();
      List<double> worstResults = new();

      for (var i = 0; i < 100; i++) {
        randomResults.Add(ProcessResults(Simulate(() => new RandomMatchmaking(), i)));
        skillBasedResults.Add(ProcessResults(Simulate(() => new SkillBasedMatchmaking(), i)));
        worstResults.Add(ProcessResults(Simulate(() => new WorstMatchmaking(), i)));
      }

      Console.WriteLine("Average players retained:");
      Console.WriteLine($"Random: {randomResults.Average()}");
      Console.WriteLine($"Skill based: {skillBasedResults.Average()}");
      Console.WriteLine($"Worst: {worstResults.Average()}");
    }

    private static double ProcessResults((string, double) results) {
      var (message, retained) = results;

      Console.WriteLine(message);

      return retained;
    }

    private static (string, double) Simulate(Func<Matchmaker> matchmaker, int iteration = 0) {
      var graph = new PlayerGraph();

      var retainedPlayers = new MatchSimulator(100, graph).Run(matchmaker());

      return (
        $"{matchmaker().Name.PadRight(30)}: Iteration {iteration.ToString().PadRight(5)} | Retained players: {retainedPlayers}",
        retainedPlayers
      );
    }
  }
}
