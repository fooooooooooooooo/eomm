using System;
using System.Collections.Generic;
using EOMM.Models;

namespace EOMM {
  public static class Extensions {
    public static MatchOutcome ToMatchOutcome(this int value) {
      return value switch {
        1 => MatchOutcome.Win,
        -1 => MatchOutcome.Loss,
        0 => MatchOutcome.Draw,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
      };
    }

    public static int ToInt(this MatchOutcome matchOutcome) {
      // -1 for loss, 0 for draw, 1 for win
      return matchOutcome switch {
        MatchOutcome.Win => 1,
        MatchOutcome.Loss => -1,
        MatchOutcome.Draw => 0,
        _ => throw new ArgumentOutOfRangeException(nameof(matchOutcome), matchOutcome, null)
      };
    }

    public static List<T> Fill<T>(this List<T> list, int amount, Func<T> generator) {
      for (var i = 0; i < amount; i++) {
        list.Add(generator());
      }

      return list;
    }

    public static T Pop<T>(this List<T> list) {
      var item = list[^1];
      list.RemoveAt(list.Count - 1);
      return item;
    }
  }
}
