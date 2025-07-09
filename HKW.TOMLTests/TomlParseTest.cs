using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Benchmark;

namespace HKW.HKWTOML.Tests;

[TestClass]
public class TomlParseTest
{
    [TestMethod]
    public void ParseExample()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        //Console.WriteLine(table.ToTomlString());
    }
}
