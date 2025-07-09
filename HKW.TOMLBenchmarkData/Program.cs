using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Running;
using HKW.HKWTOML.Benchmark.Benchmark;
using HKW.HKWTOML.ObjectBuilder;
using HKW.HKWTOML.Serializer;

namespace HKW.HKWTOML.Benchmark;

internal class Program
{
    static void Main(string[] args)
    {
#if DEBUG
        //BenchmarkSerialize.Test();
        //var obj = HKWTOML.Deserializer.TOMLDeserializer.Deserialize<ExampleObject>(
        //    TOML.Parse(TomlExample.ExampleData)
        //);
        //BenchmarkDeserialize.Test();
        //var parser = new Tomlet.TomlParser();
        //var toml = parser.Parse(BenchmarkParse.TomlData);
        //var toml = TOML.Parse(BenchmarkParse.TomlData);
        //Console.WriteLine(TOMLAsClasses.Generate(toml, "BenchmarkObject"));
        //var text = TOMLSerializer.Serialize(BenchmarkSerialize.Obj);
        //var toml = TOMLSerializer.Serialize(obj);
        //var length = 0;
        //for (var i = 0; i < 50; i++)
        //{
        //    var toml = TOMLSerializer.Serialize(BenchmarkSerialize.Obj);
        //    length += toml?.Comment?.Length ?? 1;
        //}
        var toml = TOMLSerializer.Serialize(BenchmarkSerialize.Obj);
#elif !DEBUG
        var length = 0;
        for (var i = 0; i < 50; i++)
        {
            var toml = TOMLSerializer.Serialize(BenchmarkSerialize.Obj);
            length += toml?.Comment?.Length ?? 1;
        }
        //var benchmark = BenchmarkRunner.Run<BenchmarkSerialize>();
        //var benchmark = BenchmarkRunner.Run<BenchmarkDeserialize>();
        //var obj = HKWTOML.Deserializer.TOMLDeserializer.Deserialize<BenchmarkObject>(
        //    TOML.Parse(BenchmarkParse.TomlData)
        //);
#endif
    }
}
