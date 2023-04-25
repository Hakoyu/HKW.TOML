// See https://aka.ms/new-console-template for more information
#if DEBUG
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
using HKWToml.Tests.TomlSerializerCases;
using HKWToml.Tests.TomlParseCases;
#endif

namespace HKWToml;

internal class HKWToml
{
    public static async Task Main(string[] args)
    {
#if DEBUG
        //TomlTable table = TOML.Parse(TomlExample.ExampleData);
        ////TomlTable table = TOML.ParseFromFile(TomlExample.ExampleFile);

        //string title = table["title"].AsString;
        //string titleComment = table["title"].Comment;
        //List<int> numbers = table["integers"].AsList.Select(node => node.AsInt32).ToList();


        TomlTable table =
            new()
            {
                new TomlString("TOML example \\U0001F60A")
                {
                    Comment = "Simple key/value with a string."
                },
                new TomlArray() { 42, 0x42, 042, 0b0110 }
            };
        //TomlParseCases.ParseExampleFromFile();
        //TomlAsClassesCases.CreateClassExample();
        //TomlDeserializerCases.DeserializeClassExample();
        //TomlSerializeClassCases.DeserializeClassExample();
#endif
    }
}
