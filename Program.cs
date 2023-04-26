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
//using HKWToml.Tests;
using HKWToml.Tests.TomlAsClassesCases;
using HKWToml.Tests.TomlDeserializerCases;
using HKWToml.Tests.TomlSerializerCases;
using HKWToml.Tests.TomlParseCases;
using HKWToml.Tests;
using HKWToml.Utils;
#endif

namespace HKWToml;

internal class HKWToml
{
    public static async Task Main(string[] args)
    {
#if DEBUG
        var table = TOML.Parse(TomlExample.ExampleData);
        var example1 = TomlDeserializer.Deserialize<ClassExample1>(table);
        //TomlParseCases.ParseExampleFromFile();
        //TomlAsClassesCases.CreateClassExample();
        //TomlDeserializerCases.DeserializeClassExample();
        //TomlSerializeClassCases.DeserializeClassExample();
#endif
    }
#if DEBUG

    public class ClassExample1 : ITomlClassComment
    {
        /// <inheritdoc/>
        public string ClassComment { get; set; } = string.Empty;
        /// <inheritdoc/>
        public Dictionary<string, string> ValueComments { get; set; } = new();

        /// <summary>
        /// Simple key/value with a string.
        /// </summary>
        [TomlPropertyOrder(0)]
        [TomlPropertyName("title")]
        public string Title { get; set; }
        [TomlPropertyOrder(1)]
        [TomlPropertyName("desc")]
        public string Desc { get; set; }
        /// <summary>
        /// Array with integers and floats in the various allowed formats.
        /// </summary>
        [TomlPropertyOrder(2)]
        [TomlPropertyName("integers")]
        public List<int> Integers { get; set; }
        [TomlPropertyOrder(3)]
        [TomlPropertyName("floats")]
        public List<double> Floats { get; set; }
        /// <summary>
        /// Array with supported datetime formats.
        /// </summary>
        [TomlPropertyOrder(4)]
        [TomlPropertyName("times")]
        public List<TomlNode> Times { get; set; }
        /// <summary>
        /// Durations.
        /// </summary>
        [TomlPropertyOrder(5)]
        [TomlPropertyName("duration")]
        public List<string> Duration { get; set; }

        [RunOnTomlDeserialized]
        internal void Noop()
        {
            Title = "114514";
        }

        [RunOnTomlDeserialized]
        internal static void Noop1()
        {
            Console.WriteLine(114514);
        }
    }
#endif
}
