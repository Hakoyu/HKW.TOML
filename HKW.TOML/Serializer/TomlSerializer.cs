using HKW.FastMember;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Exceptions;
using HKW.HKWTOML.Interfaces;
using HKWTOML.Utils;
using System.Collections;
using System.Reflection;

namespace HKW.HKWTOML.Serializer;

/// <summary>
/// Toml序列化
/// <para>
/// 要序列化静态类请使用 Serialize(typeof(StaticObject))
/// </para>
/// </summary>
public class TOMLSerializer
{
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
    #endregion

    /// <summary>
    /// Toml序列化设置
    /// </summary>
    private readonly TOMLSerializerOptions _options;

    /// <summary>
    /// 是默认设置
    /// </summary>
    private readonly bool _isDefaultOptions = false;

    /// <summary>
    /// 属性标识符
    /// </summary>
    private readonly BindingFlags _propertyBindingFlags =
        BindingFlags.Public | BindingFlags.Instance;

    /// <inheritdoc/>
    /// <param name="options">设置</param>
    private TOMLSerializer(TOMLSerializerOptions? options)
    {
        if (options is null)
            _isDefaultOptions = true;
        _options = options ?? new();

        if (_options.AllowStaticProperty)
            _propertyBindingFlags |= BindingFlags.Static;
        if (_options.AllowNonPublicProperty)
            _propertyBindingFlags |= BindingFlags.NonPublic;
    }

    #region SerializeObject

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="source">源头</param>
    /// <returns>Toml表格数据</returns>
    private TomlTable Serialize(object source)
    {
        Type type;
        // 检查是否为静态类
        if (source is Type staticType && staticType.IsSealed && staticType.IsAbstract)
        {
            if (_options.AllowStaticProperty is false)
                throw new TomlSerializeException(
                    "Target is static object but Options.AllowStaticProperty is false"
                        + Environment.NewLine
                        + "If you want to serialize a static object please set Options.AllowStaticProperty to true"
                );
            type = staticType;
        }
        else
        {
            type = source.GetType();
        }
        return SerializeTomlTable(source, type);
    }

    /// <summary>
    /// 创建Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="type">类型</param>
    /// <returns>Toml表格</returns>
    private TomlTable SerializeTomlTable(object source, Type type)
    {
        var accessor = ObjectAccessor.Create(source);
        var table = new TomlTable();
        // 获取所有属性
        var properties = GetProperties(type);
        var iTomlClass = source as ITomlObjectComment;
        // 设置注释
        if (iTomlClass is not null)
            table.Comment = iTomlClass.ClassComment;

        foreach (var propertyInfo in properties)
        {
            try
            {
                if (SerializeProperty(accessor, propertyInfo, iTomlClass) is var (name, node))
                    table.Add(name, node);
            }
            catch (Exception ex)
            {
                if (_options.ExceptionHandling is ExceptionHandlingMode.Ignore)
                    continue;
                else if (_options.ExceptionHandling is ExceptionHandlingMode.Throw)
                    throw;
                else if (_options.ExceptionHandling is ExceptionHandlingMode.Record)
                    _options.Exceptions.TryAdd($"{type.FullName}.{propertyInfo.Name}", ex);
            }
        }

        return table;
    }

    /// <summary>
    /// 反序列化属性
    /// </summary>
    /// <param name="accessor">访问器</param>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="iTomlObject"></param>
    /// <returns></returns>
    private (string name, TomlNode node)? SerializeProperty(
        ObjectAccessor accessor,
        PropertyInfo propertyInfo,
        ITomlObjectComment? iTomlObject
    )
    { // 跳过ITomlClass生成的接口
        if (
            iTomlObject is not null
            && (
                propertyInfo.Name == nameof(ITomlObjectComment.ClassComment)
                || propertyInfo.Name == nameof(ITomlObjectComment.ValueComments)
            )
        )
            return null;
        // 检测是否有隐藏特性
        if (propertyInfo.GetCustomAttribute<TOMLIgnoreAttribute>() is not null)
            return null;

        // 获取属性的值
        if (accessor[propertyInfo.Name] is not object value)
            return null;
        // 获取名称
        var name = GetTomlKeyName(propertyInfo) ?? propertyInfo.Name;

        // 创建Toml节点
        var node = CheckTomlConverter(value, propertyInfo) ?? CreateTomlNode(value);

        // 设置注释
        node.Comment = SetComment(iTomlObject, propertyInfo.Name)!;
        return (name, node);
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

            // 对象
            TypeCode.Object when source is TomlNode node
                => node,
            TypeCode.Object when source is IDictionary dictionary
                => SerializeDictionary(dictionary),
            TypeCode.Object when source is IEnumerable list => SerializeList(list),
            TypeCode.Object when type.IsClass => SerializeTomlTable(source, type),
            _
                => throw new NotSupportedException(
                    $"Unknown source !\nType = {type.Name}, Source = {source}"
                )
        };
    }

    /// <summary>
    /// 创建Toml数组
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>Toml数组</returns>
    private TomlArray SerializeList(IEnumerable list)
    {
        var array = new TomlArray();
        foreach (var item in list)
            array.Add(CreateTomlNode(item));
        return array;
    }

    /// <summary>
    /// 创建Toml表格
    /// </summary>
    /// <param name="dictionary">字典</param>
    /// <returns>Toml表格</returns>
    private TomlTable SerializeDictionary(IDictionary dictionary)
    {
        var table = new TomlTable();
        foreach (var item in dictionary)
        {
            var kv = (DictionaryEntry)item;
            table.Add(kv.Key.ToString()!, CreateTomlNode(kv.Value!));
        }
        return table;
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
    /// <param name="type">类型</param>
    /// <returns>经过排序后的属性</returns>
    private IEnumerable<PropertyInfo> GetProperties(Type type)
    {
        var properties = type.GetProperties(_propertyBindingFlags);
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
    /// <param name="iTomlObject">TomlClass接口</param>
    /// <param name="name">键名</param>
    private static string SetComment(ITomlObjectComment? iTomlObject, string name)
    {
        // 检查值注释
        if (iTomlObject?.ValueComments?.TryGetValue(name, out var comment) is true)
            return comment;
        return string.Empty;
    }
    #endregion
}
