using System;
using System.Collections;
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
            if (type.GetProperty(name) is not PropertyInfo propertyInfo)
                continue;
            DeserializeTableValue(target, node, propertyInfo);
        }
    }
    private static void DeserializeTableValue(object target, TomlNode node, PropertyInfo propertyInfo)
    {
        var propertyType = propertyInfo.PropertyType;
        if (node.IsTomlTable)
        {
            if (propertyType.Assembly.CreateInstance(propertyType.FullName!) is not object nestedTarget)
                return;
            propertyInfo.SetValue(target, nestedTarget);
            DeserializeTable(nestedTarget, propertyType, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            if (propertyType.Assembly.CreateInstance(propertyType.FullName!) is not IList nestedTarget)
                return;
            propertyInfo.SetValue(target, nestedTarget);
            DeserializeArray(propertyType, nestedTarget, node.AsTomlArray);
        }
        else
        {
            var value = GetNodeVale(node);
            propertyInfo.SetValue(target, value);
        }
    }
    private static void DeserializeArray(Type type, IList list, TomlArray array)
    {
        foreach (var node in array)
        {
            DeserializeArrayValue(type, list, node);
        }
    }
    private static void DeserializeArrayValue(Type type, IList list, TomlNode node)
    {
        if (node.IsTomlTable)
        {
            if (type.GetGenericArguments()[0] is not Type elementType)
                return;
            if (elementType.Assembly.CreateInstance(elementType.FullName!) is not object nestedTarget)
                return;
            list.Add(nestedTarget);
            DeserializeTable(nestedTarget, nestedTarget.GetType(), node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            if (type.GetGenericArguments()[0] is not Type elementType)
                return;
            if (elementType == typeof(TomlNode))
            {
                list.Add(GetNodeVale(node));
                return;
            }
            if (elementType.Assembly.CreateInstance(elementType.FullName!) is not IList nestedTarget)
                return;
            list.Add(nestedTarget);
            DeserializeArray(elementType, nestedTarget, node.AsTomlArray);
        }
        else
        {
            list.Add(GetNodeVale(node));
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
            _ => node
        };
    }
    private static string ToPascal(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;
        var strs = str.Split("_");
        var newStrs = strs.Select(s => FirstLetterToUpper(s));
        return string.Join("", newStrs);
    }

    private static string FirstLetterToUpper(string str) => $"{char.ToUpper(str[0])}{str[1..]}";
}
