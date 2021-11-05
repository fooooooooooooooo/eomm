using System;
using System.Collections.Generic;

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

    public static IList<T> Fill<T>(this IList<T> list, int amount, Func<T> generator) {
      for (var i = 0; i < amount; i++) {
        list.Add(generator());
      }

      return list;
    }

    public static T Pop<T>(this IList<T> list) {
      var item = list[^1];
      list.RemoveAt(list.Count - 1);
      return item;
    }
  }
}
