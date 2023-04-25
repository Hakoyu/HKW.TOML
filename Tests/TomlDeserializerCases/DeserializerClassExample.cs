#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML;
using HKW.TOML.TomlDeserializer;

namespace HKWToml.Tests.TomlDeserializerCases;
internal static partial class TomlDeserializerCases
{
    public static void DeserializeClassExample()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        ClassExample example = TomlDeserializer.Deserialize<ClassExample>(table);
        Console.WriteLine(example);
    }
}
#endif
