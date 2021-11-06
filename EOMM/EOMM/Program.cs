using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EOMM.Algorithms;
using EOMM.Matchmaking;
using EOMM.Models;
using EOMM.Simulation;

namespace EOMM {
  public static class Program {
    public static void Main() {
      List<Result> randomResults = new();
      List<Result> skillBasedResults = new();
      List<Result> worstResults = new();

      var header = $"{"Time",-10}  {"Name",-30}  {"Iteration",-9}  {"Retained",-8} ";
      Console.WriteLine(header);
      for (var i = 0; i < 3; i++) {
        randomResults.Add(ProcessResult(Simulate(() => new RandomMatchmaking()), i));
        skillBasedResults.Add(ProcessResult(Simulate(() => new SkillBasedMatchmaking()), i));
        worstResults.Add(ProcessResult(Simulate(() => new WorstMatchmaking()), i));
      }

      Console.WriteLine("\n");

      var resultsHeader = $"{"Name",-30}  {"Time",-10}  {"Retained",-8} ";
      Console.WriteLine(resultsHeader);
      Console.WriteLine(AverageResults(randomResults));
      Console.WriteLine(AverageResults(skillBasedResults));
      Console.WriteLine(AverageResults(worstResults));
    }


    private static string AverageResults(IReadOnlyCollection<Result> results) {
      var averageRetained = results.Average(r => r.Retained);
      var averageTime = results.Average(r => r.Time.TotalMilliseconds);
      var name = results.First().Name;

      var time = $"{averageTime:0.00} ms";

      var retained = $"{averageRetained:0.0000}";

      var values = $"{name,-30}  {time,-10}  {retained,-8} ";

      return values;
    }

    private static Result ProcessResult(Result result, int i) {
      var name = result.Name;
      var retainedPlayers = $"{result.Retained:0.0000}";
      var ms = $"{result.Time.TotalMilliseconds:0.00} ms";

      var message = $"{ms,-10}  {name,-30}  {i + 1,-9}  {retainedPlayers,-8} ";

      Console.WriteLine(message);

      return result;
    }

    private static Result Simulate(Func<Matchmaker> matchmaker) {
      var timer = new Stopwatch();
      timer.Start();
      var graph = new PlayerGraph();

      var retainedPlayers = new MatchSimulator(100, graph).Run(matchmaker());

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
