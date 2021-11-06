using System.Collections.Generic;
using EOMM.Models;
using EOMM.QuickGraph;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace EOMM.Tests {
  public class GraphTests {
    private readonly ITestOutputHelper _testOutputHelper;

    public GraphTests(ITestOutputHelper testOutputHelper) {
      _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestMaxMatching() {
      var players = new List<PlayerVertex>();
      players.AddRange(new[] {
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex(),
        new PlayerVertex()
      });

      var graph = new PlayerGraph();
      graph.AddPlayers(players);
      graph.GenerateEdges();

      // Assert.True(graph.Vertices.Count() == 10);

      var matching = PlayerGraph.MaxMatching(graph, edge => edge.ChurnWeight);

      // log all the matching pairs as json
      var json = JsonConvert.SerializeObject(matching, Formatting.Indented);
      _testOutputHelper.WriteLine(json);
    }
  }
}
