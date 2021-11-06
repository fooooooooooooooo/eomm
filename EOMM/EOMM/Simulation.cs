﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EOMM.Algorithms;
using EOMM.Matchmaking;
using EOMM.QuickGraph;

namespace EOMM {
  public static class Simulation {
    public static async Task RunAsync() {
      const int playerCount = 500;
      const int iterations = 100;

      Console.WriteLine("Simulating with " + playerCount + " players\n");

      var header = $"{"Time",-10}  {"Name",-32}  {"Iteration",-9}  {"Retained",-8} ";
      Console.WriteLine(header);

      // TODO : generate players in one place, then reuse them for all algorithms

      var randomParallelTask = RunMatchmaker(new RandomMatchmaking(), playerCount, iterations);
      var skillBasedParallelTask = RunMatchmaker(new SkillBasedMatchmaking(), playerCount, iterations);
      var worstParallelTask = RunMatchmaker(new WorstMatchmaking(), playerCount, iterations);
      var engagementOptimizedParallelTask =
        RunMatchmaker(new EngagementOptimizedMatchmaking(), playerCount, iterations);

      var results = await Task.WhenAll(
        randomParallelTask,
        skillBasedParallelTask,
        worstParallelTask,
        engagementOptimizedParallelTask
      );

      var randomResults = results[0];
      var skillBasedResults = results[1];
      var worstResults = results[2];
      var engagementOptimizedResults = results[3];

      Console.WriteLine("\n-- Averages --\n");

      Console.WriteLine(playerCount + " players\n");

      var resultsHeader = $"{"Name",-32}  {"Time",-10}  {"Retained",-8} ";
      Console.WriteLine(resultsHeader);

      Console.WriteLine(AverageResults(randomResults));
      Console.WriteLine(AverageResults(skillBasedResults));
      Console.WriteLine(AverageResults(worstResults));
      Console.WriteLine(AverageResults(engagementOptimizedResults));
    }

    private static async Task<List<Result>> RunMatchmaker(IMatchmaker matchmaker, int playerCount, int iterations) {
      var matchmakerTasks = Enumerable.Range(0, iterations)
        .Select(i => new Func<Task<Result>>(() => Simulate(playerCount, i, matchmaker)));

      var results = new List<Result>();

      await ParallelForEachAsync(matchmakerTasks, 100, async func => { results.Add(ProcessResult(await func())); });
      return results;
    }

    private static Task ParallelForEachAsync<T>(
      IEnumerable<T> source,
      int degreeOfParallelism,
      Func<T, Task> body) {
      async Task AwaitPartition(IEnumerator<T> partition) {
        using (partition) {
          while (partition.MoveNext()) {
            await body(partition.Current);
          }
        }
      }

      return Task.WhenAll(
        Partitioner
          .Create(source)
          .GetPartitions(degreeOfParallelism)
          .AsParallel()
          .Select(AwaitPartition));
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

    private static Result ProcessResult(Result result) {
      var name = result.Name;
      var retainedPlayers = $"{result.Retained:0.0000}";
      var ms = $"{result.Time.TotalMilliseconds:0.00} ms";

      var message = $"{ms,-10}  {name,-32}  {result.Iteration + 1,-9}  {retainedPlayers,-8} ";

      Console.WriteLine(message);

      return result;
    }

    private static async Task<Result> Simulate(int playerCount, int iteration, IMatchmaker matchmaker) {
      var timer = new Stopwatch();
      timer.Start();

      var retainedPlayers = await Task.Run(() => {
        var graph = new PlayerGraph();

        return new MatchSimulator(playerCount, graph).Run(matchmaker);
      });

      timer.Stop();

      return new Result(matchmaker.Name, iteration, retainedPlayers, timer.Elapsed);
    }
  }
}