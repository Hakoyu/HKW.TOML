using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Benchmark;
using HKW.HKWTOML.Deserializer;

namespace HKW.HKWTOML.Tests;

[TestClass]
public class TomlDeserializerTest
{
    [TestMethod]
    public void Deserialize()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        var example = TOMLDeserializer.Deserialize<ExampleObject>(table);
        //Console.WriteLine(example);
    }

    [TestMethod]
    public void DeserializeStatic()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        table.KeyComparison = StringComparison.OrdinalIgnoreCase;
        var options = new TOMLDeserializerOptions() { AllowStaticProperty = true };
        TOMLDeserializer.Deserialize(typeof(ExampleStaticObject), table, options);
        //Console.WriteLine(example);
        Assert.IsTrue(ExampleStaticObject.Title == table[nameof(ExampleStaticObject.Title)]);
    }
}
