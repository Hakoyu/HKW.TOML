using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using HKW.HKWTOML.Serializer;
using Tomlet;

namespace HKW.HKWTOML.Benchmark.Benchmark;

[MemoryDiagnoser]
public class BenchmarkSerialize
{
    public static int Test()
    {
        var deserialize = new BenchmarkSerialize();
        var count = 0;
        foreach (
            var method in typeof(BenchmarkSerialize)
                .GetMethods()
                .Where(m => Attribute.IsDefined(m, typeof(BenchmarkAttribute)))
        )
        {
            if (method.Invoke(deserialize, null) is not null)
                count++;
        }
        return count;
    }

    public static BenchmarkObject Obj { get; } =
        Newtonsoft.Json.JsonConvert.DeserializeObject<BenchmarkObject>(BenchmarkParse.JsonData)!;

    [GlobalSetup]
    public void Initialize() { }

    [Benchmark]
    [IterationCount(10)]
    public object? Net_JsonSerialize()
    {
        return System.Text.Json.JsonSerializer.SerializeToNode(Obj);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? Newtonsoft_JsonSerialize()
    {
        return Newtonsoft.Json.Linq.JObject.FromObject(Obj);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? HKW_TomlSerialize()
    {
        return TomlSerializer.Serialize(Obj);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? Tomlet_TomlSerialize()
    {
        return TomletMain.DocumentFrom(Obj);
    }

    [Benchmark]
    [IterationCount(10)]
    public object? Tomlyn_TomlSerialize()
    {
        return Tomlyn.Toml.FromModel(Obj);
    }
}
