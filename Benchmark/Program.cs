using Benchmark.HKWTOMLOnly;
using BenchmarkDotNet.Running;

namespace Benchmark;

internal class Program
{
    public static void Main(string[] args)
    {
#if DEBUG
        var test = new BenchmarkDeserializer();
        test.NewtonJsonDeserializer();
        //test.JsonParse();
#elif !DEBUG
        var test = BenchmarkRunner.Run<HKWTOMLDeserializerBenchmark>();
        //var test = BenchmarkRunner.Run<BenchmarkParse>();
        //var test = BenchmarkRunner.Run<BenchmarkDeserializer>();
#endif
    }
}
