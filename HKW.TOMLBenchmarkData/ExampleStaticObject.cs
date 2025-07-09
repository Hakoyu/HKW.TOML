using HKW.HKWTOML.Attributes;

namespace HKW.HKWTOML.Benchmark;

public static class ExampleStaticObject
{
    /// <summary>
    /// Simple key/value with a string.
    /// </summary>
    [TomlPropertyOrder(0)]
    [TomlPropertyName("title")]
    public static string Title { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("desc")]
    public static string Desc { get; set; }

    /// <summary>
    /// Array with integers and floats in the various allowed formats.
    /// </summary>
    [TomlPropertyOrder(2)]
    [TomlPropertyName("integers")]
    public static List<int> Integers { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("floats")]
    public static List<double> Floats { get; set; }

    /// <summary>
    /// Array with supported datetime formats.
    /// </summary>
    [TomlPropertyOrder(4)]
    [TomlPropertyName("times")]
    public static List<TomlNode> Times { get; set; }

    /// <summary>
    /// Durations.
    /// </summary>
    [TomlPropertyOrder(5)]
    [TomlPropertyName("duration")]
    public static List<string> Duration { get; set; }

    /// <summary>
    /// Table with inline tables.
    /// </summary>
    [TomlPropertyOrder(6)]
    [TomlPropertyName("distros")]
    public static List<DistrosAnonymousClass> Distros { get; set; }

    [TomlPropertyOrder(7)]
    [TomlPropertyName("servers")]
    public static ServersClass Servers { get; set; }

    [TomlPropertyOrder(8)]
    [TomlPropertyName("characters")]
    public static CharactersClass Characters { get; set; }

    [TomlPropertyOrder(9)]
    [TomlPropertyName("undecoded")]
    public static UndecodedClass Undecoded { get; set; }
}
