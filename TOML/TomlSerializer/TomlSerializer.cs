using System.Collections;
using System.Reflection;
using HKW.TOML.TomlAttribute;
using HKW.TOML.TomlInterface;

namespace HKW.TOML.TomlSerializer;

/// <summary>
/// Toml序列化
/// </summary>
public class TomlSerializer
{
    /// <summary>
    /// Toml序列化设置
    /// </summary>
    private static TomlSerializerOptions s_options = null!;

    private TomlSerializer() { }

    /// <summary>
    /// 序列化至Toml文件
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="source">源</param>
    /// <param name="options">序列化设置</param>
    public static void SerializeToFile(
        string tomlFile,
        object source,
        TomlSerializerOptions? options = null
    )
    {
        Serialize(source, options).SaveToFile(tomlFile);
    }

    /// <summary>
    /// 异步序列化至Toml文件
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="source">源</param>
    /// <param name="options">序列化设置</param>
    public static async Task SerializeToFileAsync(
        string tomlFile,
        object source,
        TomlSerializerOptions? options = null
    )
    {
        (await SerializeAsync(source, options)).SaveToFile(tomlFile);
    }

    /// <summary>
    /// 序列化至Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static TomlTable Serialize(object source, TomlSerializerOptions? options = null)
    {
        return PreviewSerialize(source, options);
    }

    /// <summary>
    /// 异步序列化至Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static async Task<TomlTable> SerializeAsync(
        object source,
        TomlSerializerOptions? options = null
    )
    {
        return await Task.Run(() =>
        {
            return PreviewSerialize(source, options);
        });
    }

    /// <summary>
    /// 序列化静态类至Toml文件
    /// </summary>
    /// <param name="options">序列化设置</param>
    /// <param name="staticClassType">静态类类型</param>
    /// <param name="tomlFile">Toml文件</param>
    public static void SerializeStaticToFile(string tomlFile, Type staticClassType, TomlSerializerOptions? options = null)
    {
        SerializeToFile(tomlFile, staticClassType, options);
    }

    /// <summary>
    /// 异步序列化静态类至Toml文件
    /// </summary>
    /// <param name="options">序列化设置</param>
    /// <param name="staticClassType">静态类类型</param>
    /// <param name="tomlFile">Toml文件</param>
    public static async Task SerializeStaticToFileAsync(string tomlFile, Type staticClassType, TomlSerializerOptions? options = null)
    {
        await SerializeToFileAsync(tomlFile, staticClassType, options);
    }

    /// <summary>
    /// 序列化静态类至Toml表格
    /// </summary>
    /// <param name="staticClassType">静态类类型</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static TomlTable SerializeStatic(Type staticClassType, TomlSerializerOptions? options = null)
    {
        return Serialize(staticClassType, options);
    }

    /// <summary>
    /// 异步序列化静态类至Toml表格
    /// </summary>
    /// <param name="staticClassType">静态类类型</param>
    /// <param name="options">序列化设置</param>
    /// <returns>Toml表格数据</returns>
    public static async Task<TomlTable> SerializeStaticAsync(
        Type staticClassType,
        TomlSerializerOptions? options = null
    )
    {
        return await SerializeAsync(staticClassType, options);
    }

    /// <summary>
    /// 预览序列化
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="options">设置</param>
    /// <returns>Toml表格</returns>
    private static TomlTable PreviewSerialize(object source, TomlSerializerOptions? options = null)
    {
        s_options = options ?? new();
        var table = CreateTomlTable(source);
        s_options = null!;
        return table;
    }

    /// <summary>
    /// 创建Toml表格
    /// </summary>
    /// <param name="source">源</param>
    /// <returns>Toml表格</returns>
    private static TomlTable CreateTomlTable(object source)
    {
        var table = new TomlTable();
        // 获取所有属性
        var properties = GetProperties(source);
        var iTomlClass = source as ITomlClassComment;
        // 设置注释
        if (iTomlClass is not null)
            table.Comment = iTomlClass.ClassComment;
        foreach (var propertyInfo in properties)
        {
            // 检测是否有隐藏特性
            if (Attribute.IsDefined(propertyInfo, typeof(TomlIgnoreAttribute)))
                continue;
            // 跳过ITomlClass生成的接口
            if (
                iTomlClass is not null
                && (
                    propertyInfo.Name == nameof(ITomlClassComment.ClassComment)
                    || propertyInfo.Name == nameof(ITomlClassComment.ValueComments)
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
            propertyInfo.GetCustomAttribute(typeof(TomlConverterAttribute))
            is not TomlConverterAttribute tomlConverter
        )
            return null;
        return tomlConverter.Write(value);
    }

    /// <summary>
    /// 获取源中的所有属性
    /// </summary>
    /// <param name="source">源</param>
    /// <returns>经过排序后的属性</returns>
    private static IEnumerable<PropertyInfo> GetProperties(object source)
    {
        PropertyInfo[] properties;
        if (source is Type type)
            properties = type.GetProperties();
        else
            properties = source.GetType().GetProperties();
        // 使用自定义比较器排序
        if (s_options.PropertiesOrderComparer is not null)
            Array.Sort(properties, s_options.PropertiesOrderComparer);
        // 判断是否倒序
        if (s_options.PropertiesReverseOrder)
            Array.Reverse(properties);

        return CheckTomlParameterOrder(properties);
    }

    /// <summary>
    /// 检查Toml参数顺序属性并修改顺序
    /// </summary>
    /// <param name="properties">属性</param>
    /// <returns>修改顺序后的属性</returns>
    private static IEnumerable<PropertyInfo> CheckTomlParameterOrder(
        IEnumerable<PropertyInfo> properties
    )
    {
        var newProperties = new List<PropertyInfo>();
        foreach (PropertyInfo property in properties)
        {
            if (
                property.GetCustomAttribute<TomlPropertyOrderAttribute>()
                is TomlPropertyOrderAttribute parameterOrder
            )
                newProperties.Insert(parameterOrder.Value, property);
            else
                newProperties.Add(property);
        }
        return newProperties;
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
            propertyInfo.GetCustomAttribute<TomlPropertyNameAttribute>()
            is not TomlPropertyNameAttribute tomlName
        )
            return null;
        if (string.IsNullOrWhiteSpace(tomlName.Value))
            return null;
        return tomlName.Value;
    }

    /// <summary>
    /// 创建Toml数组
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>Toml数组</returns>
    private static TomlArray CreateTomlArray(IList list)
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
    private static TomlNode CreateTomlNode(object source, Type? type = null)
    {
        type ??= source.GetType();
        return Type.GetTypeCode(type) switch
        {
            TypeCode.Boolean => new TomlBoolean { Value = (bool)source },

            TypeCode.Char => new TomlString { Value = source.ToString()! },
            TypeCode.String => new TomlString { Value = (string)source },

            // 浮点型
            TypeCode.Single
                => new TomlFloat { Value = (double)Convert.ChangeType(source, TypeCode.Double) },
            TypeCode.Double
                => new TomlFloat { Value = (double)Convert.ChangeType(source, TypeCode.Double) },

            // 整型
            TypeCode.SByte
                => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            TypeCode.Byte
                => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            TypeCode.Int16
                => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            TypeCode.UInt16
                => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            TypeCode.Int32
                => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            TypeCode.UInt32
                => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            TypeCode.Int64
                => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },
            TypeCode.UInt64
                => new TomlInteger { Value = (long)Convert.ChangeType(source, TypeCode.Int64) },

            TypeCode.DateTime => new TomlDateTimeLocal { Value = (DateTime)source },
            TypeCode.Object when source is DateTimeOffset offset
                => new TomlDateTimeOffset { Value = offset },

            TypeCode.Object when source is TomlNode node => node,
            TypeCode.Object when source is IList list => CreateTomlArray(list),
            TypeCode.Object when type.IsClass => CreateTomlTable(source),
            _
                => throw new NotSupportedException(
                    $"Unknown source !\nType = {type.Name}, Source = {source}"
                )
        };
    }

    /// <summary>
    /// 设置注释
    /// </summary>
    /// <param name="iTomlClass">TomlClass接口</param>
    /// <param name="name">键名</param>
    private static string SetCommentToNode(ITomlClassComment? iTomlClass, string name)
    {
        // 检查值注释
        if (iTomlClass?.ValueComments?.TryGetValue(name, out var comment) is true)
            return comment;
        return string.Empty;
    }
}
