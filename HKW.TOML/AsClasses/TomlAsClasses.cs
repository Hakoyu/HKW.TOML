using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;
using HKWTOML.Utils;
using HKW.HKWUtils.Extensions;

namespace HKW.HKWTOML.AsClasses;

/// <summary>
/// Toml转换为类
/// </summary>
public partial class TOMLAsClasses
{
    /// <summary>
    /// 所有类
    /// <para>(类名称, 类值)</para>
    /// </summary>
    private readonly Dictionary<string, TOMLClass> r_tomlClasses = new();

    /// <summary>
    /// 所有数组名称
    /// <para>(数组值类型名称, 数组名称)</para>
    /// </summary>
    private readonly Dictionary<string, string> r_arrayTypeNames = new();

    /// <summary>
    /// 设置
    /// </summary>
    internal readonly TOMLAsClassesOptions r_options = new();

    private TOMLAsClasses(TOMLAsClassesOptions? options)
    {
        r_options = options ?? new();
    }

    #region Generate
    /// <summary>
    /// 从文件中生成
    /// </summary>
    /// <param name="tomlFile">toml文件</param>
    /// <param name="options">设置</param>
    /// <returns>生成的数据</returns>
    public static string GenerateFromFile(string tomlFile, TOMLAsClassesOptions? options = null)
    {
        var toml = TOML.ParseFromFile(tomlFile);
        var rootClassName = Path.GetFileNameWithoutExtension(tomlFile);
        return Generate(toml, rootClassName, options);
    }

    /// <summary>
    /// 从文件中生成
    /// </summary>
    /// <param name="tomlFile">toml文件</param>
    /// <param name="rootClassName">基类名称</param>
    /// <param name="options">设置</param>
    /// <returns>生成的数据</returns>
    public static string GenerateFromFile(
        string tomlFile,
        string rootClassName = "",
        TOMLAsClassesOptions? options = null
    )
    {
        var toml = TOML.ParseFromFile(tomlFile);
        if (string.IsNullOrWhiteSpace(rootClassName))
            rootClassName = Path.GetFileNameWithoutExtension(tomlFile);
        return Generate(toml, rootClassName, options);
    }

    /// <summary>
    /// 从Toml表格中生成
    /// </summary>
    /// <param name="rootClassName">基类名称</param>
    /// <param name="table">表格</param>
    /// <param name="options">设置</param>
    /// <returns>生成的数据</returns>
    public static string Generate(
        TomlTable table,
        string rootClassName,
        TOMLAsClassesOptions? options = null
    )
    {
        var asClasses = new TOMLAsClasses(options);
        return asClasses.Generate(table, rootClassName);
    }
    #endregion
    #region Parse Toml
    /// <summary>
    /// 生成
    /// </summary>
    /// <param name="table">Toml数据表格</param>
    /// <param name="rootClassName">基类名称</param>
    /// <returns>生成的数据</returns>
    private string Generate(TomlTable table, string rootClassName)
    {
        // 初始化列表名称
        InitializeData();

        // 解析tbale
        ParseTable(rootClassName, null, table);

        // 生成数据
        var sb = new StringBuilder();
        foreach (var tomlClass in r_tomlClasses.Values)
            sb.AppendLine(tomlClass.ToString());
        return sb.ToString();
    }

    /// <summary>
    /// 初始化列表名称
    /// </summary>
    private void InitializeData()
    {
        r_arrayTypeNames.Add(
            r_options.TomlFloatNameConvert,
            string.Format(r_options.ListFormat, r_options.TomlFloatNameConvert)
        );
        r_arrayTypeNames.Add(
            r_options.TomlIntegerNameConvert,
            string.Format(r_options.ListFormat, r_options.TomlIntegerNameConvert)
        );
        r_arrayTypeNames.Add(
            r_options.TomlInteger64NameConvert,
            string.Format(r_options.ListFormat, r_options.TomlInteger64NameConvert)
        );
        r_arrayTypeNames.Add(
            nameof(TomlNode),
            string.Format(r_options.ListFormat, nameof(TomlNode))
        );

        if (r_options.AddITomlClassCommentInterface)
            r_options.MultipleInheritance.Add(r_options.ITomlClassCommentInterface);

        // 统一特性的格式
        if (r_options.ClassAttributes.Any())
            r_options.ClassAttributes = r_options.ClassAttributes
                .Select(s => RemoveSurroundedSquareBrackets(s))
                .ToHashSet();
        if (r_options.PropertyAttributes.Any())
            r_options.PropertyAttributes = r_options.PropertyAttributes
                .Select(s => RemoveSurroundedSquareBrackets(s))
                .ToHashSet();
    }

    /// <summary>
    /// 删除首尾的方括号
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>去除掉首尾的方括号的字符串</returns>
    private static string RemoveSurroundedSquareBrackets(string s)
    {
        if (s[0] is '[' && s[^1] is ']')
            return s[1..^2];
        return s;
    }

    /// <summary>
    /// 解析表格
    /// </summary>
    /// <param name="className">类名称</param>
    /// <param name="parentClassName">父类名称</param>
    /// <param name="table">表格</param>
    /// <exception cref="Exceptions">toml中使用的Csharp的关键字</exception>
    private void ParseTable(string className, string? parentClassName, TomlTable table)
    {
        var isAnonymousClass =
            parentClassName is not null && string.IsNullOrWhiteSpace(parentClassName);
        var tomlClass = GetTomlClass(className, parentClassName, table);
        var index = 0;
        foreach (var kv in table)
        {
            var name = kv.Key;
            var node = kv.Value;
            if (r_options.KeyNameConverterFunc is not null)
                name = r_options.KeyNameConverterFunc(name);
            else if (r_options.KeyNameToPascal)
                name = name.ToPascal(r_options.KeyWordSeparator, r_options.RemoveKeyWordSeparator);

            // 检测关键词
            if (TOMLUtils.CsharpKeywords.Contains(name))
                throw new Exception($"Used CsharpKeywords \"{name}\" in \"{className}\"");
            // 解析表格的值
            ParseTableValue(tomlClass, name, node);
            ChackOptions(isAnonymousClass, name, kv.Key, ref index, node, tomlClass);
        }
    }

    /// <summary>
    /// 检查设置
    /// </summary>
    /// <param name="isAnonymousClass">是匿名函数</param>
    /// <param name="name">名称</param>
    /// <param name="originalName">原始名称</param>
    /// <param name="index">标识</param>
    /// <param name="node">Toml节点</param>
    /// <param name="tomlClass">Toml类</param>
    private void ChackOptions(
        bool isAnonymousClass,
        string name,
        string originalName,
        ref int index,
        TomlNode node,
        TOMLClass tomlClass
    )
    {
        if (r_options.PropertyAttributes is not null)
            foreach (var attribute in r_options.PropertyAttributes)
                tomlClass.Values[name].Attributes.Add(attribute);
        if (r_options.AddTomlRequiredAttribute)
            tomlClass.Values[name].Attributes.Add(r_options.TomlRequiredAttribute);
        if (r_options.AddTomlPropertyOrderAttribute)
            tomlClass.Values[name].Attributes.Add(
                string.Format(r_options.TomlPropertyOrderAttributeFormat, index++)
            );
        if (r_options.AddTomlPropertyNameAttribute)
            tomlClass.Values[name].Attributes.Add(
                string.Format(r_options.TomlPropertyNameAttributeFormat, originalName)
            );
        if (isAnonymousClass)
            return;
        if (r_options.AddComment)
            tomlClass.Values[name].Comment = node.Comment;
    }

    /// <summary>
    /// 获取Toml类
    /// </summary>
    /// <param name="className">类名</param>
    /// <param name="parentClassName">父类名</param>
    /// <param name="table">Toml表格</param>
    /// <returns>Toml类</returns>
    /// <exception cref="Exception">使用了Csharp的内部字符</exception>
    private TOMLClass GetTomlClass(string className, string? parentClassName, TomlTable table)
    {
        // 检测关键字
        if (TOMLUtils.CsharpKeywords.Contains(className))
            throw new Exception($"Used CsharpKeywords \"{className}\"");
        // 获取已存在的类
        if (r_tomlClasses.TryGetValue(className, out var tomlClass) is false)
        {
            tomlClass = new(this, className, parentClassName);
            if (r_options.AddComment)
                tomlClass.Comment = table.Comment;
            r_tomlClasses.TryAdd(tomlClass.FullName, tomlClass);
        }
        return tomlClass;
    }

    /// <summary>
    /// 解析表格的值
    /// </summary>
    /// <param name="tomlClass">toml类</param>
    /// <param name="name">值名称</param>
    /// <param name="node">值数据</param>
    private void ParseTableValue(TOMLClass tomlClass, string name, TomlNode node)
    {
        if (node.IsTomlTable)
        {
            // 获取类名称
            var className = string.Format(r_options.ClassNameFormat, name);
            // 判断是否有父类,有则为父类添加新的属性,没有则新建
            if (string.IsNullOrWhiteSpace(tomlClass.ParentName))
                tomlClass.Values.TryAdd(name, new(this, name, className));
            else
                r_tomlClasses[tomlClass.FullName].Values.TryAdd(name, new(this, name, className));
            // 解析类
            ParseTable(className, tomlClass.Name, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            // 获取数组类名称
            var arrayTypeName = ParseArray(name, node.AsTomlArray);
            tomlClass.Values.TryAdd(name, new(this, name, arrayTypeName));
        }
        else
        {
            tomlClass.Values.TryAdd(name, new(this, name, node));
        }
    }

    /// <summary>
    /// 解析数组
    /// </summary>
    /// <param name="name">值名称</param>
    /// <param name="array">数组值</param>
    /// <returns>数值类型名</returns>
    private string ParseArray(string name, TomlArray array)
    {
        // 如果数组中没有值(无法判断值类型),则设置为TomlNode
        if (array.ChildrenCount is 0)
            return r_arrayTypeNames[nameof(TomlNode)];

        // 遍历所有值,并获取类型标识
        var tomlTypeCode = TomlType.GetTypeCode(array[0]);
        foreach (var node in array)
            tomlTypeCode |= TomlType.GetTypeCode(node);

        // 解析类型标识
        var typeName = string.Empty;
        if (tomlTypeCode is TomlTypeCode.Array)
        {
            typeName = ParseArrayValue(name, array);
        }
        else if (tomlTypeCode is TomlTypeCode.Table)
        {
            typeName = ParseTableInArrayValue(name, array);
            // 匿名类不需要缓存
            return string.Format(r_options.ListFormat, typeName);
        }
        else
            typeName = r_options.GetConvertName(
                array.FirstOrDefault(n => n.AsTomlInteger?.IsInteger64 is true, array[0]),
                MergeTomlTypeCode(tomlTypeCode)
            );

        // 将数组名缓存
        r_arrayTypeNames.TryAdd(typeName, string.Format(r_options.ListFormat, typeName));
        return r_arrayTypeNames[typeName];
    }

    /// <summary>
    /// 合并类型标识
    /// </summary>
    /// <param name="tomlTypeCode">Toml类代码</param>
    /// <returns>合并后的Toml类代码</returns>
    private TomlTypeCode MergeTomlTypeCode(TomlTypeCode tomlTypeCode)
    {
        // 如果同时存在int和float类型,则会被转换成float
        if (
            r_options.MergeIntegerAndFloat
            && tomlTypeCode is (TomlTypeCode.Integer | TomlTypeCode.Float)
        )
            return TomlTypeCode.Float;
        else
            return tomlTypeCode;
    }

    /// <summary>
    /// 解析数组的值(用于多维数组)
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="array">数组</param>
    /// <returns>数组类名</returns>
    private string ParseArrayValue(string name, TomlArray array)
    {
        var typeNames = new HashSet<string>();
        // 遍历数组并获取值
        foreach (var node in array)
            typeNames.Add(ParseArray(name, node.AsTomlArray));

        return ParseTypeNameSet(typeNames);
    }

    /// <summary>
    /// 解析类型名称集合
    /// </summary>
    /// <param name="typeNames">类型名称集合</param>
    /// <returns>解析完成的类型名称</returns>
    private string ParseTypeNameSet(HashSet<string> typeNames)
    {
        // 如果为同一种值
        if (typeNames.Count is 1)
            return typeNames.First();
        else if (
            typeNames.Count is 2
            && typeNames.Contains(r_arrayTypeNames[r_options.TomlIntegerNameConvert])
            && typeNames.Contains(r_arrayTypeNames[r_options.TomlInteger64NameConvert])
        )
        {
            // 如果同时为int和int64,则变为int64
            return r_arrayTypeNames[r_options.TomlInteger64NameConvert];
        }
        else if (
            r_options.MergeIntegerAndFloat
            && typeNames.Count is 2
            && (
                typeNames.Contains(r_arrayTypeNames[r_options.TomlIntegerNameConvert])
                || typeNames.Contains(r_arrayTypeNames[r_options.TomlInteger64NameConvert])
            )
            && typeNames.Contains(r_arrayTypeNames[r_options.TomlFloatNameConvert])
        )
        {
            // 如果同时为int或int64和float则返回float
            return r_arrayTypeNames[r_options.TomlFloatNameConvert];
        }
        else if (
            r_options.MergeIntegerAndFloat
            && typeNames.Count is 3
            && typeNames.Contains(r_arrayTypeNames[r_options.TomlIntegerNameConvert])
            && typeNames.Contains(r_arrayTypeNames[r_options.TomlInteger64NameConvert])
            && typeNames.Contains(r_arrayTypeNames[r_options.TomlFloatNameConvert])
        )
        {
            // 如果同时为int和int64和float则返回float
            return r_arrayTypeNames[r_options.TomlFloatNameConvert];
        }
        else
            // 否则返回TomlNode
            return r_arrayTypeNames[nameof(TomlNode)];
    }

    /// <summary>
    /// 解析数组中的表格
    /// </summary>
    /// <param name="name">数组名</param>
    /// <param name="array">数组值</param>
    /// <returns>匿名类名称</returns>
    private string ParseTableInArrayValue(string name, TomlArray array)
    {
        // 获取匿名类名称
        var anonymousClassName = string.Format(r_options.AnonymousClassNameFormat, name);
        foreach (var item in array)
        {
            var table = item.AsTomlTable;
            ParseTable(anonymousClassName, string.Empty, table);
        }
        return anonymousClassName;
    }
    #endregion
}
