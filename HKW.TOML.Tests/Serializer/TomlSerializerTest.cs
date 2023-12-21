using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Serializer;

namespace HKW.HKWTOML.Tests.Serializer;

[TestClass]
public class TomlSerializerTest
{
    [TestMethod]
    public void Deserialize()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        ExampleObject example = TOMLDeserializer.Deserialize<ExampleObject>(table);
        TomlTable serializeTable = TOMLSerializer.Serialize(example);
        //Console.WriteLine(serializeTable.ToTomlString());
    }
}
