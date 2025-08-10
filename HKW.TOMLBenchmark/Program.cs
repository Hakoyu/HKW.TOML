using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BenchmarkDotNet.Running;
using HKW.HKWTOML.Benchmark.Benchmark;
using HKW.HKWTOML.Benchmark.Resources;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.ObjectGenerator;
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

        //using var sr = new StringReader(Example.TomlExampleBeautifulData);
        //var t = Tommy.TOML.Parse(sr);
        var tomlData = """    
            [clients]
            [clients.data]
            name = "Google"
            hosts = ["alpha", "omega"]

            [clients.hosts]
            alpha = "10.0.0.1"
            omega = "10.0.0.2"      
            """;
        //var parser = new Tomlet.TomlParser();
        //var tomlet = parser.Parse(Example.TomlExampleBeautifulData);
        //Console.WriteLine(tomlet.SerializedValue);
        //var tomlyn = Tomlyn.Toml.Parse(Example.TomlExampleBeautifulData);
        //Console.WriteLine(tomlyn.ToString());
        var toml = TOML.Parse(Example.TomlExampleData);
        Console.WriteLine(toml.ToTomlString());
        //var obj = TomlDeserializer.Deserialize<BenchmarkObject>(toml);
        //var str = toml.ToTomlStringAsync().GetAwaiter().GetResult();
        ////Console.WriteLine(str);
        //Console.WriteLine(
        //    TomlObjectGenerator.Generate(
        //        toml,
        //        "ExampleObject",
        //        new()
        //        {
        //            AddComment = true,
        //            AddITomlClassCommentInterface = true,
        //            AddTomlPropertyOrderAttribute = true,
        //            AddTomlPropertyNameAttribute = true,
        //            RemoveKeyWordSeparator = true,
        //        }
        //    )
        //);
        //toml.WriteToAsync(Console.Out, null!).GetAwaiter().GetResult();
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
        //var s = NativeResources.GetAllText(NativeResources.TomlData);
#elif !DEBUG
        //var toml = TOML.Parse(NativeResources.GetAllText(NativeResources.TomlData));
        //var obj = TomlDeserializer.Deserialize<BenchmarkObject>(toml);
        //var benchmark = BenchmarkRunner.Run(typeof(Program).Assembly);
        //var benchmark1 = BenchmarkRunner.Run<BenchmarkParse>();
        //var benchmark2 = BenchmarkRunner.Run<BenchmarkDeserialize>();
        //var benchmark3 = BenchmarkRunner.Run<BenchmarkSerialize>();
#endif
    }
}
