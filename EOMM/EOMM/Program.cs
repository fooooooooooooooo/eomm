using System;

namespace EOMM {
  public static class Program {
    public static void Main(string[] args) {
      Console.WriteLine("Hello, World!");

      for (var i = 0; i < 100; i++) {
        Simulate(i);
      }
    }

    private static void Simulate(int iteration = 0) {
      var graph = new PlayerGraph();

      // Retained players with random algorithm
      var retainedPlayers = new MatchSimulator(100, graph).Run(new RandomMatchmaking());

      Console.WriteLine($"{iteration.ToString().PadRight(5)} | Retained players: {retainedPlayers}");
    }
  }
}
