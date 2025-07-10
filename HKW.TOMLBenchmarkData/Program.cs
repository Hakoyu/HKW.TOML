using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
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
        var toml = TOML.Parse(TomlExample.ExampleData);
        toml.WriteToAsync(Console.Out, null!).GetAwaiter().GetResult();
        //Console.WriteLine(TOMLAsClasses.Generate(toml, "BenchmarkObject"));
        //var text = TOMLSerializer.Serialize(BenchmarkSerialize.Obj);
        //var toml = TOMLSerializer.Serialize(obj);
        //var length = 0;
        //for (var i = 0; i < 50; i++)
        //{
        //    var toml = TOMLSerializer.Serialize(BenchmarkSerialize.Obj);
        //    length += toml?.Comment?.Length ?? 1;
        //}
        //var toml = TomlSerializer.Serialize(BenchmarkSerialize.Obj);
#elif !DEBUG

        //var length = 0;
        //for (var i = 0; i < 10; i++)
        //{
        //    //var toml = TOMLSerializer.Serialize(BenchmarkSerialize.Obj);
        //    var toml = Deserializer.TOMLDeserializer.Deserialize<BenchmarkObject>(
        //        BenchmarkParse.TomlData
        //    );
        //    length += toml?.GetHashCode() ?? 1;
        //}
        //var benchmark2 = BenchmarkRunner.Run<BenchmarkDeserialize>();
        //var benchmark1 = BenchmarkRunner.Run<BenchmarkSerialize>();
        var benchmark3 = BenchmarkRunner.Run<BenchmarkParse>();

#endif
    }
}
