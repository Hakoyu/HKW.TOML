﻿#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML;
using HKW.TOML.TomlDeserializer;

namespace HKWToml.Tests.TomlDeserializerCases;
internal class TomlDeserializerCases
{
    public static void DeserializeClassExample()
    {
        var table = TOML.Parse(TomlExample.Example0);
        var example = TomlDeserializer.Deserialize<ClassExample>(table);
        Console.WriteLine(example);
    }
}
#endif