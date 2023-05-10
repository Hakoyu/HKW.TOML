#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML;
using HKW.TOML.Deserializer;
using HKW.TOML.Serializer;

namespace HKWTOML.Tests.SerializerCases;

internal static partial class TomlSerializeClassCases
{
    public static void DeserializeClassExample()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        ClassExample example = TomlDeserializer.Deserialize<ClassExample>(table);
        TomlTable serializeTable = TomlSerializer.Serialize(example);
        Console.WriteLine(serializeTable.ToTomlString());
    }
}

#endif
