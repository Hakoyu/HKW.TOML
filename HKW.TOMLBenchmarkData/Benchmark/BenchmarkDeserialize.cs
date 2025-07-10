using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace HKW.HKWTOML.Benchmark.Benchmark;

#pragma warning disable CA1822 // 将成员标记为 static

[MemoryDiagnoser]
public class BenchmarkDeserialize
{
    public static int Test()
    {
        var deserialize = new BenchmarkDeserialize();
        var count = 0;
        foreach (
            var method in typeof(BenchmarkDeserialize)
                .GetMethods()
                .Where(m => Attribute.IsDefined(m, typeof(BenchmarkAttribute)))
        )
        {
            if (method.Invoke(deserialize, null) is not null)
                count++;
        }
        return count;
    }

    [GlobalSetup]
    public void Initialize() { }

    System.Text.Json.JsonSerializerOptions options =
        new()
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

    //[Benchmark]
    //[IterationCount(10)]
    //public object? Net_JsonDeserialize()
    //{
    //    return System.Text.Json.JsonSerializer.Deserialize<BenchmarkObject>(
    //        BenchmarkParse.JsonData,
    //        options
    //    );
    //}

    //[Benchmark]
    //[IterationCount(10)]
    //public object? Newtonsoft_JsonDeserialize()
    //{
    //    return Newtonsoft.Json.JsonConvert.DeserializeObject<BenchmarkObject>(
    //        BenchmarkParse.JsonData
    //    );
    //}

    static TomlTable _hkwToml = TOML.Parse(BenchmarkParse.TomlData);

    [Benchmark]
    [IterationCount(10)]
    public object? HKW_TomlDeserialize()
    {
        return Deserializer.TomlDeserializer.Deserialize<BenchmarkObject>(_hkwToml);
    }

    //[Benchmark]
    //[IterationCount(10)]
    //public object? Tomlet_TomlDeserialize()
    //{
    //    return Tomlet.TomletMain.To<BenchmarkObject>(BenchmarkParse.TomlData);
    //}
}
#pragma warning restore CA1822 // 将成员标记为 static
