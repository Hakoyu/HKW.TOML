using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.Libs.TOML;

public class TomlAsClasses
{
    private static readonly Dictionary<string, TomlClass> _tomlClasses = new();

    public static string Construct(string className, TomlTable table)
    {
        ConstructTomlTable(className, string.Empty, table);
        var sb = new StringBuilder();
        foreach (var tomlClass in _tomlClasses.Values)
            sb.AppendLine(tomlClass.ToString());
        return sb.ToString();
    }

    static void ConstructTomlTable(string className, string parentClassName, TomlTable table)
    {
        var tomlClass = new TomlClass(className, parentClassName);
        _tomlClasses.Add(className, tomlClass);
        foreach (var kv in table)
        {
            var name = kv.Key;
            var node = kv.Value;
            if (node.IsTomlTable)
            {
                var nestedClassName = $"{ToPascal(name)}Class";
                if (string.IsNullOrWhiteSpace(parentClassName))
                    tomlClass.Add(name, new(name, nestedClassName));
                else
                    _tomlClasses[className].Add(name, new(name, nestedClassName));
                ConstructTomlTable(nestedClassName, className, node.AsTomlTable);
            }
            else if (node.IsTomlArray)
            {
                var arrayTypeName = ConstructTomlArray(node.AsTomlArray);
                tomlClass.Add(name, new(name, arrayTypeName));
            }
            else
            {
                tomlClass.Add(name, new(name, node));
            }
        }
    }

    static string ConstructTomlArray(TomlArray array)
    {
        return $"    List<{ParseTomlArrayValueType(array)}>";
    }

    static string ParseTomlArrayValueType(TomlArray array)
    {
        var typeCount = new (bool IsType, int Index)[9];
        var index = 0;
        foreach (var node in array)
        {
            var temp = typeCount[node switch
            {
                { IsTomlBoolean: true } => 0,
                { IsTomlString: true } => 1,
                { IsTomlFloat: true } => 2,
                { IsTomlInteger: true } => 3,
                { IsTomlDateTimeLocal: true } => 4,
                { IsTomlDateTimeOffset: true } => 5,
                { IsTomlDateTime: true } => 6,
                { IsTomlArray: true } => 7,
                { IsTomlTable: true } => 8,
                _ => throw new ArgumentOutOfRangeException(nameof(array), node, null)
            }];
            temp.IsType = true;
            temp.Index = index++;
        }
        var typeName = string.Empty;
        if (typeCount[2].IsType && typeCount[3].IsType && typeCount.Count(i => i.IsType) < 7)
            typeName = "double";
        else if (typeCount[7].IsType)
            ConstructTomlArray(array[typeCount[7].Index].AsTomlArray);
        //else if (typeCount[8].IsType)
        else
            typeName = typeCount.Count(i => i.IsType) < 8 ? nameof(TomlNode) : array[0].TypeName();
        return typeName;
    }

    static string ToPascal(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;
        var strs = str.Split("_");
        var sb = new StringBuilder();
        foreach (var tempStr in strs)
            sb.Append(FirstLetterToUpper(tempStr));
        return sb.ToString();
    }
    static string FirstLetterToUpper(string str) => $"{char.ToUpper(str[0])}{str[1..]}";
}
