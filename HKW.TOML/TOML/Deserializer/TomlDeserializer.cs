using System.Collections;
using System.Reflection;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Interfaces;
using HKW.HKWTOML.Exceptions;
using System.Linq;
using HKWTOML.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化
/// </summary>
public class TomlDeserializer
{
    /// <summary>
    /// 设置
    /// </summary>
    private static DeserializerOptions s_options = null!;

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
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static T DeserializeFromFile<T>(string tomlFile, DeserializerOptions? options = null)
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
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeFromFileAsync<T>(
        string tomlFile,
        DeserializerOptions? options = null
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
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(TomlTable table, DeserializerOptions? options = null)
        where T : class, new()
    {
        var t = new T();
        PreviewDeserialize(t, t.GetType(), table, options);
        return t;
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeAsync<T>(
        TomlTable table,
        DeserializerOptions? options = null
    )
        where T : class, new()
    {
        var t = new T();
        await Task.Run(() =>
        {
            PreviewDeserialize(t, t.GetType(), table, options);
        });
        return t;
    }

    /// <summary>
    /// 从Toml文件反序列化至静态类
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="staticClass">静态类类型</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    public static void DeserializeStaticFromFile(
        string tomlFile,
        Type staticClass,
        DeserializerOptions? options = null
    )
    {
        DeserializeStatic(TOML.ParseFromFile(tomlFile), staticClass, options);
    }

    /// <summary>
    /// 从Toml文件异步反序列化至静态类
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="staticClass">静态类类型</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    public static async Task DeserializeStaticFromFileAsync(
        string tomlFile,
        Type staticClass,
        DeserializerOptions? options = null
    )
    {
        await DeserializeStaticeAsync(TOML.ParseFromFile(tomlFile), staticClass, options);
    }

    /// <summary>
    /// 从Toml表格反序列化至静态类
    /// </summary>
    /// <param name="table">Toml表格</param>
    /// <param name="staticClass">静态类类型</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    public static void DeserializeStatic(
        TomlTable table,
        Type staticClass,
        DeserializerOptions? options = null
    )
    {
        PreviewDeserialize(staticClass, staticClass, table, options);
    }

    /// <summary>
    /// 从Toml表格异步反序列化至静态类
    /// </summary>
    /// <param name="table">Toml表格</param>
    /// <param name="staticClass">静态类类型</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    public static async Task DeserializeStaticeAsync(
        TomlTable table,
        Type staticClass,
        DeserializerOptions? options = null
    )
    {
        await Task.Run(() =>
        {
            PreviewDeserialize(staticClass, staticClass, table, options);
        });
    }

    /// <summary>
    /// 预处理
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    private static void PreviewDeserialize(
        object target,
        Type type,
        TomlTable table,
        DeserializerOptions? options
    )
    {
        s_options = options ?? new();
        DeserializeTable(target, type, table);

        if (sr_missingPequiredProperties.Any())
        {
            throw new MissingRequiredException(
                "Deserialize error: missing required properties exception",
                sr_missingPequiredProperties.OrderBy(s => s)
            );
        }

        sr_missingPequiredProperties.Clear();
        s_options = null!;
    }

    /// <summary>
    /// 反序列化Toml表格
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    private static void DeserializeTable(object target, Type type, TomlTable table)
    {
        // 设置忽略大小写
        var originalKeyIgnoreCase = table.KeyIgnoreCase;
        table.KeyIgnoreCase = s_options.PropertyNameCaseInsensitive;
        // 运行反序列化前的方法
        RunMethodOnDeserializingWithClass(target, type);
        GetMethods(type, out var methodOnDeserializing, out var methodOnDeserialized);
        RunMethodOnDeserializing(target, methodOnDeserializing);
        // 设置注释
        var iTomlClass = target as ITomlClassComment;
        if (iTomlClass is not null)
        {
            iTomlClass.ClassComment = table.Comment ?? string.Empty;
            iTomlClass.ValueComments ??= new();
        }

        var missingRequiredProperties = new HashSet<string>();
        foreach (var propertyInfo in type.GetProperties())
        {
            if (DeserializeProperty(target, propertyInfo, table, iTomlClass))
                missingRequiredProperties.Add(propertyInfo.Name);
        }

        // 添加缺失的必要属性信息
        AddMissingRequiredValue(type.FullName!, missingRequiredProperties);
        // 运行反序列化后的方法
        RunMethodOnDeserializedWithClass(target, type);
        RunMethodOnDeserialized(target, methodOnDeserialized);
        // 恢复忽略大小写
        table.KeyIgnoreCase = originalKeyIgnoreCase;
    }

    private static bool DeserializeProperty(
        object target,
        PropertyInfo propertyInfo,
        TomlTable table,
        ITomlClassComment? iTomlClassComment
    )
    {
        // 检测是否为隐藏属性
        if (Attribute.IsDefined(propertyInfo, typeof(TomlIgnoreAttribute)))
            return false;
        // 跳过ITomlClassComment生成的接口
        if (iTomlClassComment is not null)
        {
            if (propertyInfo.Name == nameof(ITomlClassComment.ClassComment))
                return false;
            else if (propertyInfo.Name == nameof(ITomlClassComment.ValueComments))
                return false;
        }
        var isRequired = false;
        // 检测是否为必要属性
        if (Attribute.IsDefined(propertyInfo, typeof(TomlRequiredAttribute)))
            isRequired = true;
        var name = GetPropertyName(propertyInfo);
        if (table.TryGetValue(name, out var node) is false)
            return isRequired;
        // 设置注释
        iTomlClassComment?.ValueComments.TryAdd(name, node.Comment ?? string.Empty);
        DeserializeTableValue(target, node, propertyInfo);
        return false;
    }

    private static string GetPropertyName(PropertyInfo propertyInfo)
    {
        // 获取TomlKeyName
        if (
            propertyInfo.GetCustomAttribute<TomlPropertyNameAttribute>()
            is TomlPropertyNameAttribute keyName
        )
            return keyName.Value;
        else
            return propertyInfo.Name;
    }

    #region RunMethod
    /// <summary>
    /// 获取方法
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="methodOnDeserializing">运行于反序列化之前的方法</param>
    /// <param name="methodOnDeserialized">运行于反序列化之后的方法</param>
    private static void GetMethods(
        Type type,
        out IEnumerable<MethodAndParameters> methodOnDeserializing,
        out IEnumerable<MethodAndParameters> methodOnDeserialized
    )
    {
        List<MethodAndParameters> tempMethodOnDeserializing = new();
        List<MethodAndParameters> tempMethodOnDeserialized = new();
        foreach (var method in TOMLUtils.GetRuntimeMethodsNotContainProperty(type))
        {
            if (
                method.GetCustomAttribute(typeof(RunOnTomlDeserializingAttribute))
                is RunOnTomlDeserializingAttribute runOnTomlDeserializing
            )
            {
                tempMethodOnDeserializing.Add(new(method, runOnTomlDeserializing.Parameters));
            }
            else if (
                method.GetCustomAttribute(typeof(RunOnTomlDeserializedAttribute))
                is RunOnTomlDeserializedAttribute runOnTomlDeserialized
            )
            {
                tempMethodOnDeserialized.Add(new(method, runOnTomlDeserialized.Parameters));
            }
        }
        methodOnDeserializing = tempMethodOnDeserializing;
        methodOnDeserialized = tempMethodOnDeserialized;
    }

    /// <summary>
    /// 运行反序列化之前的方法
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="methodOnDeserializing">运行于反序列化之前的方法</param>
    private static void RunMethodOnDeserializing(
        object target,
        IEnumerable<MethodAndParameters> methodOnDeserializing
    )
    {
        foreach (var mp in methodOnDeserializing)
        {
            mp.Method.Invoke(target, mp.Parameters);
        }
    }

    /// <summary>
    /// 运行反序列化之后的方法
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="methodOnDeserialized">运行于反序列化之后的方法</param>
    private static void RunMethodOnDeserialized(
        object target,
        IEnumerable<MethodAndParameters> methodOnDeserialized
    )
    {
        foreach (var mp in methodOnDeserialized)
        {
            mp.Method.Invoke(target, mp.Parameters);
        }
    }

    /// <summary>
    /// 运行反序列化时,类附加的方法
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">类型</param>
    private static void RunMethodOnDeserializingWithClass(object target, Type type)
    {
        if (
            type.GetCustomAttribute(typeof(RunOnTomlDeserializingAttribute))
            is not RunOnTomlDeserializingAttribute runOnTomlDeserializing
        )
            return;
        runOnTomlDeserializing.Method?.Invoke(target, runOnTomlDeserializing.Parameters);
    }

    /// <summary>
    /// 运行反序列化后,类附加的方法
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">类型</param>
    private static void RunMethodOnDeserializedWithClass(object target, Type type)
    {
        if (
            type.GetCustomAttribute(typeof(RunOnTomlDeserializedAttribute))
            is not RunOnTomlDeserializedAttribute runOnTomlDeserialized
        )
            return;
        runOnTomlDeserialized.Method?.Invoke(target, runOnTomlDeserialized.Parameters);
    }

    #endregion
    #region CheckConsistency
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
    #endregion
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
            TypeCode.Single
                => Convert.ChangeType(node.AsDouble, TypeCode.Single),
            TypeCode.Double => Convert.ChangeType(node.AsDouble, TypeCode.Double),

            // 整型
            TypeCode.SByte
                => Convert.ChangeType(node.AsInt64, TypeCode.SByte),
            TypeCode.Byte => Convert.ChangeType(node.AsInt64, TypeCode.Byte),
            TypeCode.Int16 => Convert.ChangeType(node.AsInt64, TypeCode.Int16),
            TypeCode.UInt16 => Convert.ChangeType(node.AsInt64, TypeCode.UInt16),
            TypeCode.Int32 => Convert.ChangeType(node.AsInt64, TypeCode.Int32),
            TypeCode.UInt32 => Convert.ChangeType(node.AsInt64, TypeCode.UInt32),
            TypeCode.Int64 => Convert.ChangeType(node.AsInt64, TypeCode.Int64),
            TypeCode.UInt64 => Convert.ChangeType(node.AsInt64, TypeCode.UInt64),

            TypeCode.DateTime => node.AsDateTime,
            TypeCode.Object when node.IsTomlDateTimeOffset => node.AsDateTimeOffset,

            _ => node,
        };
    }
}
