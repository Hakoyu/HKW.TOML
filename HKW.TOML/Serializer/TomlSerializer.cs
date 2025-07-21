using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using HKW.FastMember;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Exceptions;
using HKW.HKWTOML.Interfaces;
using HKW.HKWUtils;
using HKW.HKWUtils.Collections;
using HKW.HKWUtils.Extensions;

namespace HKW.HKWTOML.Serializer;

/// <summary>
/// Toml序列化
/// <para>
/// 要序列化静态类请使用 Serialize(typeof(StaticObject))
/// </para>
/// </summary>
public class TomlSerializer
{
    #region Serialize

    /// <summary>
    /// 序列化至Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static TomlTable? Serialize(object source, TomlSerializerOptions? options = null)
    {
        var serializer = new TomlSerializer(options);
        var result = serializer.Serialize(source);
        serializer._propertiesCache.Clear();
        serializer._attributeDictionaryCache.Clear();
        return result;
    }

    /// <summary>
    /// 异步序列化至Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static Task<TomlTable?> SerializeAsync(
        object source,
        TomlSerializerOptions? options = null
    )
    {
        return Task.FromResult(Serialize(source, options));
    }
    #endregion

    /// <summary>
    /// Toml序列化设置
    /// </summary>
    private readonly TomlSerializerOptions _options;

    /// <summary>
    /// 是默认设置
    /// </summary>
    private readonly bool _isDefaultOptions = false;

    /// <summary>
    /// 属性标识符
    /// </summary>
    private readonly BindingFlags _propertyBindingFlags =
        BindingFlags.Public | BindingFlags.Instance;

    private readonly CacheDictionary<Type, PropertyInfo[]> _propertiesCache = new(16);
    private readonly CacheDictionary<PropertyInfo, AttributeDictionary> _attributeDictionaryCache =
        new(32);

    /// <inheritdoc/>
    /// <param name="options">设置</param>
    private TomlSerializer(TomlSerializerOptions? options)
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
        if (_propertiesCache.TryGetValue(type, out var properties) is false)
        {
            if (_options.PropertiesOrderMode is PropertiesOrderMode.None)
                _propertiesCache[type] = properties = type.GetPropertiesWithoutIgnore(
                    _propertyBindingFlags,
                    true
                );
            else
                _propertiesCache[type] = properties = [.. GetObjectProperties(type)];
        }
        var iTomlClass = source as ITomlObjectComment;
        // 设置注释
        if (iTomlClass is not null)
            table.Comment = iTomlClass.ObjectComment;

        for (var i = 0; i < properties.Length; i++)
        {
            var propertyInfo = properties[i];
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
    /// <returns>(属性名称,Toml节点)</returns>
    private (string name, TomlNode node)? SerializeProperty(
        ObjectAccessor accessor,
        PropertyInfo propertyInfo,
        ITomlObjectComment? iTomlObject
    )
    { // 跳过ITomlClass生成的接口
        if (
            iTomlObject is not null
            && (
                propertyInfo.Name == nameof(ITomlObjectComment.ObjectComment)
                || propertyInfo.Name == nameof(ITomlObjectComment.PropertyComments)
            )
        )
            return null;
        if (
            _attributeDictionaryCache.TryGetValue(propertyInfo, out var attributeDictionary)
            is false
        )
        {
            _attributeDictionaryCache[propertyInfo] = attributeDictionary =
                propertyInfo.GetAttributeDictionary();
        }

        // 获取属性的值
        if (accessor[propertyInfo.Name] is not object value)
            return null;
        // 获取名称
        var name = GetTomlKeyName(propertyInfo, attributeDictionary);

        // 创建Toml节点
        var node =
            CheckTomlConverter(value, attributeDictionary)
            ?? CreateTomlNode(value, propertyInfo.PropertyType);

        // 设置注释
        if (iTomlObject is not null)
            node.Comment = GetObjectComment(iTomlObject, propertyInfo.Name);
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
        return source switch
        {
            bool b => new TomlBoolean(b),
            char c => new TomlString(c.ToString()!),
            string s => new TomlString(s),
            // 浮点型
            float f => new TomlFloat(f),
            double d => new TomlFloat(d),
            // 枚举, 必须在整型前
            Enum e when _options.EnumToInteger is false => new TomlString(e.ToString()),
            // 整型
            sbyte sb => new TomlInteger(sb),
            byte b => new TomlInteger(b),
            short sh => new TomlInteger(sh),
            ushort ush => new TomlInteger(ush),
            int i => new TomlInteger(i),
            uint ui => new TomlInteger(ui),
            long l => new TomlInteger(l),
            ulong ul => new TomlInteger((long)ul),
            DateTime dt => new TomlDateTimeLocal(dt),
            DateTimeOffset dto => new TomlDateTimeOffset(dto),
            // 对象
            TomlNode node when source is TomlNode => node,
            IDictionary dictionary when source is IDictionary => SerializeDictionary(dictionary),
            IEnumerable list when source is IEnumerable => SerializeList(list),
            _ when type.IsClass || type.IsInterface => SerializeTomlTable(source, type),
            // 其他类型
            _
                => throw new NotSupportedException(
                    $"Unknown source type!\nType = {type.Name}, Source = {source}"
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
    /// <param name="attributeDictionary">属性字典</param>
    /// <returns>特性存在返回 <see cref="TomlNode"/> 否则返回 <see langword="null"/></returns>
    private static TomlNode? CheckTomlConverter(
        object value,
        AttributeDictionary attributeDictionary
    )
    {
        if (
            attributeDictionary.TryGetAttribute<TomlConverterAttribute>(out var tomlConverter)
            is false
        )
            return null;
        return tomlConverter.Converter.ConverteBack(value);
    }

    /// <summary>
    /// 获取源中的所有属性
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>经过排序后的属性</returns>
    private IEnumerable<PropertyInfo> GetObjectProperties(Type type)
    {
        var properties = type.GetPropertiesWithoutIgnore(_propertyBindingFlags, true);
        // 使用自定义比较器排序
        if (_options.PropertiesOrderComparer is not null)
        {
            Array.Sort(properties, _options.PropertiesOrderComparer);
            // 判断是否倒序
            if (_options.PropertiesOrderMode is PropertiesOrderMode.ReverseOrder)
                Array.Reverse(properties);
            return properties;
        }

        if (_options.PropertiesOrderMode is PropertiesOrderMode.Order)
            return properties.OrderBy(GetPropertyOrder);
        else
            return properties.OrderByDescending(GetPropertyOrder);
    }

    /// <summary>
    /// 获取属性的顺序
    /// </summary>
    /// <param name="property">属性信息</param>
    /// <returns>属性的顺序</returns>
    private static int GetPropertyOrder(PropertyInfo property)
    {
        if (
            property.IsDefined<TomlPropertyOrderAttribute>() is false
            || property.GetCustomAttribute<TomlPropertyOrderAttribute>()
                is not TomlPropertyOrderAttribute parameterOrder
        )
            return int.MaxValue;
        return parameterOrder.Value;
    }

    /// <summary>
    /// 获取Toml键名特性的值
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="attributeDictionary">属性字典</param>
    /// <returns>Toml键名</returns>
    private static string GetTomlKeyName(
        PropertyInfo propertyInfo,
        AttributeDictionary attributeDictionary
    )
    {
        // 检查TomlName特性
        if (
            attributeDictionary.TryGetAttribute<TomlPropertyNameAttribute>(out var tomlName)
                is false
            || string.IsNullOrWhiteSpace(tomlName.Value)
        )
            return propertyInfo.Name;
        else
            return tomlName.Value;
    }

    /// <summary>
    /// 获取对象注释
    /// </summary>
    /// <param name="iTomlObject">TomlClass接口</param>
    /// <param name="name">键名</param>
    private static string GetObjectComment(ITomlObjectComment? iTomlObject, string name)
    {
        // 检查值注释
        if (iTomlObject?.PropertyComments?.TryGetValue(name, out var comment) is true)
            return comment;
        return string.Empty;
    }
    #endregion
}
