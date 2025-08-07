using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Benchmark;
using HKW.HKWTOML.ObjectGenerator;

namespace HKW.HKWTOML.Tests;

[TestClass]
public class TomlObjectGeneratorTests
{
    [TestMethod]
    public void ObjectGenerator()
    {
        var table = TOML.Parse(Example.TomlExampleData);
        string classString = TomlObjectGenerator
            .Generate(
                table,
                "ExampleObject",
                new()
                {
                    AddComment = true,
                    AddITomlClassCommentInterface = true,
                    AddTomlPropertyOrderAttribute = true,
                    AddTomlPropertyNameAttribute = true,
                    RemoveKeyWordSeparator = true,
                }
            )
            .Trim();
        Assert.AreEqual(Example.ClassData, classString);
    }
}
