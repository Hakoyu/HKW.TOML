using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML;
using HKW.TOML.Attributes;
using HKW.TOML.Interfaces;

namespace Benchmark;

/// <summary>
/// This is an example TOML document which shows most of its features.
/// </summary>

public class ClassExample
{
    /// <summary>
    /// Simple key/value with a string.
    /// </summary>
    public string Title { get; set; }
    public string Desc { get; set; }

    /// <summary>
    /// Array with integers and floats in the various allowed formats.
    /// </summary>
    public List<int> Integers { get; set; }
    public List<double> Floats { get; set; }

    /// <summary>
    /// Durations.
    /// </summary>
    public List<string> Duration { get; set; }

    /// <summary>
    /// Table with inline tables.
    /// </summary>
    public List<DistrosAnonymousClass> Distros { get; set; }
    public ServersClass Servers { get; set; }
    public CharactersClass Characters { get; set; }
    public UndecodedClass Undecoded { get; set; }
}

public class DistrosAnonymousClass
{
    public string Name { get; set; }
    public string Packages { get; set; }
}

public class ServersClass
{
    /// <summary>
    /// Create new _table; note the "servers" _table is created implicitly.
    /// </summary>
    public AlphaClass Alpha { get; set; }
    public BetaClass Beta { get; set; }
}

/// <summary>
/// Create new _table; note the "servers" _table is created implicitly.
/// </summary>

public class AlphaClass
{
    /// <summary>
    /// You can indent as you please, tabs or spaces.
    /// </summary>
    public string Ip { get; set; }
    public string Hostname { get; set; }
    public bool Enabled { get; set; }
}

public class BetaClass
{
    public string Ip { get; set; }
    public string Hostname { get; set; }
    public bool Enabled { get; set; }
}

public class CharactersClass
{
    public List<StarTrekAnonymousClass> StarTrek { get; set; }
}

public class StarTrekAnonymousClass
{
    public string Name { get; set; }
    public string Rank { get; set; }
}

public class UndecodedClass
{
    public string Key { get; set; }
}
