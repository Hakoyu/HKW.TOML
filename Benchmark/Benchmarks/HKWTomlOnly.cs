using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using HKW.TOML;
using HKW.TOML.Deserializer;

namespace Benchmark.Benchmarks;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class HKWTomlOnly
{
    [Benchmark]
    public object? HKWTomlParse()
    {
        return TOML.Parse(TomlExample.ExampleData);
    }

    //[Benchmark]
    public object? HKWTomlDeserializer()
    {
        var table = TOML.Parse(TomlExample.ExampleData);
        return TomlDeserializer.Deserialize<ClassExample>(table);
    }
}
