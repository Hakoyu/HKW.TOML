// See https://aka.ms/new-console-template for more information
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

namespace HKWToml;
internal class HKWToml
{
    public static void Main(string[] args)
    {
#if DEBUG
        var table = TOML.Parse(TestExample.TomlExample);
        var classString = TomlAsClasses.Construct(table, "Test", new()
        {
            AddITomlClassCommentInterface = true,
            AddTomlPropertyOrderAttribute = true,
            AddTomlRequiredAttribute = true,
            KeyWordSeparators = new() { '-' }
        });
        Console.WriteLine(classString);

        //Console.WriteLine(table.ToTomlString());
        //try
        //{
        //    //var test = TomlDeserializer.DeserializeFromFile<Test>(file, new() { CheckConsistency = true });
        //    var test = TomlDeserializer.DeserializeFromFile<Test>(file);
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.ToString());
        //}
        //TomlSerializer.SerializeToFile(test, outFile);
        //var test1 = TomlDeserializer.Deserialize<Test1>(table["database"]["temp_targets"].AsTomlTable);
        //Console.WriteLine(TomlSerializer.Serialize(test).ToTomlString());
#endif
    }

#if DEBUG
    public class BBBConverter : ITomlConverter<int>
    {
        public int Read(TomlNode node)
        {
            return 114514;
        }

        public TomlNode Write(int item)
        {
            return new TomlString() { Value = item.ToString() };
        }
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

        //[TomlSortOrder(0)]
        //[TomlPropertyName("title")]
        //public string AAA { get; set; }
        //[TomlPropertyName("title")]
        //[TomlConverter(typeof(BBBConverter))]
        [TomlRequired]
        public int BBB { get; set; }
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
#endif
}

