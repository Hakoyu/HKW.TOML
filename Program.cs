// See https://aka.ms/new-console-template for more information
#if DEBUG
using System.Reflection;
using System.Diagnostics;
using HKW.TOML;
using HKW.TOML.Serializer;
using HKW.TOML.Deserializer;
using HKW.TOML.Attributes;
using HKW.TOML.Interfaces;
using HKW.TOML.Exceptions;
using HKW.TOML.AsClasses;
using HKWToml.Tests;
using HKWToml.Tests.AsClassesCases;
using HKWToml.Tests.DeserializerCases;
using HKWToml.Tests.SerializerCases;
using HKWToml.Tests.ParseCases;
using HKWToml.Utils;
using System.Text.RegularExpressions;
#endif

namespace HKWToml;

internal class HKWToml
{
    public static async Task Main(string[] args)
    {
#if DEBUG
        var table = TOML.Parse(TomlExample.ExampleData);
        var example1 = TomlDeserializer.Deserialize<ClassExample1>(
            table,
            new() { PropertyNameCaseInsensitive = false }
        );
        //ParseCases.ParseExampleFromFile();
        //AsClassesCases.CreateClassExample();
        //DeserializerCases.DeserializeClassExample();
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
    }
#endif
}
