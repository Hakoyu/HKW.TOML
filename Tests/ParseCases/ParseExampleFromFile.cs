#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML;

namespace HKWTOML.Tests.ParseCases;

internal static partial class TomlParseCases
{
    public static void ParseExampleFromFile()
    {
        TomlTable table = TOML.ParseFromFile(TomlExample.ExampleFile);
        Console.WriteLine(table.ToTomlString());
    }
}
#endif
