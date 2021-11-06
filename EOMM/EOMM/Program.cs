using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EOMM.Algorithms;
using EOMM.Matchmaking;
using EOMM.QuickGraph;
using EOMM.Simulation;

namespace EOMM {
  public static class Program {
    public static void Main() {
      List<Result> randomResults = new();
      List<Result> skillBasedResults = new();
      List<Result> worstResults = new();
      List<Result> engagementOptimizedResults = new();

      const int playerCount = 100;

      Console.WriteLine("Simulating with " + playerCount + " players\n");

      var header = $"{"Time",-10}  {"Name",-32}  {"Iteration",-9}  {"Retained",-8} ";
      Console.WriteLine(header);

      // TODO : generate players in one place, then reuse them for all algorithms

      for (var i = 0; i < 3; i++) {
        randomResults.Add(ProcessResult(Simulate(playerCount, () => new RandomMatchmaking()), i));
        skillBasedResults.Add(ProcessResult(Simulate(playerCount, () => new SkillBasedMatchmaking()), i));
        worstResults.Add(ProcessResult(Simulate(playerCount, () => new WorstMatchmaking()), i));
        engagementOptimizedResults.Add(ProcessResult(Simulate(playerCount, () => new EngagementOptimizedMatchmaking()),
          i));
      }

      Console.WriteLine("\n-- Averages --\n");

      Console.WriteLine(playerCount + " players\n");

      var resultsHeader = $"{"Name",-32}  {"Time",-10}  {"Retained",-8} ";
      Console.WriteLine(resultsHeader);

      Console.WriteLine(AverageResults(randomResults));
      Console.WriteLine(AverageResults(skillBasedResults));
      Console.WriteLine(AverageResults(worstResults));
      Console.WriteLine(AverageResults(engagementOptimizedResults));
    }


    private static string AverageResults(IReadOnlyCollection<Result> results) {
      var averageRetained = results.Average(r => r.Retained);
      var averageTime = results.Average(r => r.Time.TotalMilliseconds);
      var name = results.First().Name;

      var time = $"{averageTime:0.00} ms";

      var retained = $"{averageRetained:0.0000}";

      var values = $"{name,-32}  {time,-10}  {retained,-8} ";

      return values;
    }

    private static Result ProcessResult(Result result, int i) {
      var name = result.Name;
      var retainedPlayers = $"{result.Retained:0.0000}";
      var ms = $"{result.Time.TotalMilliseconds:0.00} ms";

      var message = $"{ms,-10}  {name,-32}  {i + 1,-9}  {retainedPlayers,-8} ";

      Console.WriteLine(message);

      return result;
    }

    private static Result Simulate(int playerCount, Func<Matchmaker> matchmaker) {
      var timer = new Stopwatch();
      timer.Start();
      var graph = new PlayerGraph();

      var retainedPlayers = new MatchSimulator(playerCount, graph).Run(matchmaker());

      timer.Stop();

      return new Result(matchmaker().Name, retainedPlayers, timer.Elapsed);
    }
  }

  public class Result {
    public Result(string name, double retained, TimeSpan time) {
      Name = name;
      Retained = retained;
      Time = time;
    }

    public string Name { get; }
    public double Retained { get; }
    public TimeSpan Time { get; }
  }
}
