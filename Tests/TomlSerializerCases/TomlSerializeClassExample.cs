#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML;
using HKW.TOML.TomlDeserializer;
using HKW.TOML.TomlSerializer;

namespace HKWToml.Tests.TomlSerializerCases;

internal static partial class TomlSerializeClassCases
{
    public static void DeserializeClassExample()
    {
        var table = TOML.Parse(TomlExample.ExampleData);
        var example = TomlDeserializer.Deserialize<ClassExample>(table);
        var serializeTable = TomlSerializer.Serialize(example);
        Console.WriteLine(serializeTable.ToTomlString());
    }
}

#endif