#if DEBUG
using HKW.HKWTOML;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Interfaces;
using HKW.HKWTOML.Serializer;
using System.Reflection;
#endif

namespace HKW.HKWTOML;

internal class HKWToml
{
    public static async Task Main(string[] args)
    {
#if DEBUG
        //var toml = TOML.Parse("TestEnum = \"Value1\"\nTestEnums = [\"Value1\",\"Value2\"]");
        //var test = TOMLDeserializer.Deserialize<TestClass>(toml);
        //Console.WriteLine(TOMLSerializer.Serialize(test).ToTomlString());
        //var table = TOML.Parse(TomlExample.ExampleData);
        //var example1 = TomlDeserializer.Deserialize<ClassExample1>(
        //    table,
        //    new() { PropertyNameCaseInsensitive = false }
        //);
        //TomlParseCases.ParseExampleFromFile();
        //TomlAsClassesCases.CreateClassExample();
        //TomlDeserializerCases.DeserializeClassExample();
        //TomlSerializeClassCases.DeserializeClassExample();
#endif
    }

#if DEBUG
    [AttributeUsage(AttributeTargets.Class)]
    public class TestA : Attribute
    {
        public List<string> Args = new();
    }

    [TestA]
    public class TestClass
    {
        public TestEnum TestEnum { get; set; }

        public List<TestEnum> TestEnums { get; set; } = new();
    }

    public enum TestEnum
    {
        Value1,
        Value2,
        Value3,
    }

    public class ClassExample1 : ITomlObjectComment
    {
        /// <inheritdoc/>
        public string ObjectComment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public Dictionary<string, string> PropertyComments { get; set; } = new();

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

        public string CantWrite { get; } = "No set property";
    }
#endif
}
