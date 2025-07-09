using System.Collections;
using System.Reflection;
using HKW.FastMember;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Exceptions;
using HKW.HKWTOML.Interfaces;
using HKWTOML.Utils;

namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化
/// <para>
/// 要反序列化静态类请使用 Deserialize(typeof(StaticObject), tomlData)
/// </para>
/// </summary>
public class TOMLDeserializer
{
    #region Deserialize

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="table">Toml表格</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(TomlTable table, TOMLDeserializerOptions? options = null)
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
    public static async Task<T> DeserializeAsync<T>(
        TomlTable table,
        TOMLDeserializerOptions? options = null
    )
        where T : class, new()
    {
        return await Task.Run(() => Deserialize<T>(table, options));
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
        TOMLDeserializerOptions? options = null
    )
    {
        var deserializer = new TOMLDeserializer(options);
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
    public static async Task<T> DeserializeAsync<T>(
        T target,
        TomlTable table,
        TOMLDeserializerOptions? options = null
    )
    {
        return await Task.Run(() => Deserialize<T>(target, table, options));
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
        TOMLDeserializerOptions? options = null
    )
    {
        var deserializer = new TOMLDeserializer(options);
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
    public static async Task<object> DeserializeAsync(
        object target,
        TomlTable table,
        TOMLDeserializerOptions? options = null
    )
    {
        return await Task.Run(() => Deserialize(target, table, options));
    }

    #region DeserializeFromData

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(string tomlData, TOMLDeserializerOptions? options = null)
        where T : class, new()
    {
        return Deserialize<T>(TOML.Parse(tomlData), options);
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeAsync<T>(
        string tomlData,
        TOMLDeserializerOptions? options = null
    )
        where T : class, new()
    {
        return await Task.Run(() => Deserialize<T>(TOML.Parse(tomlData), options));
    }

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(
        T target,
        string tomlData,
        TOMLDeserializerOptions? options = null
    )
    {
        return Deserialize<T>(target, TOML.Parse(tomlData), options);
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeFromFileAsync<T>(
        T target,
        string tomlData,
        TOMLDeserializerOptions? options = null
    )
    {
        return await Task.Run(() => Deserialize(target, tomlData, options));
    }

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static object Deserialize(
        object target,
        string tomlData,
        TOMLDeserializerOptions? options = null
    )
    {
        return Deserialize(target, TOML.Parse(tomlData), options);
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="tomlData">Toml数据</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<object> DeserializeAsync(
        object target,
        string tomlData,
        TOMLDeserializerOptions? options = null
    )
    {
        return await Task.Run(() => Deserialize(target, tomlData, options));
    }
    #endregion

    #region DeserializeFromFile

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T DeserializeFromFile<T>(string tomlFile, TOMLDeserializerOptions? options = null)
        where T : class, new()
    {
        return Deserialize<T>(TOML.ParseFromFile(tomlFile), options);
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeFromFileAsync<T>(
        string tomlFile,
        TOMLDeserializerOptions? options = null
    )
        where T : class, new()
    {
        return await Task.Run(() => DeserializeFromFile<T>(tomlFile, options));
    }

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static T DeserializeFromFile<T>(
        T target,
        string tomlFile,
        TOMLDeserializerOptions? options = null
    )
    {
        return Deserialize<T>(target, TOML.ParseFromFile(tomlFile), options);
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeAsync<T>(
        T target,
        string tomlFile,
        TOMLDeserializerOptions? options = null
    )
    {
        return await Task.Run(() => DeserializeFromFile(target, tomlFile, options));
    }

    /// <summary>
    /// 从Toml表格反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static object DeserializeFromFile(
        object target,
        string tomlFile,
        TOMLDeserializerOptions? options = null
    )
    {
        return DeserializeFromFile(target, tomlFile, options);
    }

    /// <summary>
    /// 从Toml表格异步反序列化
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<object> DeserializeFromFileAsync(
        object target,
        string tomlFile,
        TOMLDeserializerOptions? options = null
    )
    {
        return await Task.Run(() => DeserializeFromFile(target, tomlFile, options));
    }
    #endregion

    #endregion

    /// <summary>
    /// 设置
    /// </summary>
    private readonly TOMLDeserializerOptions _options;

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
    private TOMLDeserializer(TOMLDeserializerOptions? options)
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
                    "Target is static object but Options.AllowStaticProperty is false"
                        + Environment.NewLine
                        + "If you want to deserialize a static object please set Options.AllowStaticProperty to true"
                );
            type = staticType;
        }

        DeserializeTable(target, type, table);

        // 检查缺失的必要属性
        if (_options.MissingPequiredProperties.Count != 0)
        {
            if (_isDefaultOptions)
                throw new TomlDeserializeException(
                    "Deserialization with missing required properties"
                        + Environment.NewLine
                        + "Please pass a options to the deserializer to get the missing required properties."
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
        // 设置键比较器
        var keyComparison = table.KeyComparison;
        table.KeyComparison = _options.KeyComparison;
        // 设置注释
        var iTomlClass = target as ITomlObjectComment;
        if (iTomlClass is not null)
        {
            iTomlClass.ObjectComment = table.Comment ?? string.Empty;
            iTomlClass.PropertyComments ??= [];
        }

        foreach (var propertyInfo in type.GetProperties(_propertyBindingFlags))
        {
            try
            {
                if (DeserializeProperty(accessor, propertyInfo, table, iTomlClass) is false)
                    _options.MissingPequiredProperties.Add($"{type.FullName}.{propertyInfo.Name}");
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

        // 恢复键比较器
        table.KeyComparison = keyComparison;
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
        if (array.Any() is false)
            return;
        // 获取列表值的类型
        if (type.GetGenericArguments()[0] is not Type elementType)
            return;
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
        // 检测是否为隐藏属性
        if (propertyInfo.GetCustomAttribute<TomlIgnoreAttribute>() is not null)
            return true;
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
            propertyInfo.GetCustomAttribute<TomlConverterAttribute>()
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
            {
                SetPropertyValue(accessor, propertyInfo, nestedTarget);
                return;
            }
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
            propertyInfo.GetCustomAttribute<TomlPropertyNameAttribute>()
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
        if (node is TomlString)
        {
            return Enum.ToObject(enumType, node.AsInt64);
        }
        else if (node is TomlInteger)
        {
            return Enum.Parse(enumType, node.AsString, _options.EnumIgnoreCase);
        }
        else
            throw new NotImplementedException($"TomlNode {node} type error");
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
