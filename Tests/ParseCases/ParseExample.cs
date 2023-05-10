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
    public static void ParseExample()
    {
        TomlTable table = TOML.Parse(TomlExample.ExampleData);
        Console.WriteLine(table.ToTomlString());
    }
}
#endif
