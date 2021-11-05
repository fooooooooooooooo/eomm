using System;
using System.Linq;
using JetBrains.Annotations;

namespace EOMM {
  public class Matchmaking {
    /// <summary>
    ///   Predict win rate of <c>player 1</c>, against <c>player 2</c>.
    ///   Here we adopt the ELO algorithm for ease of computation.
    /// </summary>
    /// <param name="first">Player 1</param>
    /// <param name="second">Player 2</param>
    /// <returns>Player 1 win chance, Player 1 draw chance, Player 1 loss chance</returns>
    [PublicAPI]
    public static (double, double, double) PredictOutcome(Player first, Player second) {
      var firstWin = 1 / (1 + Math.Pow(10, ((double) second.Mmr - first.Mmr) / 400));
      const int firstDraw = 0;
      var firstLoss = 1 - firstWin - firstDraw;

      return (firstWin, firstDraw, firstLoss);
    }

    /// <summary>
    ///   To simplify the churn prediction of individual player,
    ///   the churn probability is set to be statistical churn_rate(*100%)
    ///   according to the latest+predicted match outcomes.
    /// </summary>
    /// <param name="player">The player</param>
    /// <param name="nextOutcome">Predicted match outcome</param>
    /// <returns>churn rate(*100%) of player with the given predicted outcome</returns>
    [PublicAPI]
    public static double PredictPlayerChurn(Player player, MatchOutcome nextOutcome) {
      var newWinHistory = player.WinHistory
        .ToList()
        .TakeLast(player.WinHistory.Length - 1)
        .Append(nextOutcome)
        .ToArray();

      var (a, b, c) = ((int) newWinHistory[0], (int) newWinHistory[1], (int) newWinHistory[2]);

      var churnRate = (a, b, c) switch {
        (1, 1, 1) => 37,
        (1, 1, -1) => 49,
        (1, -1, 1) => 46,
        (-1, 1, 1) => 43,
        (-1, 1, -1) => 37,
        (-1, -1, 1) => 27,
        (1, -1, -1) => 56,
        (-1, -1, -1) => 61,
        _ => 50
      };

      return churnRate;
    }

    /// <summary>
    ///   Predict the churn weight when a pair of players are matched together
    /// </summary>
    /// <param name="first">Player 1</param>
    /// <param name="second">Player 2</param>
    /// <returns>Pairwise churn weight</returns>
    [PublicAPI]
    public static double PredictPairChurn(Player first, Player second) {
      var (firstWin, firstDraw, firstLoss) = PredictOutcome(first, second);

      var p1Win = firstWin *
                  (PredictPlayerChurn(first, MatchOutcome.Win) + PredictPlayerChurn(second, MatchOutcome.Loss));

      const int draw = 0;

      var p2Win = firstLoss * (PredictPlayerChurn(first, MatchOutcome.Loss) + PredictPlayerChurn
        (second, MatchOutcome.Win));

      var pairChurn = p1Win + draw + p2Win;

      return pairChurn;
    }
  }
}
