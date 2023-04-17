// See https://aka.ms/new-console-template for more information
using System.Reflection;
using HKW.TOML;
using HKW.TOML.TomlSerializer;
using HKW.TOML.TomlDeserializer;
using HKW.TOML.TomlAttribute;
using System.Diagnostics;
using HKW.TOML.TomlInterface;
using HKW.TOML.TomlException;
using HKW.TOML.TomlAsClasses;
using HKWToml.Tests;
using HKWToml.Tests.TomlAsClassesCases;
using HKWToml.Tests.TomlDeserializerCases;

namespace HKWToml;
internal class HKWToml
{
    public static void Main(string[] args)
    {
#if DEBUG
        var table = TOML.Parse(TomlExample.Example0);
        TomlAsClassesCases.CreateClassExample();
        TomlDeserializerCases.DeserializeClassExample();
        //Console.WriteLine(table.ToTomlString());
        //try
        //{
        //    //var test = TomlDeserializer.DeserializeFromFile<Test>(file, new() { CheckConsistency = true });
        //    var test = TomlDeserializer.DeserializeFromFile<Test>(file);
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.ToString());
        //}
        //TomlSerializer.SerializeToFile(test, outFile);
        //var test1 = TomlDeserializer.Deserialize<Test1>(table["database"]["temp_targets"].AsTomlTable);
        //Console.WriteLine(TomlSerializer.Serialize(test).ToTomlString());
#endif
    }
}

