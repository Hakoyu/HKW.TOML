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
using HKWTOML.Tests;
using HKWTOML.Tests.AsClassesCases;
using HKWTOML.Tests.DeserializerCases;
using HKWTOML.Tests.SerializerCases;
using HKWTOML.Tests.ParseCases;
using HKWTOML.Utils;
using System.Text.RegularExpressions;
using System.Collections.Generic;
#endif
using System.Threading.Tasks;

namespace HKWTOML;

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
