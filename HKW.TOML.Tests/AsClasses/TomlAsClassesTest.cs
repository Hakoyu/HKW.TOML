﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.ObjectBuilder;

namespace HKW.HKWTOML.Tests.AsClasses;

[TestClass]
public class TomlAsClassesTest
{
    [TestMethod]
    public void AsClasses()
    {
        var table = TOML.Parse(TomlExample.ExampleData);
        string classString = ObjectBuilder.ObjectBuilder.Generate(
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
        );
        Assert.IsTrue(classString == TomlExample.ClassData);
    }
}
