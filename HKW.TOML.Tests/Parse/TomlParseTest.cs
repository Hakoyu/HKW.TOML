using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML;

namespace HKW.HKWTOML.Tests.Parse;

[TestClass]
public class TomlParseTest
{
    [TestMethod]
    public void ParseExample()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        //Console.WriteLine(table.ToTomlString());
    }

    //public static void ParseExampleFromFile()
    //{
    //    TomlTable table = TOML.ParseFromFile(TomlExample.ExampleFile);
    //    Console.WriteLine(table.ToTomlString());
    //}
}
