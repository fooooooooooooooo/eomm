namespace EOMM {
  public static class Program {
    public static void Main() {
      Simulation.RunAsync().GetAwaiter().GetResult();
    }
  }
}
