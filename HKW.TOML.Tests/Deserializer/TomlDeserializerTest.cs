using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML;
using HKW.HKWTOML.Deserializer;

namespace HKW.HKWTOML.Tests.Deserializer;

[TestClass]
public class TomlDeserializerTest
{
    [TestMethod]
    public void Deserialize()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        ClassExample example = TOMLDeserializer.Deserialize<ClassExample>(table);
        //Console.WriteLine(example);
    }
}
