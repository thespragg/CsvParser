using CsvParser.Core;
using CsvParser.Core.Mutation;

namespace CsvParser.Test.Mutation;

public class CsvPipelineTests
{
    private static readonly string[] InputColumns = new[] { "in1", "in2" };

    [Fact]
    public void CsvPipeline_ReturnsExpected()
    {
        var pipeline = CsvPipeline.FromColumns("out1", "out2", "out3");
        pipeline.MapColumn("in1", "out1");
        pipeline.MapColumn("in2", "out2", template: "${value:upper}");
        pipeline.MapValue(InputColumns, "out3", "test", replacementCol: "in1",
            template: "${value:first:upper}", contains: true, caseSensitive: false);
        var result = pipeline.Run(new CsvDataWrapper(new List<Dictionary<string, string?>>
        {
            new() { { "in1", "test" }, { "in2", "item2" } },
            new() { { "in1", "purple" }, { "in2", "Test3sdadsa" } },
        }, new List<string> { "in1", "in2" }));

        const string expected = "out1,out2,out3\ntest,ITEM2,T\npurple,TEST3SDADSA,P\n";
        Assert.Equal(expected, result.ToString());
    }
}