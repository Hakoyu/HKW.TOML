using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using HKW.TOML;
using Newtonsoft.Json.Linq;

namespace Benchmark.Benchmarks;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class BenchmarkParse
{
    #region TOML
    [Benchmark]
    public object? HKWTomlParse()
    {
        return TOML.Parse(TomlExample.ExampleData);
    }
    #endregion
    #region JSON
    [Benchmark]
    public object? JsonParse()
    {
        return JsonNode.Parse(JsonExample.ExampleData);
    }

    [Benchmark]
    public object? NewtonJsonParse()
    {
        return JObject.Parse(JsonExample.ExampleData);
    }
    #endregion
}
