using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Benchmark;

namespace HKW.HKWTOML.Tests;

[TestClass]
public class TomlParseTests
{
    [TestMethod]
    public void Parse()
    {
        var table = TOML.Parse(Example.TomlExampleData);
        var tomlString = table.ToTomlString();
        Assert.AreEqual(tomlString, Example.TomlExampleData);
    }

    [TestMethod]
    public void ParseAsync()
    {
        var table = TOML.ParseAsync(Example.TomlExampleData).GetAwaiter().GetResult();
        var tomlString = table.ToTomlString();
        Assert.AreEqual(tomlString, Example.TomlExampleData);
    }

    [TestMethod]
    public void ToTomlStringAsync()
    {
        var table = TOML.Parse(Example.TomlExampleData);
        var tomlString = table.ToTomlStringAsync().GetAwaiter().GetResult();
        Assert.AreEqual(tomlString, Example.TomlExampleData);
    }
}
