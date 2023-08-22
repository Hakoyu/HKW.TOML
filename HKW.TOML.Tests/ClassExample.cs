using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Interfaces;

namespace HKW.HKWTOML.Tests;

/// <summary>
/// This is an example TOML document which shows most of its features.
/// </summary>

public class ClassExample : ITOMLClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    /// <summary>
    /// Simple key/value with a string.
    /// </summary>
    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("title")]
    public string Title { get; set; }

    [TOMLPropertyOrder(1)]
    [TOMLPropertyName("desc")]
    public string Desc { get; set; }

    /// <summary>
    /// Array with integers and floats in the various allowed formats.
    /// </summary>
    [TOMLPropertyOrder(2)]
    [TOMLPropertyName("integers")]
    public List<int> Integers { get; set; }

    [TOMLPropertyOrder(3)]
    [TOMLPropertyName("floats")]
    public List<double> Floats { get; set; }

    /// <summary>
    /// Array with supported datetime formats.
    /// </summary>
    [TOMLPropertyOrder(4)]
    [TOMLPropertyName("times")]
    public List<TomlNode> Times { get; set; }

    /// <summary>
    /// Durations.
    /// </summary>
    [TOMLPropertyOrder(5)]
    [TOMLPropertyName("duration")]
    public List<string> Duration { get; set; }

    /// <summary>
    /// Table with inline tables.
    /// </summary>
    [TOMLPropertyOrder(6)]
    [TOMLPropertyName("distros")]
    public List<DistrosAnonymousClass> Distros { get; set; }

    [TOMLPropertyOrder(7)]
    [TOMLPropertyName("servers")]
    public ServersClass Servers { get; set; }

    [TOMLPropertyOrder(8)]
    [TOMLPropertyName("characters")]
    public CharactersClass Characters { get; set; }

    [TOMLPropertyOrder(9)]
    [TOMLPropertyName("undecoded")]
    public UndecodedClass Undecoded { get; set; }
}

public class DistrosAnonymousClass
{
    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("name")]
    public string Name { get; set; }

    [TOMLPropertyOrder(1)]
    [TOMLPropertyName("packages")]
    public string Packages { get; set; }
}

public class ServersClass : ITOMLClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    /// <summary>
    /// Create new table; note the "servers" table is created implicitly.
    /// </summary>
    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("alpha")]
    public AlphaClass Alpha { get; set; }

    [TOMLPropertyOrder(1)]
    [TOMLPropertyName("beta")]
    public BetaClass Beta { get; set; }
}

/// <summary>
/// Create new table; note the "servers" table is created implicitly.
/// </summary>

public class AlphaClass : ITOMLClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    /// <summary>
    /// You can indent as you please, tabs or spaces.
    /// </summary>
    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("ip")]
    public string Ip { get; set; }

    [TOMLPropertyOrder(1)]
    [TOMLPropertyName("hostname")]
    public string Hostname { get; set; }

    [TOMLPropertyOrder(2)]
    [TOMLPropertyName("enabled")]
    public bool Enabled { get; set; }
}

public class BetaClass : ITOMLClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("ip")]
    public string Ip { get; set; }

    [TOMLPropertyOrder(1)]
    [TOMLPropertyName("hostname")]
    public string Hostname { get; set; }

    [TOMLPropertyOrder(2)]
    [TOMLPropertyName("enabled")]
    public bool Enabled { get; set; }
}

public class CharactersClass : ITOMLClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("star-trek")]
    public List<StarTrekAnonymousClass> StarTrek { get; set; }
}

public class StarTrekAnonymousClass
{
    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("name")]
    public string Name { get; set; }

    [TOMLPropertyOrder(1)]
    [TOMLPropertyName("rank")]
    public string Rank { get; set; }
}

public class UndecodedClass : ITOMLClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();

    [TOMLPropertyOrder(0)]
    [TOMLPropertyName("key")]
    public string Key { get; set; }
}
