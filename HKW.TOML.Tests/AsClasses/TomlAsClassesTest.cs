﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML;
using HKW.HKWTOML.AsClasses;

namespace HKW.HKWTOML.Tests.AsClasses;

[TestClass]
public class TomlAsClassesTest
{
    [TestMethod]
    public void AsClasses()
    {
        var table = TOML.Parse(TomlExample.ExampleData);
        string classString = TomlAsClasses.Generate(
            table,
            "ClassExample",
            new()
            {
                AddComment = true,
                AddITomlClassCommentInterface = true,
                AddTomlPropertyOrderAttribute = true,
                AddTomlPropertyNameAttribute = true,
                RemoveKeyWordSeparator = true,
            }
        );
        Assert.IsTrue(classString.Length == TomlExample.ClassData.Length);
    }
}
