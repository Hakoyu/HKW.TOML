// See https://aka.ms/new-console-template for more information
using HKW.Libs.TOML;

var file = "C:\\Users\\HKW\\Desktop\\Dotnet\\test.toml";
//var classString = TomlAsClasses.Construct("Test", TOML.Parse(file));
//var table = TOML.Parse(file);
var test = TomlSerializer.DeserializeFromFile<Test>(file);
Console.WriteLine(test);
Console.WriteLine();


public class Test
{
    public string Title { get; set; }
    public OwnerClass Owner { get; set; }
    public DatabaseClass Database { get; set; }
    public ServersClass Servers { get; set; }
}
public class OwnerClass
{
    public string Name { get; set; }
    public DateTimeOffset Dob { get; set; }
}
public class DatabaseClass
{
    public bool Enabled { get; set; }
    public List<int> Ports { get; set; }
    public List<TomlNode> Data { get; set; }
    public TempTargetsClass TempTargets { get; set; }
}
public class TempTargetsClass
{
    public double Cpu { get; set; }
    public double Case { get; set; }
}
public class ServersClass
{
    public AlphaClass Alpha { get; set; }
    public BetaClass Beta { get; set; }
}
public class AlphaClass
{
    public string Ip { get; set; }
    public string Role { get; set; }
}
public class BetaClass
{
    public string Ip { get; set; }
    public string Role { get; set; }
}