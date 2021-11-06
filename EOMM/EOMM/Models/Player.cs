using System;
using System.Collections.Generic;
using System.Linq;

namespace EOMM.Models {
  public class Player {
    public Player(Guid id) {
      var r = new Random();
      Id = id;
      Mmr = r.Next(1, 100);

      var choices = new[] {-1, 1};

      WinHistory = new[] {
        choices[r.Next(choices.Length)].ToMatchOutcome(),
        choices[r.Next(choices.Length)].ToMatchOutcome(),
        choices[r.Next(choices.Length)].ToMatchOutcome()
      };
    }

    public Player() {
      var r = new Random();
      Id = Guid.NewGuid();
      Mmr = r.Next(1, 100);

      var choices = new[] {-1, 1};

      WinHistory = new[] {
        choices[r.Next(choices.Length)].ToMatchOutcome(),
        choices[r.Next(choices.Length)].ToMatchOutcome(),
        choices[r.Next(choices.Length)].ToMatchOutcome()
      };
    }

    public Player(Guid id, int mmr, MatchOutcome[] winHistory) {
      Id = id;
      Mmr = mmr;
      WinHistory = winHistory;
    }

    public Guid Id { get; }
    public int Mmr { get; set; }
    public MatchOutcome[] WinHistory { get; }

    public static Player GeneratePlayer() {
      return new Player();
    }

    public static Player? FindById(IEnumerable<Player> players, Guid id) {
      return players.FirstOrDefault(p => p.Id == id);
    }
  }
}
