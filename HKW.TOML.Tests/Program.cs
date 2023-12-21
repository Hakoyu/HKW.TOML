using BenchmarkDotNet.Running;
using HKW.HKWTOML.AsClasses;
using HKW.HKWTOML.Serializer;
using HKW.HKWTOML.Tests.Benchmark;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace HKW.HKWTOML.Tests;

internal class Program
{
    static void Main(string[] args)
    {
#if DEBUG
        //BenchmarkSerialize.Test();
        //var obj = HKWTOML.Deserializer.TOMLDeserializer.Deserialize<BenchmarkObject>(
        //    TOML.Parse(BenchmarkParse.TomlData)
        //);
        //BenchmarkDeserialize.Test();
        //var parser = new Tomlet.TomlParser();
        //var toml = parser.Parse(BenchmarkParse.TomlData);
        //var toml = TOML.Parse(BenchmarkParse.TomlData);
        //Console.WriteLine(TOMLAsClasses.Generate(toml, "BenchmarkObject"));
#elif !DEBUG
        //TOMLSerializer.Serialize(BenchmarkSerialize.Obj);
        //var benchmark = BenchmarkRunner.Run<BenchmarkDeserialize>();
        var benchmark = BenchmarkRunner.Run<BenchmarkSerialize>();
        //var obj = HKWTOML.Deserializer.TOMLDeserializer.Deserialize<BenchmarkObject>(
        //    TOML.Parse(BenchmarkParse.TomlData)
        //);
#endif
    }
}
