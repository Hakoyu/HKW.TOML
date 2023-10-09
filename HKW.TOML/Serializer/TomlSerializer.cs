using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Interfaces;
using HKWTOML.Utils;

namespace HKW.HKWTOML.Serializer;

/// <summary>
/// Toml序列化
/// </summary>
public class TOMLSerializer
{
    /// <summary>
    /// Toml序列化设置
    /// </summary>
    private readonly TOMLSerializerOptions _options;

    /// <inheritdoc/>
    /// <param name="options">设置</param>
    private TOMLSerializer(TOMLSerializerOptions? options = null)
    {
        _options = options ?? new();
    }

    #region Serialize

    /// <summary>
    /// 序列化至Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static TomlTable Serialize(object source, TOMLSerializerOptions? options = null)
    {
        var serializer = new TOMLSerializer(options);
        return serializer.Serialize(source);
    }

    /// <summary>
    /// 异步序列化至Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static async Task<TomlTable> SerializeAsync(
        object source,
        TOMLSerializerOptions? options = null
    )
    {
        return await Task.Run(() => Serialize(source, options));
    }

    /// <summary>
    /// 序列化静态类至Toml表格
    /// </summary>
    /// <param name="sourceType">静态类类型</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static TomlTable SerializeStatic(Type sourceType, TOMLSerializerOptions? options = null)
    {
        return Serialize(sourceType, options);
    }

    /// <summary>
    /// 异步序列化静态类至Toml表格
    /// </summary>
    /// <param name="sourceType">静态类类型</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static async Task<TomlTable> SerializeStaticAsync(
        Type sourceType,
        TOMLSerializerOptions? options = null
    )
    {
        return await SerializeAsync(sourceType, options);
    }
    #endregion

    #region Create TOML

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="source">源头</param>
    /// <returns>Toml表格数据</returns>
    private TomlTable Serialize(object source)
    {
        return CreateTomlTable(source);
    }

    /// <summary>
    /// 创建Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <returns>Toml表格</returns>
    private TomlTable CreateTomlTable(object source)
    {
        if (source is not Type type)
            type = source.GetType();
        RunMethodOnSerializingWithClass(source, type);
        GetMethods(type, out var methodOnSerializing, out var methodOnSerialized);
        RunMethodOnSerializing(source, methodOnSerializing);

        var table = new TomlTable();
        // 获取所有属性
        var properties = GetProperties(source);
        var iTomlClass = source as ITOMLClassComment;
        // 设置注释
        if (iTomlClass is not null)
            table.Comment = iTomlClass.ClassComment;

        ParseProperties(source, table, properties, iTomlClass);

        RunMethodOnSerializedWithClass(source, type);
        RunMethodOnSerialized(source, methodOnSerialized);
        return table;
    }

    /// <summary>
    /// 解析属性
    /// </summary>
    /// <param name="source">源头</param>
    /// <param name="table">Toml表格数据</param>
    /// <param name="properties">多个属性</param>
    /// <param name="iTomlClass">Toml注释接口</param>
    void ParseProperties(
        object source,
        TomlTable table,
        IEnumerable<PropertyInfo> properties,
        ITOMLClassComment? iTomlClass
    )
    {
        foreach (var propertyInfo in properties)
        {
            // 检测是否有隐藏特性
            if (Attribute.IsDefined(propertyInfo, typeof(TOMLIgnoreAttribute)))
                continue;
            // 跳过ITomlClass生成的接口
            if (
                iTomlClass is not null
                && (
                    propertyInfo.Name == nameof(ITOMLClassComment.ClassComment)
                    || propertyInfo.Name == nameof(ITOMLClassComment.ValueComments)
                )
            )
                continue;
            // 获取属性的值
            if (propertyInfo.GetValue(source) is not object value)
                continue;
            // 获取名称
            var name = GetTomlKeyName(propertyInfo) ?? propertyInfo.Name;
            // 创建Toml节点
            var node = CheckTomlConverter(value, propertyInfo) ?? CreateTomlNode(value);
            table.TryAdd(name, node);
            // 设置注释
            node.Comment = SetCommentToNode(iTomlClass, propertyInfo.Name)!;
        }
    }

    /// <summary>
    /// 创建Toml数组
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>Toml数组</returns>
    private TomlArray CreateTomlArray(IEnumerable list)
    {
        var array = new TomlArray();
        foreach (var item in list)
            array.Add(CreateTomlNode(item));
        return array;
    }

    /// <summary>
    /// 创建Toml节点
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="type">类型</param>
    /// <returns>Toml节点</returns>
    private TomlNode CreateTomlNode(object source, Type? type = null)
    {
        type ??= source.GetType();
        return Type.GetTypeCode(type) switch
        {
            _ when type.IsEnum && _options.EnumToInteger is false
                => new TomlString(source.ToString()!),
            TypeCode.Boolean => new TomlBoolean((bool)source),

            TypeCode.Char => new TomlString(source.ToString()!),
            TypeCode.String => new TomlString((string)source),

            // 浮点型
            TypeCode.Single
                => new TomlFloat((double)Convert.ChangeType(source, TypeCode.Double)),
            TypeCode.Double => new TomlFloat((double)Convert.ChangeType(source, TypeCode.Double)),

            // 整型
            TypeCode.SByte
                => new TomlInteger((long)Convert.ChangeType(source, TypeCode.Int64)),
            TypeCode.Byte => new TomlInteger((long)Convert.ChangeType(source, TypeCode.Int64)),
            TypeCode.Int16 => new TomlInteger((long)Convert.ChangeType(source, TypeCode.Int64)),
            TypeCode.UInt16 => new TomlInteger((long)Convert.ChangeType(source, TypeCode.Int64)),
            TypeCode.Int32 => new TomlInteger((long)Convert.ChangeType(source, TypeCode.Int64)),
            TypeCode.UInt32 => new TomlInteger((long)Convert.ChangeType(source, TypeCode.Int64)),
            TypeCode.Int64 => new TomlInteger((long)Convert.ChangeType(source, TypeCode.Int64)),
            TypeCode.UInt64 => new TomlInteger((long)Convert.ChangeType(source, TypeCode.Int64)),

            TypeCode.DateTime => new TomlDateTimeLocal((DateTime)source),
            TypeCode.Object when source is DateTimeOffset offset => new TomlDateTimeOffset(offset),

            TypeCode.Object when source is TomlNode node => node,
            TypeCode.Object when source is IEnumerable list => CreateTomlArray(list),
            TypeCode.Object when type.IsClass => CreateTomlTable(source),
            _
                => throw new NotSupportedException(
                    $"Unknown source !\nType = {type.Name}, Source = {source}"
                )
        };
    }

    /// <summary>
    /// 检查Toml值转换特性
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="propertyInfo">属性信息</param>
    /// <returns>特性存在返回 <see cref="TomlNode"/> 否则返回 <see langword="null"/></returns>
    private static TomlNode? CheckTomlConverter(object value, PropertyInfo propertyInfo)
    {
        if (
            propertyInfo.GetCustomAttribute(typeof(TOMLConverterAttribute))
            is not TOMLConverterAttribute tomlConverter
        )
            return null;
        return tomlConverter.Write(value);
    }

    /// <summary>
    /// 获取源中的所有属性
    /// </summary>
    /// <param name="source">源</param>
    /// <returns>经过排序后的属性</returns>
    private IEnumerable<PropertyInfo> GetProperties(object source)
    {
        PropertyInfo[] properties;
        if (source is Type type)
            properties = type.GetProperties();
        else
            properties = source.GetType().GetProperties();
        // 使用自定义比较器排序
        if (_options.PropertiesOrderComparer is not null)
        {
            Array.Sort(properties, _options.PropertiesOrderComparer);
            // 判断是否倒序
            if (_options.PropertiesReverseOrder)
                Array.Reverse(properties);
            return properties;
        }
        return CheckTomlParameterOrder(properties, _options.PropertiesReverseOrder);
    }

    /// <summary>
    /// 检查Toml参数顺序属性并修改顺序
    /// </summary>
    /// <param name="properties">属性</param>
    /// <param name="descending">倒序</param>
    /// <returns>修改顺序后的属性</returns>
    private static IEnumerable<PropertyInfo> CheckTomlParameterOrder(
        IEnumerable<PropertyInfo> properties,
        bool descending
    )
    {
        if (descending is false)
            return properties.OrderBy(p => GetPropertyOrder(p)).ThenBy(p => p.Name);
        else
            return properties.OrderByDescending(p => GetPropertyOrder(p)).ThenBy(p => p.Name);
    }

    /// <summary>
    /// 获取属性的顺序
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <returns>属性的顺序</returns>
    private static int GetPropertyOrder(PropertyInfo property)
    {
        if (
            property.GetCustomAttribute<TOMLPropertyOrderAttribute>()
            is TOMLPropertyOrderAttribute parameterOrder
        )
            return parameterOrder.Value;
        return int.MaxValue;
    }

    /// <summary>
    /// 获取Toml键名特性的值
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    private static string? GetTomlKeyName(PropertyInfo propertyInfo)
    {
        // 检查TomlName特性
        if (
            propertyInfo.GetCustomAttribute<TOMLPropertyNameAttribute>()
            is not TOMLPropertyNameAttribute tomlName
        )
            return null;
        if (string.IsNullOrWhiteSpace(tomlName.Value))
            return null;
        return tomlName.Value;
    }

    /// <summary>
    /// 设置注释
    /// </summary>
    /// <param name="iTomlClass">TomlClass接口</param>
    /// <param name="name">键名</param>
    private static string SetCommentToNode(ITOMLClassComment? iTomlClass, string name)
    {
        // 检查值注释
        if (iTomlClass?.ValueComments?.TryGetValue(name, out var comment) is true)
            return comment;
        return string.Empty;
    }
    #endregion
    #region RunMethod
    /// <summary>
    /// 获取方法
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="methodOnSerializing">运行于序列化之前的方法</param>
    /// <param name="methodOnSerialized">运行于序列化之后的方法</param>
    private static void GetMethods(
        Type type,
        out IEnumerable<MethodAndParameters> methodOnSerializing,
        out IEnumerable<MethodAndParameters> methodOnSerialized
    )
    {
        List<MethodAndParameters> tempMethodOnSerializing = new();
        List<MethodAndParameters> tempMethodOnSerialized = new();
        foreach (var method in TOMLUtils.GetRuntimeMethodsNotContainProperty(type))
        {
            if (
                method.GetCustomAttribute(typeof(RunOnTOMLSerializingAttribute))
                is RunOnTOMLSerializingAttribute runOnTomlSerializingAttribute
            )
            {
                tempMethodOnSerializing.Add(new(method, runOnTomlSerializingAttribute.Parameters));
            }
            else if (
                method.GetCustomAttribute(typeof(RunOnTOMLSerializedAttribute))
                is RunOnTOMLSerializedAttribute runOnTomlSerializedAttribute
            )
            {
                tempMethodOnSerialized.Add(new(method, runOnTomlSerializedAttribute.Parameters));
            }
        }
        methodOnSerializing = tempMethodOnSerializing;
        methodOnSerialized = tempMethodOnSerialized;
    }

    /// <summary>
    /// 运行序列化之前的方法
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="methodOnSerializing">运行于序列化之前的方法</param>
    private static void RunMethodOnSerializing(
        object target,
        IEnumerable<MethodAndParameters> methodOnSerializing
    )
    {
        foreach (var mp in methodOnSerializing)
        {
            mp.Method.Invoke(target, mp.Parameters);
        }
    }

    /// <summary>
    /// 运行序列化之后的方法
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="methodOnSerialized">运行于序列化之后的方法</param>
    private static void RunMethodOnSerialized(
        object target,
        IEnumerable<MethodAndParameters> methodOnSerialized
    )
    {
        foreach (var mp in methodOnSerialized)
        {
            mp.Method.Invoke(target, mp.Parameters);
        }
    }

    /// <summary>
    /// 运行反序列化时,类附加的方法
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">类型</param>
    private static void RunMethodOnSerializingWithClass(object target, Type type)
    {
        if (
            type.GetCustomAttribute(typeof(RunOnTOMLSerializingAttribute))
            is not RunOnTOMLSerializingAttribute runOnTomlSerializing
        )
            return;
        runOnTomlSerializing.Method?.Invoke(target, runOnTomlSerializing.Parameters);
    }

    /// <summary>
    /// 运行反序列化后,类附加的方法
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">类型</param>
    private static void RunMethodOnSerializedWithClass(object target, Type type)
    {
        if (
            type.GetCustomAttribute(typeof(RunOnTOMLSerializedAttribute))
            is not RunOnTOMLSerializedAttribute runOnTomlSerialized
        )
            return;
        runOnTomlSerialized.Method?.Invoke(target, runOnTomlSerialized.Parameters);
    }

    #endregion
}
