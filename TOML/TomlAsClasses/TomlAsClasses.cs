using System.Diagnostics;
using System.Text;
using HKWToml.Utils;

namespace HKW.TOML.TomlAsClasses;

/// <summary>
/// Toml转换为类
/// </summary>
public partial class TomlAsClasses
{
    /// <summary>
    /// 所有类
    /// <para>(类名称, 类值)</para>
    /// </summary>
    private static readonly Dictionary<string, TomlClass> sr_tomlClasses = new();

    /// <summary>
    /// 所有数组名称
    /// <para>(数组值类型名称, 数组名称)</para>
    /// </summary>
    private static readonly Dictionary<string, string> sr_arrayTypeNames = new();

    /// <summary>
    /// 所有单词分隔符
    /// </summary>
    private static char[] s_keyWordSeparators = null!;

    /// <summary>
    /// 设置
    /// </summary>
    private static TomlAsClassesOptions s_options = new();

    private TomlAsClasses() { }

    /// <summary>
    /// 从文件中生成
    /// </summary>
    /// <param name="tomlFile">toml文件</param>
    /// <param name="options">设置</param>
    /// <returns>生成的数据</returns>
    public static string GenerateFromFile(string tomlFile, TomlAsClassesOptions? options = null)
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
        TomlAsClassesOptions? options = null
    )
    {
        var toml = TOML.ParseFromFile(tomlFile);
        if (string.IsNullOrWhiteSpace(rootClassName))
            rootClassName = Path.GetFileNameWithoutExtension(tomlFile);
        return Generate(toml, rootClassName, options);
    }

    /// <summary>
    /// 从toml表格中生成
    /// </summary>
    /// <param name="rootClassName">基类名称</param>
    /// <param name="table">表格</param>
    /// <param name="options">设置</param>
    /// <returns>生成的数据</returns>
    public static string Generate(
        TomlTable table,
        string rootClassName,
        TomlAsClassesOptions? options = null
    )
    {
        // 获取设置
        s_options = options ?? new();
        // 初始化列表名称
        InitializeData();

        // 解析tbale
        ParseTable(rootClassName, null, table);

        // 生成数据
        var sb = new StringBuilder();
        foreach (var tomlClass in sr_tomlClasses.Values)
            sb.AppendLine(tomlClass.ToString());

        // 清空数据
        sr_tomlClasses.Clear();
        sr_arrayTypeNames.Clear();
        s_options = null!;
        return sb.ToString();
    }

    /// <summary>
    /// 初始化列表名称
    /// </summary>
    private static void InitializeData()
    {
        s_keyWordSeparators = s_options.KeyWordSeparators.ToArray();

        sr_arrayTypeNames.Add(
            s_options.TomlFloatNameConvert,
            string.Format(s_options.ListFormat, s_options.TomlFloatNameConvert)
        );
        sr_arrayTypeNames.Add(
            s_options.TomlIntegerNameConvert,
            string.Format(s_options.ListFormat, s_options.TomlIntegerNameConvert)
        );
        sr_arrayTypeNames.Add(
            s_options.TomlInteger64NameConvert,
            string.Format(s_options.ListFormat, s_options.TomlInteger64NameConvert)
        );
        sr_arrayTypeNames.Add(
            nameof(TomlNode),
            string.Format(s_options.ListFormat, nameof(TomlNode))
        );

        if (s_options.AddITomlClassCommentInterface)
            s_options.MultipleInheritance.Add(s_options.ITomlClassCommentInterface);

        // 统一特性的格式
        if (s_options.ClassAttributes.Any())
            s_options.ClassAttributes = s_options.ClassAttributes.Select(s => RemoveSurroundedSquareBrackets(s)).ToHashSet();
        if (s_options.PropertyAttributes.Any())
            s_options.PropertyAttributes = s_options.PropertyAttributes.Select(s => RemoveSurroundedSquareBrackets(s)).ToHashSet();
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
    /// <exception cref="TomlException">toml中使用的Csharp的关键字</exception>
    private static void ParseTable(string className, string? parentClassName, TomlTable table)
    {
        var isAnonymousClass =
            parentClassName is not null && string.IsNullOrWhiteSpace(parentClassName);
        var tomlClass = GetTomlClass(className, parentClassName, table);
        var index = 0;
        foreach (var kv in table)
        {
            var name = kv.Key;
            var node = kv.Value;
            if (s_options.KeyNameConverterFunc is not null)
                name = s_options.KeyNameConverterFunc(name);
            else if (s_options.KeyNameToPascal)
                name = Utils.ToPascal(name, s_keyWordSeparators, s_options.RemoveKeyWordSeparators);

            // 检测关键词
            if (s_csharpKeywords.Contains(name))
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
    private static void ChackOptions(
        bool isAnonymousClass,
        string name,
        string originalName,
        ref int index,
        TomlNode node,
        TomlClass tomlClass
    )
    {
        if (s_options.PropertyAttributes is not null)
            foreach (var attribute in s_options.PropertyAttributes)
                tomlClass.Values[name].Attributes.Add(attribute);
        if (s_options.AddTomlRequiredAttribute)
            tomlClass.Values[name].Attributes.Add(s_options.TomlRequiredAttribute);
        if (s_options.AddTomlPropertyOrderAttribute)
            tomlClass.Values[name].Attributes.Add(
                string.Format(s_options.TomlPropertyOrderAttributeFormat, index++)
            );
        if (s_options.AddTomlPropertyNameAttribute)
            tomlClass.Values[name].Attributes.Add(
                string.Format(s_options.TomlPropertyNameAttributeFormat, originalName)
            );
        if (isAnonymousClass)
            return;
        if (s_options.AddComment)
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
    private static TomlClass GetTomlClass(
        string className,
        string? parentClassName,
        TomlTable table
    )
    {
        // 检测关键字
        if (s_csharpKeywords.Contains(className))
            throw new Exception($"Used CsharpKeywords \"{className}\"");
        // 获取已存在的类
        if (sr_tomlClasses.TryGetValue(className, out var tomlClass) is false)
        {
            tomlClass = new(className, parentClassName);
            if (s_options.AddComment)
                tomlClass.Comment = table.Comment;
            sr_tomlClasses.TryAdd(tomlClass.FullName, tomlClass);
        }
        return tomlClass;
    }

    /// <summary>
    /// 解析表格的值
    /// </summary>
    /// <param name="tomlClass">toml类</param>
    /// <param name="name">值名称</param>
    /// <param name="node">值数据</param>
    private static void ParseTableValue(TomlClass tomlClass, string name, TomlNode node)
    {
        if (node.IsTomlTable)
        {
            // 获取类名称
            var className = string.Format(s_options.ClassNameFormat, name);
            // 判断是否有父类,有则为父类添加新的属性,没有则新建
            if (string.IsNullOrWhiteSpace(tomlClass.ParentName))
                tomlClass.Values.TryAdd(name, new(name, className));
            else
                sr_tomlClasses[tomlClass.FullName].Values.TryAdd(name, new(name, className));
            // 解析类
            ParseTable(className, tomlClass.Name, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            // 获取数组类名称
            var arrayTypeName = ParseArray(name, node.AsTomlArray);
            tomlClass.Values.TryAdd(name, new(name, arrayTypeName));
        }
        else
        {
            tomlClass.Values.TryAdd(name, new(name, node));
        }
    }

    /// <summary>
    /// 解析数值
    /// </summary>
    /// <param name="name">值名称</param>
    /// <param name="array">数组值</param>
    /// <returns>数值类型名</returns>
    private static string ParseArray(string name, TomlArray array)
    {
        // 如果数值中没有值(无法判断值类型),则设置为TomlNode
        if (array.ChildrenCount is 0)
            return sr_arrayTypeNames[nameof(TomlNode)];

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
            return string.Format(s_options.ListFormat, typeName);
        }
        else
            typeName = s_options.GetConvertName(
                array.FirstOrDefault(n => n.AsTomlInteger?.IsInteger64 is true, array[0]),
                MergeTomlTypeCode(tomlTypeCode)
            );

        // 将数组名缓存
        sr_arrayTypeNames.TryAdd(typeName, string.Format(s_options.ListFormat, typeName));
        return sr_arrayTypeNames[typeName];
    }

    /// <summary>
    /// 合并类型标识
    /// </summary>
    /// <param name="tomlTypeCode"></param>
    /// <returns></returns>
    private static TomlTypeCode MergeTomlTypeCode(TomlTypeCode tomlTypeCode)
    {
        // 如果同时存在int和float类型,则会被转换成float
        if (
            s_options.MergeIntegerAndFloat
            && tomlTypeCode is (TomlTypeCode.Integer | TomlTypeCode.Float)
        )
            return TomlTypeCode.Float;
        else
            return tomlTypeCode;
    }

    /// <summary>
    /// 解析数组的值(用于多维数组)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="array"></param>
    /// <returns></returns>
    private static string ParseArrayValue(string name, TomlArray array)
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
    private static string ParseTypeNameSet(HashSet<string> typeNames)
    {
        // 如果为同一种值
        if (typeNames.Count is 1)
            return typeNames.First();
        else if (
            typeNames.Count is 2
            && typeNames.Contains(sr_arrayTypeNames[s_options.TomlIntegerNameConvert])
            && typeNames.Contains(sr_arrayTypeNames[s_options.TomlInteger64NameConvert])
        )
        {
            // 如果同时为int和int64,则变为int64
            return sr_arrayTypeNames[s_options.TomlInteger64NameConvert];
        }
        else if (
            s_options.MergeIntegerAndFloat
            && typeNames.Count is 2
            && (
                typeNames.Contains(sr_arrayTypeNames[s_options.TomlIntegerNameConvert])
                || typeNames.Contains(sr_arrayTypeNames[s_options.TomlInteger64NameConvert])
            )
            && typeNames.Contains(sr_arrayTypeNames[s_options.TomlFloatNameConvert])
        )
        {
            // 如果同时为int或int64和float则返回float
            return sr_arrayTypeNames[s_options.TomlFloatNameConvert];
        }
        else if (
            s_options.MergeIntegerAndFloat
            && typeNames.Count is 3
            && typeNames.Contains(sr_arrayTypeNames[s_options.TomlIntegerNameConvert])
            && typeNames.Contains(sr_arrayTypeNames[s_options.TomlInteger64NameConvert])
            && typeNames.Contains(sr_arrayTypeNames[s_options.TomlFloatNameConvert])
        )
        {
            // 如果同时为int和int64和float则返回float
            return sr_arrayTypeNames[s_options.TomlFloatNameConvert];
        }
        else
            // 否则返回TomlNode
            return sr_arrayTypeNames[nameof(TomlNode)];
    }

    /// <summary>
    /// 解析数组中的表格
    /// </summary>
    /// <param name="name">数组名</param>
    /// <param name="array">数组值</param>
    /// <returns>匿名类名称</returns>
    private static string ParseTableInArrayValue(string name, TomlArray array)
    {
        // 获取匿名类名称
        var anonymousClassName = string.Format(s_options.AnonymousClassNameFormat, name);
        foreach (var item in array)
        {
            var table = item.AsTomlTable;
            ParseTable(anonymousClassName, string.Empty, table);
        }
        return anonymousClassName;
    }

    /// <summary>
    /// csharp关键字集合
    /// </summary>
    private static readonly HashSet<string> s_csharpKeywords =
        new()
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while"
        };
}
