// See https://aka.ms/new-console-template for more information
using System.Reflection;
using HKW.TOML;
namespace HKWToml;

internal class HKWToml
{
    public static void Main(string[] args)
    {
        System.Diagnostics.Stopwatch stopWatch = new();
        stopWatch.Start();
        var file = "C:\\Users\\HKW\\Desktop\\Dotnet\\test.toml";
        var sFile = "C:\\Users\\HKW\\Desktop\\Dotnet\\allTest.toml";
        var outFile = "C:\\Users\\HKW\\Desktop\\Dotnet\\test1.toml";
        //var i = (int)long.MaxValue;
        //int i = ((TomlNode)(float)114.514).AsInt32;
        //var classString = TomlAsClasses.Construct(
        //    "Test",
        //    TOML.Parse(file),
        //    new()
        //    {
        //        AddComment = true,
        //        AddITomlClassInterface = true,
        //        AddTomlParameterOrderAttribute = true,
        //        KeyNameConverterFunc = (s) => s + "999"
        //    }
        //);
        //Console.WriteLine(classString);

        //var table = TOML.Parse(file);
        //Console.WriteLine(table.ToTomlString());
        //var test = TomlDeserializer.DeserializeFromFile<Test>(file);
        //TomlSerializer.SerializeToFile(test, outFile);
        //var test1 = TomlDeserializer.Deserialize<Test1>(table["database"]["temp_targets"].AsTomlTable);
        //Console.WriteLine(test);
        stopWatch.Stop();
        Console.WriteLine($"\nSTOP {stopWatch.Elapsed.TotalMilliseconds.ToString():f4}ms");
    }
    public class Noop : IComparer<PropertyInfo>
    {
        public int Compare(PropertyInfo? x, PropertyInfo? y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }

    public class Test : ITomlClassComment
    {
        public string ClassComment { get; set; } = string.Empty;
        public Dictionary<string, string> ValueComments { get; set; } = new();

        [TomlSortOrder(0)]
        [TomlKeyName("title")]
        public string AAA { get; set; }
        public int Int1 { get; set; }
        public long Long1 { get; set; }
        public List<NoopClass0> Noop { get; set; }
        public OwnerClass Owner { get; set; }
        public DatabaseClass Database { get; set; }
        public ServersClass Servers { get; set; }
    }

    public class NoopClass0
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
    }

    public class OwnerClass : ITomlClassComment
    {
        public string ClassComment { get; set; } = string.Empty;
        public Dictionary<string, string> ValueComments { get; set; } = new();

        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset Dob { get; set; }
    }

    public class DatabaseClass : ITomlClassComment
    {
        public string ClassComment { get; set; } = string.Empty;
        public Dictionary<string, string> ValueComments { get; set; } = new();

        public bool Enabled { get; set; }
        public List<List<double>> Points { get; set; }
        public List<int> Ports { get; set; }
        public List<List<TomlNode>> Data { get; set; }
        public TempTargetsClass TempTargets { get; set; }
    }

    public class TempTargetsClass : ITomlClassComment
    {
        public string ClassComment { get; set; } = string.Empty;
        public Dictionary<string, string> ValueComments { get; set; } = new();

        public double Cpu { get; set; }
        public double Apu { get; set; }
    }

    public class ServersClass : ITomlClassComment
    {
        public string ClassComment { get; set; } = string.Empty;
        public Dictionary<string, string> ValueComments { get; set; } = new();

        public AlphaClass Alpha { get; set; }
        public BetaClass Beta { get; set; }
    }

    public class AlphaClass : ITomlClassComment
    {
        public string ClassComment { get; set; } = string.Empty;
        public Dictionary<string, string> ValueComments { get; set; } = new();

        public string Ip { get; set; }
        public string Role { get; set; }
    }

    public class BetaClass : ITomlClassComment
    {
        public string ClassComment { get; set; } = string.Empty;
        public Dictionary<string, string> ValueComments { get; set; } = new();

        public string Ip { get; set; }
        public string Role { get; set; }
    }
}


