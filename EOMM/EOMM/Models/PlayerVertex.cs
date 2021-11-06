using System;

namespace EOMM.Models {
  public class PlayerVertex {
    public PlayerVertex() {
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

    public PlayerVertex(Guid id, int mmr, MatchOutcome[] winHistory) {
      Id = id;
      Mmr = mmr;
      WinHistory = winHistory;
    }

    public Guid Id { get; }
    public int Mmr { get; set; }
    public MatchOutcome[] WinHistory { get; }
  }
}