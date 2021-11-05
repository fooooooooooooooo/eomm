// See https://aka.ms/new-console-template for more information

using System;

namespace EOMM {
  public static class Program {
    public static void Main(string[] args) {
      Console.WriteLine("Hello, World!");

      var graph = new PlayerGraph();

      // Retained players with random algorithm
      var retainedPlayers = new MatchSimulator(100, graph).Run(new RandomMatchmaking());

      Console.WriteLine($"Retained players: {retainedPlayers}");
    }
  }
}
