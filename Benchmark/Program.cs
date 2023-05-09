using Benchmark.Benchmarks;
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
        var test = BenchmarkRunner.Run<HKWTomlOnly>();
        //var test = BenchmarkRunner.Run<BenchmarkParse>();
        //var test = BenchmarkRunner.Run<BenchmarkDeserializer>();
#endif
    }
}
