namespace HKW.HKWTOML.Tests;

public class TomlExample
{
    //public static string ExampleFile { get; set; } = "..\\..\\..\\Example.toml";
    public static string ExampleData =>
        @"
# This is an example HKWTOML document which shows most of its features.

# Simple key/value with a string.
title = ""HKWTOML example \U0001F60A""

desc = """"""
An example HKWTOML document. \
""""""

# Array with integers and floats in the various allowed formats.
integers = [42, 0x42, 0o42, 0b0110]
floats   = [1.42, 1e-02]

# Array with supported datetime formats.
times = [
	2021-11-09T15:16:17+01:00,  # datetime with timezone.
	2021-11-09T15:16:17Z,       # UTC datetime.
	2021-11-09T15:16:17,        # local datetime.
	2021-11-09,                 # local date.
	15:16:17,                   # local time.
]

# Durations.
duration = [""4m49s"", ""8m03s"", ""1231h15m55s""]

# Table with inline tables.
distros = [
	{name = ""Arch Linux"", packages = ""pacman""},
	{name = ""Void Linux"", packages = ""xbps""},
	{name = ""Debian"",     packages = ""apt""},
]

# Create new table; note the ""servers"" table is created implicitly.
[servers.alpha]
	# You can indent as you please, tabs or spaces.
	ip        = '10.0.0.1'
	hostname  = 'server1'
	enabled   = false
[servers.beta]
	ip        = '10.0.0.2'
	hostname  = 'server2'
	enabled   = true

# Start a new table array; note that the ""characters"" table is created implicitly.
[[characters.star-trek]]
	name = ""James Kirk""
	rank = ""Captain""
[[characters.star-trek]]
	name = ""Spock""
	rank = ""Science officer""

[undecoded] # To show the MetaData.Undecoded() feature.
	key = ""This table intentionally left undecoded""
";

    public static string ClassData =>
        @"/// <summary>
/// This is an example HKWTOML document which shows most of its features.
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
    [TomlPropertyName(""title"")]
    public string Title { get; set; }


    [TomlPropertyOrder(1)]
    [TomlPropertyName(""desc"")]
    public string Desc { get; set; }

    /// <summary>
    /// Array with integers and floats in the various allowed formats.
    /// </summary>
    [TomlPropertyOrder(2)]
    [TomlPropertyName(""integers"")]
    public List<int> Integers { get; set; }


    [TomlPropertyOrder(3)]
    [TomlPropertyName(""floats"")]
    public List<double> Floats { get; set; }

    /// <summary>
    /// Array with supported datetime formats.
    /// </summary>
    [TomlPropertyOrder(4)]
    [TomlPropertyName(""times"")]
    public List<TomlNode> Times { get; set; }

    /// <summary>
    /// Durations.
    /// </summary>
    [TomlPropertyOrder(5)]
    [TomlPropertyName(""duration"")]
    public List<string> Duration { get; set; }

    /// <summary>
    /// Table with inline tables.
    /// </summary>
    [TomlPropertyOrder(6)]
    [TomlPropertyName(""distros"")]
    public List<DistrosAnonymousClass> Distros { get; set; }


    [TomlPropertyOrder(7)]
    [TomlPropertyName(""servers"")]
    public ServersClass Servers { get; set; }


    [TomlPropertyOrder(8)]
    [TomlPropertyName(""characters"")]
    public CharactersClass Characters { get; set; }


    [TomlPropertyOrder(9)]
    [TomlPropertyName(""undecoded"")]
    public UndecodedClass Undecoded { get; set; }
}

public class DistrosAnonymousClass
{

    [TomlPropertyOrder(0)]
    [TomlPropertyName(""name"")]
    public string Name { get; set; }


    [TomlPropertyOrder(1)]
    [TomlPropertyName(""packages"")]
    public string Packages { get; set; }
}

public class ServersClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();


    /// <summary>
    /// Create new table; note the ""servers"" table is created implicitly.
    /// </summary>
    [TomlPropertyOrder(0)]
    [TomlPropertyName(""alpha"")]
    public AlphaClass Alpha { get; set; }


    [TomlPropertyOrder(1)]
    [TomlPropertyName(""beta"")]
    public BetaClass Beta { get; set; }
}

/// <summary>
/// Create new table; note the ""servers"" table is created implicitly.
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
    [TomlPropertyName(""ip"")]
    public string Ip { get; set; }


    [TomlPropertyOrder(1)]
    [TomlPropertyName(""hostname"")]
    public string Hostname { get; set; }


    [TomlPropertyOrder(2)]
    [TomlPropertyName(""enabled"")]
    public bool Enabled { get; set; }
}

public class BetaClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();



    [TomlPropertyOrder(0)]
    [TomlPropertyName(""ip"")]
    public string Ip { get; set; }


    [TomlPropertyOrder(1)]
    [TomlPropertyName(""hostname"")]
    public string Hostname { get; set; }


    [TomlPropertyOrder(2)]
    [TomlPropertyName(""enabled"")]
    public bool Enabled { get; set; }
}

public class CharactersClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();



    [TomlPropertyOrder(0)]
    [TomlPropertyName(""star-trek"")]
    public List<Star-trekAnonymousClass> Star-trek { get; set; }
}

public class Star-trekAnonymousClass
{

    [TomlPropertyOrder(0)]
    [TomlPropertyName(""name"")]
    public string Name { get; set; }


    [TomlPropertyOrder(1)]
    [TomlPropertyName(""rank"")]
    public string Rank { get; set; }
}

public class UndecodedClass : ITomlClassComment
{
    /// <inheritdoc/>
    public string ClassComment { get; set; } = string.Empty;
    /// <inheritdoc/>
    public Dictionary<string, string> ValueComments { get; set; } = new();



    [TomlPropertyOrder(0)]
    [TomlPropertyName(""key"")]
    public string Key { get; set; }
}

";
}
