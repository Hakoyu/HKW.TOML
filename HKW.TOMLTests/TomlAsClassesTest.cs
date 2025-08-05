using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Benchmark;
using HKW.HKWTOML.ObjectBuilder;

namespace HKW.HKWTOML.Tests;

[TestClass]
public class TomlAsClassesTest
{
    [TestMethod]
    public void AsClasses()
    {
        var table = TOML.Parse(Example.TomlExampleData);
        string classString = ObjectBuilder
            .ObjectBuilder.Generate(
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
