// See https://aka.ms/new-console-template for more information
#if DEBUG
using System.Reflection;
using HKW.TOML;
using HKW.TOML.TomlSerializer;
using HKW.TOML.TomlDeserializer;
using HKW.TOML.TomlAttribute;
using System.Diagnostics;
using HKW.TOML.TomlInterface;
using HKW.TOML.TomlException;
using HKW.TOML.TomlAsClasses;
using HKWToml.Tests;
using HKWToml.Tests.TomlAsClassesCases;
using HKWToml.Tests.TomlDeserializerCases;
using HKWToml.Tests.TomlSerializerCases;
#endif

namespace HKWToml;
internal class HKWToml
{
    public static void Main(string[] args)
    {
#if DEBUG
        //var table = TOML.Parse(TomlExample.Example0);
        //TomlAsClassesCases.CreateClassExample();
        //TomlDeserializerCases.DeserializeClassExample();
        //TomlSerializeClassCases.DeserializeClassExample();
#endif
    }
}

