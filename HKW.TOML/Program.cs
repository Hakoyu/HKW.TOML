#if DEBUG
using HKW.HKWTOML;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Interfaces;
using HKW.HKWTOML.Serializer;
#endif

namespace HKWTOML;

internal class HKWToml
{
    public static async Task Main(string[] args)
    {
#if DEBUG
        var toml = TOML.Parse("TestEnum = \"Value1\"\nTestEnums = [\"Value1\",\"Value2\"]");
        var test = TOMLDeserializer.Deserialize<TestClass>(toml);
        Console.WriteLine(TOMLSerializer.Serialize(test).ToTomlString());
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
        public string ClassComment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public Dictionary<string, string> ValueComments { get; set; } = new();

        /// <summary>
        /// Simple key/value with a string.
        /// </summary>
        [TOMLPropertyOrder(0)]
        public string Title { get; set; }

        [TOMLPropertyOrder(1)]
        [TOMLPropertyName("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// Array with integers and floats in the various allowed formats.
        /// </summary>
        [TOMLPropertyOrder(2)]
        [TOMLPropertyName("integers")]
        public List<int> Integers { get; set; }

        [TOMLPropertyOrder(3)]
        [TOMLPropertyName("floats")]
        public List<double> Floats { get; set; }

        /// <summary>
        /// Array with supported datetime formats.
        /// </summary>
        [TOMLPropertyOrder(4)]
        [TOMLPropertyName("times")]
        public List<TomlNode> Times { get; set; }

        /// <summary>
        /// Durations.
        /// </summary>
        [TOMLPropertyOrder(5)]
        [TOMLPropertyName("duration")]
        public List<string> Duration { get; set; }
    }
#endif
}
