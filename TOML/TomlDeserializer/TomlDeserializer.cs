using System.Collections;
using System.Reflection;
using HKW.TOML.TomlAttribute;
using HKW.TOML.TomlInterface;
using HKW.TOML.TomlException;

namespace HKW.TOML.TomlDeserializer;

/// <summary>
/// Toml反序列化
/// </summary>
public class TomlDeserializer
{
    /// <summary>
    /// 设置
    /// </summary>
    private static TomlDeserializerOptions s_options = null!;

    /// <summary>
    /// 缺失的属性
    /// </summary>
    private static readonly HashSet<string> sr_missingProperties = new();

    /// <summary>
    /// 缺失的Toml节点
    /// </summary>
    private static readonly HashSet<string> sr_missingTomlNodes = new();

    /// <summary>
    /// 缺失的必要属性
    /// </summary>
    private static readonly HashSet<string> sr_missingPequiredProperties = new();

    /// <summary>
    /// 从Toml文件反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static T DeserializeFromFile<T>(string tomlFile, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        return Deserialize<T>(TOML.ParseFromFile(tomlFile), options);
    }

    /// <summary>
    /// 从Toml文件异步反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeFromFileAsync<T>(
        string tomlFile,
        TomlDeserializerOptions? options = null
    )
        where T : class, new()
    {
        return await DeserializeAsync<T>(TOML.ParseFromFile(tomlFile), options);
    }

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(TomlTable table, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        var t = new T();
        s_options = options ?? new();
        PreviewDeserializeTable(t, t.GetType(), table);
        s_options = null!;
        return t;
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeAsync<T>(
        TomlTable table,
        TomlDeserializerOptions? options = null
    )
        where T : class, new()
    {
        var t = new T();
        s_options = options ?? new();
        await Task.Run(() =>
        {
            PreviewDeserializeTable(t, t.GetType(), table);
        });
        s_options = null!;
        return t;
    }

    /// <summary>
    /// 预处理
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    /// <exception cref="ConsistencyException">一致性异常</exception>
    private static void PreviewDeserializeTable(object target, Type type, TomlTable table)
    {
        DeserializeTable(target, type, table, string.Empty);

        if (sr_missingProperties.Any() || sr_missingTomlNodes.Any())
        {
            throw new ConsistencyException(
                "Deserialize error: consistency exception",
                sr_missingProperties.OrderBy(s => s),
                sr_missingTomlNodes.OrderBy(s => s)
            );
        }
        else if (sr_missingPequiredProperties.Any())
        {
            throw new MissingRequiredException(
                "Deserialize error: missing required properties exception",
                sr_missingPequiredProperties.OrderBy(s => s)
            );
        }

        sr_missingProperties.Clear();
        sr_missingTomlNodes.Clear();
        sr_missingPequiredProperties.Clear();
    }

    /// <summary>
    /// 反序列化Toml表格并检查一致性
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    /// <param name="tableName">Toml表格名称</param>
    private static void DeserializeTable(
        object target,
        Type type,
        TomlTable table,
        string tableName
    )
    {
        // 设置注释
        var iTomlClass = target as ITomlClassComment;
        if (iTomlClass is not null)
        {
            iTomlClass.ClassComment = table.Comment ?? string.Empty;
            iTomlClass.ValueComments ??= new();
        }
        // 设置一致性信息
        TryCheckConsistency(type, table, out var missingProperties, out var missingTomlNodes);

        foreach (var kv in table)
        {
            var name = ToPascal(kv.Key);
            var node = kv.Value;
            if (type.GetProperty(name) is not PropertyInfo propertyInfo)
                continue;
            // 检测是否包含隐藏特性
            if (Attribute.IsDefined(propertyInfo, typeof(TomlIgnoreAttribute)))
                continue;
            // 删除存在的内容
            CheckConsistency(propertyInfo.Name, kv.Key, missingProperties, missingTomlNodes);
            // 设置注释
            iTomlClass?.ValueComments.TryAdd(name, node.Comment ?? string.Empty);
            DeserializeTableValue(target, kv.Key, node, propertyInfo);
        }
        var missingPequiredProperties = new HashSet<string>();
        // 检查TomlName特性
        CheckProperties(
            iTomlClass,
            target,
            type,
            table,
            missingProperties,
            missingTomlNodes,
            missingPequiredProperties
        );
        // 添加一致性信息
        AddMissingValue(type.FullName!, tableName, missingProperties, missingTomlNodes);
        // 添加缺失的必要属性信息
        AddMissingRequiredValue(type.FullName!, missingPequiredProperties);
    }

    private static void TryCheckConsistency(
        Type type,
        TomlTable table,
        out HashSet<string>? missingProperties,
        out HashSet<string>? missingTomlNodes
    )
    {
        missingProperties = null;
        missingTomlNodes = null;
        if (s_options.CheckConsistency is false)
            return;
        missingProperties = type.GetProperties().Select(i => i.Name).ToHashSet();
        missingProperties.Remove(nameof(ITomlClassComment.ClassComment));
        missingProperties.Remove(nameof(ITomlClassComment.ValueComments));
        missingTomlNodes = table.Keys.ToHashSet();
    }

    private static void CheckConsistency(
        string propertyName,
        string keyName,
        HashSet<string>? missingProperties,
        HashSet<string>? missingTomlNodes
    )
    {
        if (missingProperties is null || missingTomlNodes is null)
            return;
        missingProperties.Remove(propertyName);
        missingTomlNodes.Remove(keyName);
    }

    /// <summary>
    /// 添加一致性信息
    /// </summary>
    /// <param name="className">类名</param>
    /// <param name="tableName">Toml表格名</param>
    /// <param name="missingProperties">缺失数据的属性</param>
    /// <param name="missingTomlNodes">缺失数据的Toml节点</param>
    private static void AddMissingValue(
        string className,
        string tableName,
        HashSet<string>? missingProperties,
        HashSet<string>? missingTomlNodes
    )
    {
        if (
            string.IsNullOrWhiteSpace(tableName)
            || missingProperties is null
            || missingTomlNodes is null
        )
            return;
        foreach (var propertyName in missingProperties)
            sr_missingProperties.Add($"{className}.{propertyName}");
        foreach (var nodeName in missingTomlNodes)
            sr_missingTomlNodes.Add($"{tableName}.{nodeName}");
    }

    /// <summary>
    /// 添加缺失的必要属性信息
    /// </summary>
    /// <param name="className">类名</param>
    /// <param name="missingPequiredProperties">缺失的必要属性</param>
    private static void AddMissingRequiredValue(
        string className,
        HashSet<string> missingPequiredProperties
    )
    {
        foreach (var propertyName in missingPequiredProperties)
            sr_missingPequiredProperties.Add($"{className}.{propertyName}");
    }

    /// <summary>
    /// 检查TomlName特性并检查一致性
    /// </summary>
    /// <param name="iTomlClass">Toml类接口</param>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    /// <param name="missingProperties">缺失数据的属性</param>
    /// <param name="missingTomlNodes">缺失数据的Toml节点</param>
    /// <param name="missingPequiredProperties">缺失数据的必要属性</param>
    private static void CheckProperties(
        ITomlClassComment? iTomlClass,
        object target,
        Type type,
        TomlTable table,
        HashSet<string>? missingProperties,
        HashSet<string>? missingTomlNodes,
        HashSet<string> missingPequiredProperties
    )
    {
        foreach (var propertyInfo in type.GetProperties())
        {
            // 检测是否为必要属性
            if (Attribute.IsDefined(propertyInfo, typeof(TomlRequiredAttribute)))
                missingPequiredProperties.Add(propertyInfo.Name);
            // 获取TomlKeyName
            if (
                propertyInfo.GetCustomAttribute<TomlPropertyNameAttribute>()
                is not TomlPropertyNameAttribute keyName
            )
                continue;
            if (string.IsNullOrWhiteSpace(keyName.Value))
                continue;
            if (table.TryGetValue(keyName.Value, out var node) is false)
                continue;
            // 删除存在的内容
            CheckConsistency(propertyInfo.Name, keyName.Value, missingProperties, missingTomlNodes);
            // 设置值
            iTomlClass?.ValueComments.TryAdd(propertyInfo.Name, node.Comment ?? string.Empty);
            DeserializeTableValue(target, keyName.Value, node, propertyInfo);
            missingPequiredProperties.Remove(propertyInfo.Name);
        }
    }

    /// <summary>
    /// 反序列化Toml表格的值并检查一致性
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="nodeKeyName">Toml节点键名</param>
    /// <param name="node">值</param>
    /// <param name="propertyInfo">属性信息</param>
    private static void DeserializeTableValue(
        object target,
        string nodeKeyName,
        TomlNode node,
        PropertyInfo propertyInfo
    )
    {
        var propertyType = propertyInfo.PropertyType;

        // 检测TomlConverter
        if (
            propertyInfo.GetCustomAttribute(typeof(TomlConverterAttribute))
            is TomlConverterAttribute tomlConverter
        )
        {
            propertyInfo.SetValue(target, tomlConverter.Read(node));
            return;
        }

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
            DeserializeTable(nestedTarget, propertyType, node.AsTomlTable, nodeKeyName);
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
            DeserializeTable(nestedTarget, nestedTarget.GetType(), node.AsTomlTable, string.Empty);
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
            TypeCode.Single
                => Convert.ChangeType(node.AsDouble, TypeCode.Single),
            TypeCode.Double => Convert.ChangeType(node.AsDouble, TypeCode.Double),

            // 整型
            TypeCode.SByte
                => Convert.ChangeType(node.AsDouble, TypeCode.SByte),
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
