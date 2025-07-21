using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using HKW.FastMember;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Exceptions;
using HKW.HKWTOML.Interfaces;
using HKW.HKWUtils.Extensions;

namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化
/// <para>
/// 要反序列化静态类请使用 Deserialize(typeof(StaticObject), tomlData)
/// </para>
/// </summary>
public class TomlDeserializer
{
    #region Deserialize

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(TomlTable table, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        return Deserialize(new T(), table, options);
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<T> DeserializeAsync<T>(
        TomlTable table,
        TomlDeserializerOptions? options = null
    )
        where T : class, new()
    {
        return Task.FromResult(Deserialize<T>(table, options));
    }

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(
        T target,
        TomlTable table,
        TomlDeserializerOptions? options = null
    )
    {
        var deserializer = new TomlDeserializer(options);
        deserializer.Deserialize(target, typeof(T), table);
        return target;
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<T> DeserializeAsync<T>(
        T target,
        TomlTable table,
        TomlDeserializerOptions? options = null
    )
    {
        return Task.FromResult(Deserialize<T>(target, table, options));
    }

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static object Deserialize(
        object target,
        TomlTable table,
        TomlDeserializerOptions? options = null
    )
    {
        var deserializer = new TomlDeserializer(options);
        deserializer.Deserialize(target, target.GetType(), table);
        return target;
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<object> DeserializeAsync(
        object target,
        TomlTable table,
        TomlDeserializerOptions? options = null
    )
    {
        return Task.FromResult(Deserialize(target, table, options));
    }

    #region DeserializeFromData

    /// <summary>
    /// 从Toml数据反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(string tomlData, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        return Deserialize<T>(TOML.Parse(tomlData), options);
    }

    /// <summary>
    /// 从Toml数据异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<T> DeserializeAsync<T>(
        string tomlData,
        TomlDeserializerOptions? options = null
    )
        where T : class, new()
    {
        return Task.FromResult(Deserialize<T>(TOML.Parse(tomlData), options));
    }

    /// <summary>
    /// 从Toml数据反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(
        T target,
        string tomlData,
        TomlDeserializerOptions? options = null
    )
    {
        return Deserialize<T>(target, TOML.Parse(tomlData), options);
    }

    /// <summary>
    /// 从Toml数据异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<T> DeserializeAsync<T>(
        T target,
        string tomlData,
        TomlDeserializerOptions? options = null
    )
    {
        return Task.FromResult(Deserialize(target, tomlData, options));
    }

    /// <summary>
    /// 从Toml数据反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static object Deserialize(
        object target,
        string tomlData,
        TomlDeserializerOptions? options = null
    )
    {
        return Deserialize(target, TOML.Parse(tomlData), options);
    }

    /// <summary>
    /// 从Toml数据异步反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<object> DeserializeAsync(
        object target,
        string tomlData,
        TomlDeserializerOptions? options = null
    )
    {
        return Task.FromResult(Deserialize(target, tomlData, options));
    }
    #endregion

    #region DeserializeFromFile

    /// <summary>
    /// 从Toml文件反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T DeserializeFromFile<T>(string tomlFile, TomlDeserializerOptions? options = null)
        where T : class, new()
    {
        return Deserialize<T>(TOML.ParseFromFile(tomlFile), options);
    }

    /// <summary>
    /// 从Toml文件异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<T> DeserializeFromFileAsync<T>(
        string tomlFile,
        TomlDeserializerOptions? options = null
    )
        where T : class, new()
    {
        return Task.FromResult(DeserializeFromFile<T>(tomlFile, options));
    }

    /// <summary>
    /// 从Toml文件反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T DeserializeFromFile<T>(
        T target,
        string tomlFile,
        TomlDeserializerOptions? options = null
    )
    {
        return Deserialize<T>(target, TOML.ParseFromFile(tomlFile), options);
    }

    /// <summary>
    /// 从Toml文件异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<T> DeserializeFromFileAsync<T>(
        T target,
        string tomlFile,
        TomlDeserializerOptions? options = null
    )
    {
        return Task.FromResult(DeserializeFromFile(target, tomlFile, options));
    }

    /// <summary>
    /// 从Toml文件反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static object DeserializeFromFile(
        object target,
        string tomlFile,
        TomlDeserializerOptions? options = null
    )
    {
        return Deserialize(target, TOML.ParseFromFile(tomlFile), options);
    }

    /// <summary>
    /// 从Toml文件异步反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static Task<object> DeserializeFromFileAsync(
        object target,
        string tomlFile,
        TomlDeserializerOptions? options = null
    )
    {
        return Task.FromResult(DeserializeFromFile(target, tomlFile, options));
    }
    #endregion

    #endregion

    /// <summary>
    /// 设置
    /// </summary>
    private readonly TomlDeserializerOptions _options;

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
    private TomlDeserializer(TomlDeserializerOptions? options)
    {
        if (options is null)
            _isDefaultOptions = true;
        _options = options ?? new();

        if (_options.AllowStaticProperty)
            _propertyBindingFlags |= BindingFlags.Static;
        if (_options.AllowNonPublicProperty)
            _propertyBindingFlags |= BindingFlags.NonPublic;
    }

    #region Deserialize Value
    /// <summary>
    /// 预处理
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    private void Deserialize(object? target, Type type, TomlTable table)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        // 检查是否为静态类
        if (target is Type staticType && staticType.IsSealed && staticType.IsAbstract)
        {
            if (_options.AllowStaticProperty is false)
                throw new TomlDeserializeException(
                    "如果要反序列化静态对象请将选项中的 AllowStaticProperty 设置为 true"
                );
            type = staticType;
        }

        DeserializeTable(target, type, table);

        // 检查缺失的必要属性
        if (_options.MissingPequiredProperties.Count != 0)
        {
            if (_isDefaultOptions)
                throw new TomlDeserializeException(
                    "反序列化时缺少必需属性" + Environment.NewLine + "请向反序列化器传递选项以获取缺少的必需属性。"
                );
        }
    }

    /// <summary>
    /// 反序列化Toml表格
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    private void DeserializeTable(object target, Type type, TomlTable table)
    {
        var accessor = ObjectAccessor.Create(target);
        // 设置注释
        var iTomlClass = target as ITomlObjectComment;
        if (iTomlClass is not null)
        {
            iTomlClass.ObjectComment = table.Comment ?? string.Empty;
            iTomlClass.PropertyComments ??= [];
        }

        var properties = type.GetPropertiesWithoutIgnore(_propertyBindingFlags, false);
        for (var i = 0; i < properties.Length; i++)
        {
            var propertyInfo = properties[i];
            try
            {
                if (DeserializeProperty(accessor, propertyInfo, table, iTomlClass) is false)
                    _options.MissingPequiredProperties.Add($"{type.FullName}.{propertyInfo.Name}");
            }
            catch (Exception ex)
            {
                switch (_options.ExceptionHandling)
                {
                    case ExceptionHandlingMode.Ignore:
                        return;
                    case ExceptionHandlingMode.Throw:
                        throw;
                    case ExceptionHandlingMode.Record:
                        _options.Exceptions.TryAdd($"{type.FullName}.{propertyInfo.Name}", ex);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 反序列化字典
    /// </summary>
    /// <param name="table">Toml表格</param>
    /// <param name="nestedTarget">内部目标</param>
    private static bool DeserializeDictionary(TomlTable table, object nestedTarget)
    {
        if (nestedTarget is not IDictionary dictionary)
            return false;
        foreach (var item in table)
            dictionary.Add(item.Key, item.Value);
        return true;
    }

    /// <summary>
    /// 反序列化Toml数组
    /// </summary>
    /// <param name="type">属性类型</param>
    /// <param name="list">列表</param>
    /// <param name="array">数组</param>
    private void DeserializeArray(Type type, IList list, TomlArray array)
    {
        if (array.Count == 0)
            return;

        // 获取列表值的类型
        var genericArguments = type.GetGenericArguments();
        if (genericArguments.Length == 0)
            return;

        var elementType = genericArguments[0];

        // 如果泛型类型是枚举
        if (elementType.IsEnum)
        {
            foreach (var node in array)
                list.Add(GetEnumValue(elementType, node));
            return;
        }

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
    private void DeserializeArrayValue(
        IList list,
        TomlNode node,
        Type elementType,
        TypeCode typeCode
    )
    {
        var typeAccessor = TypeAccessor.Create(elementType);
        if (node.IsTomlTable)
        {
            // 如果值是Toml表格,则创建一个新的对象
            if (typeAccessor.CreateNew() is not object nestedTarget)
                return;

            // 如果是字典, 则填入内容
            if (DeserializeDictionary(node.AsTomlTable, nestedTarget))
                return;
            list.Add(nestedTarget);

            DeserializeTable(nestedTarget, nestedTarget.GetType(), node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            // 如果是Toml数组,则检测是否为IList类型
            if (typeAccessor.CreateNew() is not IList nestedTarget)
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
    /// 反序列化属性
    /// </summary>
    /// <param name="accessor">访问器</param>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="table">Toml表格</param>
    /// <param name="iTomlClassComment">Toml注释接口</param>
    /// <returns>解析成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    private bool DeserializeProperty(
        ObjectAccessor accessor,
        PropertyInfo propertyInfo,
        TomlTable table,
        ITomlObjectComment? iTomlClassComment
    )
    {
        // 跳过ITomlClassComment生成的接口
        if (iTomlClassComment is not null)
        {
            if (propertyInfo.Name == nameof(ITomlObjectComment.ObjectComment))
                return true;
            else if (propertyInfo.Name == nameof(ITomlObjectComment.PropertyComments))
                return true;
        }

        // 获取属性名
        var name = GetPropertyName(propertyInfo);

        if (table.TryGetValue(name, out var node) is false)
        {
            // 如果这是必要属性, 则返回失败
            if (propertyInfo.GetCustomAttribute<TomlRequiredAttribute>() is not null)
                return false;
            return true;
        }

        // 设置注释
        iTomlClassComment?.PropertyComments.TryAdd(name, node.Comment ?? string.Empty);
        DeserializePropertyValue(accessor, node, propertyInfo);
        return true;
    }

    /// <summary>
    /// 反序列化属性值
    /// </summary>
    /// <param name="accessor">访问器</param>
    /// <param name="node">值</param>
    /// <param name="propertyInfo">属性信息</param>
    private void DeserializePropertyValue(
        ObjectAccessor accessor,
        TomlNode node,
        PropertyInfo propertyInfo
    )
    {
        var propertyType = propertyInfo.PropertyType;
        var typeAccessor = TypeAccessor.Create(propertyType);

        // 检测TomlConverter
        if (
            propertyInfo.IsDefined<TomlConverterAttribute>()
            && propertyInfo.GetCustomAttribute<TomlConverterAttribute>()
                is TomlConverterAttribute tomlConverter
        )
        {
            SetPropertyValue(accessor, propertyInfo, tomlConverter.Converter.Converte(node));
            return;
        }

        if (node.IsTomlTable)
        {
            // 如果值是Toml表格,则创建一个新的对象
            if (typeAccessor.CreateNew() is not object nestedTarget)
                return;

            // 如果是字典, 则填入内容
            if (DeserializeDictionary(node.AsTomlTable, nestedTarget))
                return;
            SetPropertyValue(accessor, propertyInfo, nestedTarget);

            // 递归Toml表格
            DeserializeTable(nestedTarget, propertyType, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            // 如果是Toml数组,则检测是否为IList类型
            if (typeAccessor.CreateNew() is not IList nestedTarget)
                return;
            SetPropertyValue(accessor, propertyInfo, nestedTarget);
            DeserializeArray(propertyType, nestedTarget, node.AsTomlArray);
        }
        else if (propertyType.IsEnum)
        {
            // 获取枚举值
            var value = GetEnumValue(propertyType, node);
            SetPropertyValue(accessor, propertyInfo, value);
        }
        else
        {
            // 获取属性值
            var value = GetNodeVale(node, Type.GetTypeCode(propertyType));
            SetPropertyValue(accessor, propertyInfo, value);
        }
    }

    /// <summary>
    /// 获取属性名称
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <returns>属性名称</returns>
    private static string GetPropertyName(PropertyInfo propertyInfo)
    {
        // 获取TomlKeyName
        if (
            propertyInfo.IsDefined<TomlPropertyNameAttribute>()
            && propertyInfo.GetCustomAttribute<TomlPropertyNameAttribute>()
                is TomlPropertyNameAttribute keyName
        )
            return keyName.Value;
        else
            return propertyInfo.Name;
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
                => Convert.ToSingle(node.AsDouble),
            TypeCode.Double => Convert.ToDouble(node.AsDouble),

            // 整型
            TypeCode.SByte
                => Convert.ToSByte(node.AsInt64),
            TypeCode.Byte => Convert.ToByte(node.AsInt64),
            TypeCode.Int16 => Convert.ToInt16(node.AsInt64),
            TypeCode.UInt16 => Convert.ToUInt16(node.AsInt64),
            TypeCode.Int32 => Convert.ToInt32(node.AsInt64),
            TypeCode.UInt32 => Convert.ToUInt32(node.AsInt64),
            TypeCode.Int64 => Convert.ToInt64(node.AsInt64),
            TypeCode.UInt64 => Convert.ToUInt64(node.AsInt64),

            TypeCode.DateTime => node.AsDateTime,
            TypeCode.Object when node.IsTomlDateTimeOffset => node.AsDateTimeOffset,
            _ => node,
        };
    }
    #endregion

    #region GetEnumValue

    /// <summary>
    /// 获取枚举值
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    /// <param name="node">Toml节点</param>
    /// <returns>获取的枚举值</returns>
    private object? GetEnumValue(Type enumType, TomlNode node)
    {
        if (node.IsTomlString)
        {
            return Enum.Parse(enumType, node.AsString, _options.EnumIgnoreCase);
        }
        else if (node.IsTomlInteger)
        {
            return Enum.ToObject(enumType, node.AsInt64);
        }
        else
            throw new NotSupportedException($"不支持的TomlNode类型 {node.GetType().Name} 用于枚举转换");
    }

    #endregion

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="accessor">访问器</param>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="value">值</param>
    private void SetPropertyValue(ObjectAccessor accessor, PropertyInfo propertyInfo, object? value)
    {
        if (_options.AllowStaticProperty && propertyInfo.IsStatic())
            propertyInfo.SetValue(accessor.Source, value);
        else
            accessor[propertyInfo.Name] = value!;
    }
}
