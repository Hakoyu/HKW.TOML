// See https://aka.ms/new-console-template for more information
using System.Text;
using HKW.Libs.TOML;

var file = "C:\\Users\\HKW\\Desktop\\Dotnet\\test.toml";
//var classString = TomlAsClasses.Construct("Test", TOML.Parse(file));
var table = TOML.Parse(file);
var array = table.ElementAt(0).Value.AsTomlArray;
int i = array[1];
//var test = TomlSerializer.FromTomlFile<Test>(file);
//Console.WriteLine(classString);
Console.WriteLine();
class Test
{
    public string title { get; set; }
    public string str2 { get; set; }
    public string str3 { get; set; }
    public Owner owner { get; set; }

    public class Owner
    {
        public string name { get; set; }
        public string organization { get; set; }
        public string bio { get; set; }
        public DateTimeOffset dob { get; set; }
    }
}