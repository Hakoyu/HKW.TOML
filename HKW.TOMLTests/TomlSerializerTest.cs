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
    public void Deserialize()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        ExampleObject example = TomlDeserializer.Deserialize<ExampleObject>(table);
        TomlTable serializeTable = TomlSerializer.Serialize(example);
        //Console.WriteLine(serializeTable.ToTomlString());
    }
}
