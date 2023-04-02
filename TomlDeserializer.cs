using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HKW.Libs.TOML;

/// <summary>
/// Toml反序列化
/// </summary>
public class TomlDeserializer
{
    private static TomlDeserializerOptions s_options = null!;

    /// <summary>
    /// 从Toml文件反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <returns>完成反序列化的对象</returns>
    public static T DeserializeFromFile<T>(string tomlFile, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        return Deserialize<T>(TOML.Parse(tomlFile), options);
    }

    /// <summary>
    /// 从Toml文件异步反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeFromFileAsync<T>(string tomlFile, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        return await DeserializeAsync<T>(TOML.Parse(tomlFile), options);
    }

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="table">Toml表格</param>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(TomlTable table, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        var t = new T();
        s_options = options ?? new();
        DeserializeTable(t, t.GetType(), table);
        s_options = null!;
        return t;
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="table">Toml表格</param>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeAsync<T>(TomlTable table, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        var t = new T();
        s_options = options ?? new();
        await Task.Run(() =>
        {
            DeserializeTable(t, t.GetType(), table);
        });
        s_options = null!;
        return t;
    }

    /// <summary>
    /// 反序列化Toml表格
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    private static void DeserializeTable(object target, Type type, TomlTable table)
    {
        // 设置注释
        var iTomlClass = target as ITomlClassComment;
        if (iTomlClass is not null)
        {
            iTomlClass.ClassComment = table.Comment ?? string.Empty;
            iTomlClass.ValueComments ??= new();
        }

        foreach (var kv in table)
        {
            var name = ToPascal(kv.Key);
            var node = kv.Value;
            if (type.GetProperty(name) is not PropertyInfo propertyInfo)
                continue;
            // 检测是否包含隐藏特性
            if (Attribute.IsDefined(propertyInfo, typeof(TomlIgnore)))
                continue;
            // 设置注释
            iTomlClass?.ValueComments.Add(name, node.Comment ?? string.Empty);
            DeserializeTableValue(target, node, propertyInfo);
        }

        // 检查TomlName特性
        CheckTomlName(target, type, table);
    }

    /// <summary>
    /// 检查TomlName特性
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    private static void CheckTomlName(object target, Type type, TomlTable table)
    {
        foreach (var propertyInfo in type.GetProperties())
        {
            if (propertyInfo.GetCustomAttribute<TomlKeyName>() is not TomlKeyName tomlName)
                continue;
            if (string.IsNullOrWhiteSpace(tomlName.Name))
                continue;
            DeserializeTableValue(target, table[tomlName.Name], propertyInfo);
        }
    }

    /// <summary>
    /// 反序列化Toml表格的值
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="node">值</param>
    /// <param name="propertyInfo">属性信息</param>
    private static void DeserializeTableValue(
        object target,
        TomlNode node,
        PropertyInfo propertyInfo
    )
    {
        var propertyType = propertyInfo.PropertyType;
        if (node.IsTomlTable)
        {
            // 如果值是Toml表格,则创建一个新的对象
            if (
                propertyType.Assembly.CreateInstance(propertyType.FullName!)
                is not object nestedTarget
            )
                return;
            propertyInfo.SetValue(target, nestedTarget);
            // 递归Toml表格
            DeserializeTable(nestedTarget, propertyType, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            // 如果是Toml数组,则检测是否为IList类型
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
            // 获取并设定属性值
            var value = GetNodeVale(node, Type.GetTypeCode(propertyType));
            propertyInfo.SetValue(target, value);
        }
    }

    /// <summary>
    /// 反序列化Toml数组
    /// </summary>
    /// <param name="type">属性类型</param>
    /// <param name="list">列表</param>
    /// <param name="array">数组</param>
    private static void DeserializeArray(Type type, IList list, TomlArray array)
    {
        if (array.Any() is false)
            return;
        // 获取列表值的类型
        if (type.GetGenericArguments()[0] is not Type elementType)
            return;
        // 如果值是Toml节点,则直接添加
        if (elementType == typeof(TomlNode))
        {
            foreach (var node in array)
                list.Add(node);
            return;
        }
        // 获取类型代码
        var typeCode = Type.GetTypeCode(elementType);
        foreach (var node in array)
            DeserializeArrayValue(list, node, elementType, typeCode);
    }

    /// <summary>
    /// 反序列化Toml数组的值
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="node">值</param>
    /// <param name="elementType">列表值类型</param>
    /// <param name="typeCode">类型代码</param>
    private static void DeserializeArrayValue(
        IList list,
        TomlNode node,
        Type elementType,
        TypeCode typeCode
    )
    {
        if (node.IsTomlTable)
        {
            // 如果值是Toml表格,则创建一个新的对象
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
            // 如果是Toml数组,则检测是否为IList类型
            if (
                elementType.Assembly.CreateInstance(elementType.FullName!) is not IList nestedTarget
            )
                return;
            list.Add(nestedTarget);
            DeserializeArray(elementType, nestedTarget, node.AsTomlArray);
        }
        else
        {
            list.Add(GetNodeVale(node, typeCode));
        }
    }

    /// <summary>
    /// 获取Toml节点的值
    /// </summary>
    /// <param name="node">节点</param>
    /// <param name="typeCode">类型代码</param>
    /// <returns>csharp原生值</returns>
    private static object GetNodeVale(TomlNode node, TypeCode typeCode)
    {
        return typeCode switch
        {
            TypeCode.Boolean => node.AsBoolean,
            TypeCode.String => node.AsString,

            // 浮点型
            TypeCode.Single => Convert.ChangeType(node.AsDouble, TypeCode.Single),
            TypeCode.Double => Convert.ChangeType(node.AsDouble, TypeCode.Double),

            // 整形
            TypeCode.SByte => Convert.ChangeType(node.AsDouble, TypeCode.SByte),
            TypeCode.Byte => Convert.ChangeType(node.AsDouble, TypeCode.Byte),
            TypeCode.Int16 => Convert.ChangeType(node.AsDouble, TypeCode.Int16),
            TypeCode.UInt16 => Convert.ChangeType(node.AsDouble, TypeCode.UInt16),
            TypeCode.Int32 => Convert.ChangeType(node.AsDouble, TypeCode.Int32),
            TypeCode.UInt32 => Convert.ChangeType(node.AsDouble, TypeCode.UInt32),
            TypeCode.Int64 => Convert.ChangeType(node.AsDouble, TypeCode.Int64),
            TypeCode.UInt64 => Convert.ChangeType(node.AsDouble, TypeCode.UInt64),

            TypeCode.DateTime => node.AsDateTime,
            TypeCode.Object when node.IsTomlDateTimeOffset => node.AsDateTimeOffset,

            _ => node,
        };
    }

    /// <summary>
    /// 将字符串转换为帕斯卡格式
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>帕斯卡格式字符串</returns>
    private static string ToPascal(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;
        // 使用分隔符拆分单词
        var strs = str.Split(s_options.KeyWordSeparator);
        // 将单词首字母大写
        var newStrs = strs.Select(s => FirstLetterToUpper(s));
        // 是否保留分隔符
        if (s_options.RemoveKeyWordSeparator)
            return string.Join("", newStrs);
        else
            return string.Join(s_options.KeyWordSeparator, newStrs);
    }

    /// <summary>
    /// 将字符串首字母大写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>第一个为大写的字符串</returns>
    private static string FirstLetterToUpper(string str) => $"{char.ToUpper(str[0])}{str[1..]}";
}

/// <summary>
/// Toml反序列化设置
/// </summary>
public class TomlDeserializerOptions
{
    /// <summary>
    /// 删除键的单词分隔符 如 "_"
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool RemoveKeyWordSeparator { get; set; } = true;

    /// <summary>
    /// 单词分隔符
    /// <para>默认为 "<see langword="_"/>"</para>
    /// </summary>
    public string KeyWordSeparator { get; set; } = "_";
}

/// <summary>
/// Toml忽略值
/// <para>在序列化和反序列化时忽略的值</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TomlIgnore : Attribute { }

/// <summary>
/// Toml名称
/// <para>指定Toml键的名称</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TomlKeyName : Attribute
{
    /// <summary>
    /// 键名
    /// </summary>
    public string Name { get; }

    public TomlKeyName(string name) => Name = name;
}
