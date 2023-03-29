using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace HKW.Libs.TOML;

public class TomlSerializer
{
    public static T DeserializeFromFile<T>(string tomlFile)
        where T : class, new()
    {
        var t = new T();
        var toml = TOML.Parse(tomlFile);
        DeserializeTable(t, t.GetType(), toml);
        return t;
    }

    private static void DeserializeTable(object target, Type type, TomlTable table)
    {
        foreach (var kv in table)
        {
            var name = ToPascal(kv.Key);
            var node = kv.Value;
            if (node.IsTomlTable)
            {
                if (type.GetProperty(name) is not PropertyInfo propertyInfo)
                    continue;
                DeserializeTable(target, propertyInfo.PropertyType, table);
            }
            else
            {
                if (type.GetProperty(name) is not PropertyInfo propertyInfo)
                    continue;
                var value = GetNodeVale(node);
                propertyInfo.SetValue(target, value);
            }
        }
    }
    private static object GetNodeVale(TomlNode node)
    {
        return node switch
        {
            { IsTomlBoolean: true } => node.AsBoolean,
            { IsTomlString: true } => node.AsString,
            { IsTomlFloat: true } => node.AsFloat,
            { IsTomlInteger: true } => node.AsInteger,
            { IsTomlDateTimeLocal: true } => node.AsDateTimeLocal,
            { IsTomlDateTimeOffset: true } => node.AsDateTimeOffset,
            { IsTomlDateTime: true } => node.AsDateTime,
            _ => throw new ArgumentOutOfRangeException(nameof(node), node, null)
        };
    }
    private static string ToPascal(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;
        var strs = str.Split("_");
        var sb = new StringBuilder();
        foreach (var tempStr in strs)
            sb.Append(FirstLetterToUpper(tempStr));
        return sb.ToString();
    }

    private static string FirstLetterToUpper(string str) => $"{char.ToUpper(str[0])}{str[1..]}";
}
