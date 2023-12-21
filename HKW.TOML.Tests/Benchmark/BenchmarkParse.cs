using BenchmarkDotNet.Attributes;
using HKW.HKWTOML.Tests.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace HKW.HKWTOML.Tests;

[MemoryDiagnoser]
public class BenchmarkParse
{
    public static string JsonData { get; } = NativeResources.GetAllText(NativeResources.JsonData);

    public static string TomlData { get; } = NativeResources.GetAllText(NativeResources.TomlData);

    [GlobalSetup]
    public void Initialize() { }

    [Benchmark]
    public object? Net_JsonParse()
    {
        return JsonNode.Parse(JsonData);
    }

    [Benchmark]
    public object? Newtonsoft_JsonParse()
    {
        return JObject.Parse(JsonData);
    }

    [Benchmark]
    public object? SimdJson_JsonParse()
    {
        return SimdJsonSharp.SimdJson.MinifyJson(JsonData);
    }

    [Benchmark]
    public object? HKW_TomlParse()
    {
        return TOML.Parse(TomlData);
    }

    [Benchmark]
    public object? Tomlyn_TomlParse()
    {
        return Tomlyn.Toml.Parse(TomlData);
    }

    [Benchmark]
    public object? Tomlet_TomlParse()
    {
        var parser = new Tomlet.TomlParser();
        return parser.Parse(TomlData);
    }

    // ERROR
    //[Benchmark]
    //public object? Tommy_TomlParse()
    //{
    //    using var sr = new StreamReader(TomlData);
    //    return Tommy.TOML.Parse(sr);
    //}
}
