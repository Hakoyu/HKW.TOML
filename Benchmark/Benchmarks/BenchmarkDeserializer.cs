using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using HKW.TOML;
using HKW.TOML.Deserializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Benchmark;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class BenchmarkDeserializer
{
    #region TOML
    [Benchmark]
    public object? HKWTomlDeserializer()
    {
        var table = TOML.Parse(TomlExample.ExampleData);
        return TomlDeserializer.Deserialize<ClassExample>(table);
    }
    #endregion
    #region JSON
    //[Benchmark]
    //public object? JsonDeserializer()
    //{
    //    return System.Text.Json.JsonSerializer.Deserialize<ClassExample>(
    //        JsonExample.ExampleData,
    //        new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
    //    )!;
    //}

    [Benchmark]
    public object? NewtonJsonDeserializer()
    {
        return JsonConvert.DeserializeObject<ClassExample>(JsonExample.ExampleData);
    }
    #endregion
}
