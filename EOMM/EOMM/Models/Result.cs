using System;

namespace EOMM {
  public record Result {
    public Result(string name, int iteration, double retained, TimeSpan time) {
      Name = name;
      Iteration = iteration;
      Retained = retained;
      Time = time;
    }

    public string Name { get; }
    public int Iteration { get; }
    public double Retained { get; }
    public TimeSpan Time { get; }
  }
}
