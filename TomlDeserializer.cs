using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HKW.Libs.TOML;

public class TomlDeserializer
{
    
    public static T DeserializeFromFile<T>(string tomlFile)
        where T : class, new()
    {
        return Deserialize<T>(TOML.Parse(tomlFile));
    }

    public static T Deserialize<T>(TomlTable table)
        where T : class, new()
    {
        var t = new T();
        DeserializeTable(t, t.GetType(), table);
        return t;
    }

    private static void DeserializeTable(object target, Type type, TomlTable table)
    {
        var iTomlClass = target as ITomlClass;
        if (iTomlClass is not null)
        {
            iTomlClass.TableComment = table.Comment ?? string.Empty;
            iTomlClass.ValueComments ??= new();
        }

        foreach (var kv in table)
        {
            var name = ToPascal(kv.Key);
            var node = kv.Value;
            if (type.GetProperty(name) is not PropertyInfo propertyInfo)
                continue;
            if (Attribute.IsDefined(propertyInfo, typeof(TomlIgnore)))
                continue;
            if (iTomlClass is not null)
            {
                iTomlClass.ValueComments ??= new();
                iTomlClass.ValueComments.Add(name, node.Comment ?? string.Empty);
            }
            DeserializeTableValue(target, node, propertyInfo);
        }
    }

    private static void DeserializeTableValue(
        object target,
        TomlNode node,
        PropertyInfo propertyInfo
    )
    {
        var propertyType = propertyInfo.PropertyType;
        if (node.IsTomlTable)
        {
            if (
                propertyType.Assembly.CreateInstance(propertyType.FullName!)
                is not object nestedTarget
            )
                return;
            propertyInfo.SetValue(target, nestedTarget);
            DeserializeTable(nestedTarget, propertyType, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            if (
                propertyType.Assembly.CreateInstance(propertyType.FullName!)
                is not IList nestedTarget
            )
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
            if (
                elementType.Assembly.CreateInstance(elementType.FullName!)
                is not object nestedTarget
            )
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
            if (
                elementType.Assembly.CreateInstance(elementType.FullName!) is not IList nestedTarget
            )
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
            TomlBoolean => node.AsBoolean,
            TomlString => node.AsString,
            TomlFloat => node.AsDouble,
            TomlInteger => node.AsInt32,
            TomlDateTimeOffset => node.AsDateTimeOffset,
            TomlDateTimeLocal => node.AsDateTime,
            TomlDateTime => node.AsDateTime,
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

[AttributeUsage(AttributeTargets.Property)]
public class TomlIgnore : Attribute { }
