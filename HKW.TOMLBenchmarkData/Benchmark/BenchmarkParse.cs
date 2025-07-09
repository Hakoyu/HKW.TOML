using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using HKW.HKWTOML.Benchmark.Resources;
using Newtonsoft.Json.Linq;

namespace HKW.HKWTOML.Benchmark;

[MemoryDiagnoser]
public class BenchmarkParse
{
    public static string JsonData { get; } = NativeResources.GetAllText(NativeResources.JsonData);

    public static string TomlData { get; } = NativeResources.GetAllText(NativeResources.TomlData);

    [GlobalSetup]
    public void Initialize() { }

    [Benchmark]
    [IterationCount(10)]
    public object? Net_JsonParse()
    {
        return JsonNode.Parse(JsonData);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? Newtonsoft_JsonParse()
    {
        return JObject.Parse(JsonData);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? SimdJson_JsonParse()
    {
        return SimdJsonSharp.SimdJson.MinifyJson(JsonData);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? HKW_TomlParse()
    {
        return TOML.Parse(TomlData);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? Tomlyn_TomlParse()
    {
        return Tomlyn.Toml.Parse(TomlData);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? Tomlet_TomlParse()
    {
        var parser = new Tomlet.TomlParser();
        return parser.Parse(TomlData);
    }

    // ERROR
    //[Benchmark]
    //[IterationCount(10)]
    //public object? Tommy_TomlParse()
    //{
    //    using var sr = new StreamReader(TomlData);
    //    return Tommy.TOML.Parse(sr);
    //}
}
