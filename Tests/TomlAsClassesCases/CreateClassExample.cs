﻿#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML;
using HKW.TOML.TomlAsClasses;

namespace HKWToml.Tests.TomlAsClassesCases;

internal partial class TomlAsClassesCases
{
    public static void CreateClassExample()
    {
        var table = TOML.Parse(TomlExample.Example0);
        var classString = TomlAsClasses.Generate(table, "ClassExample", new()
        {
            AddComment = true,
            AddITomlClassCommentInterface = true,
            AddTomlPropertyOrderAttribute = true,
            AddTomlPropertyNameAttribute = true,
            RemoveKeyWordSeparators = true,
            KeyWordSeparators = new() { '-' },
        });
        Console.WriteLine(classString);
    }
}
/*
 * output:
/// <summary>
/// This is an example TOML document which shows most of its features.
/// </summary>

public class ClassExample : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    /// <summary>
    /// Simple key/value with a string.
    /// </summary>
    [TomlPropertyOrder(0)]
    [TomlPropertyName("title")]
    public string Title { get; set; }
    [TomlPropertyOrder(1)]
    [TomlPropertyName("desc")]
    public string Desc { get; set; }
    /// <summary>
    /// Array with integers and floats in the various allowed formats.
    /// </summary>
    [TomlPropertyOrder(2)]
    [TomlPropertyName("integers")]
    public List<int> Integers { get; set; }
    [TomlPropertyOrder(3)]
    [TomlPropertyName("floats")]
    public List<double> Floats { get; set; }
    /// <summary>
    /// Array with supported datetime formats.
    /// </summary>
    [TomlPropertyOrder(4)]
    [TomlPropertyName("times")]
    public List<TomlNode> Times { get; set; }
    /// <summary>
    /// Durations.
    /// </summary>
    [TomlPropertyOrder(5)]
    [TomlPropertyName("duration")]
    public List<string> Duration { get; set; }
    /// <summary>
    /// Table with inline tables.
    /// </summary>
    [TomlPropertyOrder(6)]
    [TomlPropertyName("distros")]
    public List<DistrosAnonymousClass> Distros { get; set; }
    [TomlPropertyOrder(7)]
    [TomlPropertyName("servers")]
    public ServersClass Servers { get; set; }
    [TomlPropertyOrder(8)]
    [TomlPropertyName("characters")]
    public CharactersClass Characters { get; set; }
    [TomlPropertyOrder(9)]
    [TomlPropertyName("undecoded")]
    public UndecodedClass Undecoded { get; set; }
}

public class DistrosAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    public string Name { get; set; }
    [TomlPropertyOrder(1)]
    [TomlPropertyName("packages")]
    public string Packages { get; set; }
}

public class ServersClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    /// <summary>
    /// Create new table; note the "servers" table is created implicitly.
    /// </summary>
    [TomlPropertyOrder(0)]
    [TomlPropertyName("alpha")]
    public AlphaClass Alpha { get; set; }
    [TomlPropertyOrder(1)]
    [TomlPropertyName("beta")]
    public BetaClass Beta { get; set; }
}

/// <summary>
/// Create new table; note the "servers" table is created implicitly.
/// </summary>

public class AlphaClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    /// <summary>
    /// You can indent as you please, tabs or spaces.
    /// </summary>
    [TomlPropertyOrder(0)]
    [TomlPropertyName("ip")]
    public string Ip { get; set; }
    [TomlPropertyOrder(1)]
    [TomlPropertyName("hostname")]
    public string Hostname { get; set; }
    [TomlPropertyOrder(2)]
    [TomlPropertyName("enabled")]
    public bool Enabled { get; set; }
}

public class BetaClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("ip")]
    public string Ip { get; set; }
    [TomlPropertyOrder(1)]
    [TomlPropertyName("hostname")]
    public string Hostname { get; set; }
    [TomlPropertyOrder(2)]
    [TomlPropertyName("enabled")]
    public bool Enabled { get; set; }
}

public class CharactersClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("star-trek")]
    public List<StarTrekAnonymousClass> StarTrek { get; set; }
}

public class StarTrekAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    public string Name { get; set; }
    [TomlPropertyOrder(1)]
    [TomlPropertyName("rank")]
    public string Rank { get; set; }
}

public class UndecodedClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("key")]
    public string Key { get; set; }
}

 */
#endif