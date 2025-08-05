using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Benchmark;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Serializer;

namespace HKW.HKWTOML.Tests;

[TestClass]
public class TomlSerializerTest
{
    [TestMethod]
    public void Serialize()
    {
        var example = TomlDeserializer.Deserialize<ExampleObject>(Example.TomlExampleData);
        var serializeTable = TomlSerializer.Serialize(example);
        var table = TOML.Parse(Example.TomlExampleData);
        var serializeTableString = serializeTable!.ToTomlString();
        var tableString = table.ToTomlString();
        Assert.AreEqual(serializeTableString, tableString);
        //Console.WriteLine(serializeTable.ToTomlString());
    }

    [TestMethod]
    public void SerializeAsync()
    {
        var example = TomlDeserializer.Deserialize<ExampleObject>(Example.TomlExampleData);
        var serializeTable = TomlSerializer.SerializeAsync(example).GetAwaiter().GetResult();
        var table = TOML.Parse(Example.TomlExampleData);
        var serializeTableString = serializeTable!.ToTomlString();
        var tableString = table.ToTomlString();
        Assert.AreEqual(serializeTableString, tableString);
        //Console.WriteLine(serializeTable.ToTomlString());
    }
}
