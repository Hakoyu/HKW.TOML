using HKW.HKWTOML.Attributes;

namespace HKW.HKWTOML.Tests;

public static class ExampleStaticObject
{
    /// <summary>
    /// Simple key/value with a string.
    /// </summary>
    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("title")]
    public static string Title { get; set; }

    [TOMLPropertyOrder(1)]
    [TOMLPropertyName("desc")]
    public static string Desc { get; set; }

    /// <summary>
    /// Array with integers and floats in the various allowed formats.
    /// </summary>
    [TOMLPropertyOrder(2)]
    [TOMLPropertyName("integers")]
    public static List<int> Integers { get; set; }

    [TOMLPropertyOrder(3)]
    [TOMLPropertyName("floats")]
    public static List<double> Floats { get; set; }

    /// <summary>
    /// Array with supported datetime formats.
    /// </summary>
    [TOMLPropertyOrder(4)]
    [TOMLPropertyName("times")]
    public static List<TomlNode> Times { get; set; }

    /// <summary>
    /// Durations.
    /// </summary>
    [TOMLPropertyOrder(5)]
    [TOMLPropertyName("duration")]
    public static List<string> Duration { get; set; }

    /// <summary>
    /// Table with inline tables.
    /// </summary>
    [TOMLPropertyOrder(6)]
    [TOMLPropertyName("distros")]
    public static List<DistrosAnonymousClass> Distros { get; set; }

    [TOMLPropertyOrder(7)]
    [TOMLPropertyName("servers")]
    public static ServersClass Servers { get; set; }

    [TOMLPropertyOrder(8)]
    [TOMLPropertyName("characters")]
    public static CharactersClass Characters { get; set; }

    [TOMLPropertyOrder(9)]
    [TOMLPropertyName("undecoded")]
    public static UndecodedClass Undecoded { get; set; }
}
