using System.Collections;
using System.Reflection;
using HKW.TOML.TomlAttribute;
using HKW.TOML.TomlInterface;
using HKW.TOML.TomlException;
using System.Linq;
using HKWToml.Utils;

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
    /// 所有单词分隔符
    /// </summary>
    private static char[] s_keyWordSeparators = null!;

    /// <summary>
    /// 从Toml文件反序列化
    /// </summary>
    /// <typeparam name="T">targetType</typeparam>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="options">反序列化设置</param>
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
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
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
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
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static T Deserialize<T>(TomlTable table, TomlDeserializerOptions? options = null)
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
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    /// <returns>完成反序列化的对象</returns>
    public static async Task<T> DeserializeAsync<T>(
        TomlTable table,
        TomlDeserializerOptions? options = null
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
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    public static void DeserializeStaticFromFile(
        string tomlFile,
        Type staticClass,
        TomlDeserializerOptions? options = null
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
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    public static async Task DeserializeStaticFromFileAsync(
        string tomlFile,
        Type staticClass,
        TomlDeserializerOptions? options = null
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
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    public static void DeserializeStatic(
        TomlTable table,
        Type staticClass,
        TomlDeserializerOptions? options = null
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
    /// <exception cref="ConsistencyException">一致性异常</exception>
    /// <exception cref="MissingRequiredException">缺失必要属性异常</exception>
    public static async Task DeserializeStaticeAsync(
        TomlTable table,
        Type staticClass,
        TomlDeserializerOptions? options = null
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
    /// <exception cref="ConsistencyException">一致性异常</exception>
    private static void PreviewDeserialize(
        object target,
        Type type,
        TomlTable table,
        TomlDeserializerOptions? options
    )
    {
        s_options = options ?? new();
        s_keyWordSeparators = s_options.KeyWordSeparators.ToArray();
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
        s_options = null!;
    }

    /// <summary>
    /// 反序列化Toml表格
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
        // 设置一致性信息
        TryCheckConsistency(type, table, out var missingProperties, out var missingTomlNodes);

        IterationTable(target, type, table, iTomlClass, missingProperties, missingTomlNodes);

        var missingRequiredProperties = new HashSet<string>();
        // 检查TomlName特性
        CheckProperties(
            iTomlClass,
            target,
            type,
            table,
            missingProperties,
            missingTomlNodes,
            missingRequiredProperties
        );
        // 添加一致性信息
        AddMissingValue(type.FullName!, tableName, missingProperties, missingTomlNodes);
        // 添加缺失的必要属性信息
        AddMissingRequiredValue(type.FullName!, missingRequiredProperties);

        RunMethodOnDeserializedWithClass(target, type);
        RunMethodOnDeserialized(target, methodOnDeserialized);

        static void IterationTable(object target, Type type, TomlTable table, ITomlClassComment? iTomlClass, HashSet<string>? missingProperties, HashSet<string>? missingTomlNodes)
        {
            foreach (var kv in table)
            {
                var name = Utils.ToPascal(
                    kv.Key,
                    s_keyWordSeparators,
                    s_options.RemoveKeyWordSeparator
                );
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
        }
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
        foreach (var method in Utils.GetMethodInfosWithOutProperty(type))
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
    /// 尝试检查一致性
    /// </summary>
    /// <param name="type">类</param>
    /// <param name="table">Toml表格</param>
    /// <param name="missingProperties">缺失的属性</param>
    /// <param name="missingTomlNodes">缺失的Toml节点</param>
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

    /// <summary>
    /// 检查一致性
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <param name="keyName">键名</param>
    /// <param name="missingProperties">缺失的属性</param>
    /// <param name="missingTomlNodes">缺失的Toml节点</param>
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
    /// 检查TomlName特性
    /// </summary>
    /// <param name="iTomlClass">Toml类接口</param>
    /// <param name="target">目标</param>
    /// <param name="type">目标类型</param>
    /// <param name="table">Toml表格</param>
    /// <param name="missingProperties">缺失数据的属性</param>
    /// <param name="missingTomlNodes">缺失数据的Toml节点</param>
    /// <param name="missingRequiredProperties">缺失数据的必要属性</param>
    private static void CheckProperties(
        ITomlClassComment? iTomlClass,
        object target,
        Type type,
        TomlTable table,
        HashSet<string>? missingProperties,
        HashSet<string>? missingTomlNodes,
        HashSet<string> missingRequiredProperties
    )
    {
        foreach (var propertyInfo in type.GetProperties())
        {
            // 检测是否为必要属性
            if (Attribute.IsDefined(propertyInfo, typeof(TomlRequiredAttribute)))
                missingRequiredProperties.Add(propertyInfo.Name);
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
            missingRequiredProperties.Remove(propertyInfo.Name);
        }
    }
    #endregion
    /// <summary>
    /// 反序列化Toml表格的值
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
}
