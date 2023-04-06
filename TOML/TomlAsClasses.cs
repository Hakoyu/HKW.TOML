using System.Diagnostics;
using System.Text;

namespace HKW.TOML;

/// <summary>
/// Toml转换为类
/// </summary>
public partial class TomlAsClasses
{
    /// <summary>
    /// 匿名类数量
    /// </summary>
    private static int s_anonymousTableCount = 0;

    /// <summary>
    /// 所有类
    /// <para>(类名称, 类值)</para>
    /// </summary>
    private static readonly Dictionary<string, TomlClass> s_tomlClasses = new();

    /// <summary>
    /// 所有数组名称
    /// <para>(数组值类型名称, 数组名称)</para>
    /// </summary>
    private static readonly Dictionary<string, string> s_arrayTypeNames = new();

    /// <summary>
    /// 设置
    /// </summary>
    private static TomlAsClassesOptions s_options = new();

    private TomlAsClasses()
    { }

    /// <summary>
    /// 从文件中构造
    /// </summary>
    /// <param name="tomlFile">toml文件</param>
    /// <param name="options">设置</param>
    /// <returns>构造的数据</returns>
    public static string ConstructFromFile(string tomlFile, TomlAsClassesOptions? options = null)
    {
        var toml = TOML.ParseFromFile(tomlFile);
        var rootClassName = Path.GetFileNameWithoutExtension(tomlFile);
        return Construct(rootClassName, toml, options);
    }

    /// <summary>
    /// 从文件中构造
    /// </summary>
    /// <param name="tomlFile">toml文件</param>
    /// <param name="rootClassName">基类名称</param>
    /// <param name="options">设置</param>
    /// <returns>构造的数据</returns>
    public static string ConstructFromFile(
        string tomlFile,
        string rootClassName = "",
        TomlAsClassesOptions? options = null
    )
    {
        var toml = TOML.ParseFromFile(tomlFile);
        if (string.IsNullOrWhiteSpace(rootClassName))
            rootClassName = Path.GetFileNameWithoutExtension(tomlFile);
        return Construct(rootClassName, toml, options);
    }

    /// <summary>
    /// 从toml表格中构造
    /// </summary>
    /// <param name="rootClassName">基类名称</param>
    /// <param name="table">表格</param>
    /// <param name="options">设置</param>
    /// <returns>构造的数据</returns>
    public static string Construct(
        string rootClassName,
        TomlTable table,
        TomlAsClassesOptions? options = null
    )
    {
        // 获取设置
        s_options = options ?? new();
        // 初始化列表名称
        InitializeData();

        // 解析tbale
        ParseTable(rootClassName, string.Empty, table, false);

        // 生成数据
        var sb = new StringBuilder();
        foreach (var tomlClass in s_tomlClasses.Values)
            sb.AppendLine(tomlClass.ToString());

        // 清空数据
        s_tomlClasses.Clear();
        s_arrayTypeNames.Clear();
        s_anonymousTableCount = 0;
        s_options = null!;
        return sb.ToString();
    }

    /// <summary>
    /// 初始化列表名称
    /// </summary>
    private static void InitializeData()
    {
        s_arrayTypeNames.Add(
            s_options.TomlFloatNameConvert,
            string.Format(s_options.ListFormat, s_options.TomlFloatNameConvert)
        );
        s_arrayTypeNames.Add(
            s_options.TomlIntegerNameConvert,
            string.Format(s_options.ListFormat, s_options.TomlIntegerNameConvert)
        );
        s_arrayTypeNames.Add(
            s_options.TomlInteger64NameConvert,
            string.Format(s_options.ListFormat, s_options.TomlInteger64NameConvert)
        );
        s_arrayTypeNames.Add(
            nameof(TomlNode),
            string.Format(s_options.ListFormat, nameof(TomlNode))
        );

        if (s_options.AddITomlClassInterface)
            s_options.MultipleInheritance.Add(s_options.ITomlClassInterface);
    }

    /// <summary>
    /// 解析表格
    /// </summary>
    /// <param name="className">类名称</param>
    /// <param name="parentClassName">父类名称</param>
    /// <param name="table">表格</param>
    /// <param name="isAnonymousClass">是匿名函数</param>
    /// <exception cref="Exception">toml中使用的Csharp的关键字</exception>
    private static void ParseTable(
        string className,
        string parentClassName,
        TomlTable table,
        bool isAnonymousClass
    )
    {
        var tomlClass = GetTomlClass(className, parentClassName, table, isAnonymousClass);

        var index = 0;
        foreach (var kv in table)
        {
            var name = kv.Key;
            var node = kv.Value;
            if (s_options.KeyNameConverterFunc is not null)
                name = s_options.KeyNameConverterFunc(name);
            else if (s_options.KeyNameToPascal)
                name = ToPascal(name);

            // 检测关键词
            if (s_csharpKeywords.Contains(name))
                throw new Exception($"Used CsharpKeywords \"{name}\" in \"{className}\"");
            // 解析表格的值
            ParseTableValue(tomlClass, name, node);
            if (s_options.AddComment)
                tomlClass.Values[name].Comment = node.Comment;
            if (s_options.AddTomlSortOrderAttribute)
                tomlClass.Values[name].Attributes.Add(string.Format(s_options.TomlSortOrderAttributeFomat, index++));
        }
    }

    private static TomlClass GetTomlClass(
        string className,
        string parentClassName,
        TomlTable table,
        bool isAnonymousClass
    )
    {
        // 检测关键字
        if (s_csharpKeywords.Contains(className))
            throw new Exception($"Used CsharpKeywords \"{className}\"");
        // 获取已存在的类
        if (s_tomlClasses.TryGetValue(className, out var tomlClass) is false)
        {
            tomlClass = new(className, parentClassName, isAnonymousClass);
            if (s_options.AddComment)
                tomlClass.Comment = table.Comment;
            s_tomlClasses.TryAdd(tomlClass.FullName, tomlClass);
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
                s_tomlClasses[tomlClass.FullName].Values.TryAdd(name, new(name, className));
            // 解析类
            ParseTable(className, tomlClass.Name, node.AsTomlTable, false);
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
            return s_arrayTypeNames[nameof(TomlNode)];

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
        s_arrayTypeNames.TryAdd(typeName, string.Format(s_options.ListFormat, typeName));
        return s_arrayTypeNames[typeName];
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
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlIntegerNameConvert])
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlInteger64NameConvert])
        )
        {
            // 如果同时为int和int64,则变为int64
            return s_arrayTypeNames[s_options.TomlInteger64NameConvert];
        }
        else if (
            s_options.MergeIntegerAndFloat
            && typeNames.Count is 2
            && (
                typeNames.Contains(s_arrayTypeNames[s_options.TomlIntegerNameConvert])
                || typeNames.Contains(s_arrayTypeNames[s_options.TomlInteger64NameConvert])
            )
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlFloatNameConvert])
        )
        {
            // 如果同时为int或int64和float则返回float
            return s_arrayTypeNames[s_options.TomlFloatNameConvert];
        }
        else if (
            s_options.MergeIntegerAndFloat
            && typeNames.Count is 3
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlIntegerNameConvert])
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlInteger64NameConvert])
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlFloatNameConvert])
        )
        {
            // 如果同时为int和int64和float则返回float
            return s_arrayTypeNames[s_options.TomlFloatNameConvert];
        }
        else
            // 否则返回TomlNode
            return s_arrayTypeNames[nameof(TomlNode)];
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
        var anonymousClassName = string.Format(
            s_options.AnonymousClassNameFormat,
            name,
            s_anonymousTableCount++
        );
        foreach (var item in array)
        {
            var table = item.AsTomlTable;
            ParseTable(anonymousClassName, string.Empty, table, true);
        }
        return anonymousClassName;
    }

    /// <summary>
    /// 将字符串转换为帕斯卡格式
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>帕斯卡格式字符串</returns>
    private static string ToPascal(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;
        // 使用分隔符拆分单词
        var strs = str.Split(s_options.KeyWordSeparator);
        // 将单词首字母大写
        var newStrs = strs.Select(s => FirstLetterToUpper(s));
        // 是否保留分隔符
        if (s_options.RemoveKeyWordSeparator)
            return string.Join("", newStrs);
        else
            return string.Join(s_options.KeyWordSeparator, newStrs);
    }

    /// <summary>
    /// 将字符串首字母大写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>第一个为大写的字符串</returns>
    private static string FirstLetterToUpper(string str) => $"{char.ToUpper(str[0])}{str[1..]}";

    /// <summary>
    /// Toml构造类
    /// </summary>
    [DebuggerDisplay("{Name},Count = {Count}")]
    private class TomlClass
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  全名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 父类名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 是匿名类
        /// </summary>
        public bool IsAnonymous { get; set; } = false;

        /// <summary>
        /// 值字典
        /// <para>(值名称, 值)</para>
        /// </summary>
        public Dictionary<string, TomlClassValue> Values { get; set; } = new();

        /// <summary>
        /// 特性
        /// </summary>
        public HashSet<string> Attributes { get; set; } = new();

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="parentName">父类名称</param>
        /// <param name="isAnonymous">是匿名函数</param>
        public TomlClass(string name, string parentName = "", bool isAnonymous = false)
        {
            Name = name;
            FullName = name + parentName;
            ParentName = parentName;
            IsAnonymous = isAnonymous;
        }

        /// <summary>
        /// 转化为格式化字符串
        /// </summary>
        /// <returns>格式化字符串</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            var classname = Name;
            // 为匿名函数时,不设置注释,特性,继承
            if (IsAnonymous is false)
            {
                if (GetComment(Comment) is string comment)
                    sb.AppendLine(comment);
                if (GetAttribute(s_options.ClassAttributes) is string attribute)
                    sb.AppendLine(attribute);
                Name += GetInheritance(s_options.MultipleInheritance);
            }

            return string.Format(s_options.ClassFormat, sb.ToString(), classname, GetValues(Values.Values));
        }

        /// <summary>
        /// 获取注释
        /// </summary>
        /// <param name="comment">注释</param>
        /// <returns>格式化的注释</returns>
        private static string GetComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return comment;
            var comments = comment.Split(
                new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries
            );
            if (comments.Length is 1)
                return string.Format(s_options.CommentFormat, string.Empty, comments[0]);
            var multiLineComment =
               comments[0]
               + "\n"
               + string.Join(
                   "\n",
                   comments[1..].Select(
                       s => string.Format(s_options.CommentParaFormat, string.Empty, s)
                   )
               );
            return string.Format(s_options.CommentFormat, string.Empty, multiLineComment);
        }

        /// <summary>
        /// 获取特性数据
        /// </summary>
        /// <param name="attributes">特性</param>
        /// <returns>格式化的特性数据</returns>
        private static string GetAttribute(IEnumerable<string> attributes)
        {
            var sb = new StringBuilder();
            foreach (var attribute in attributes)
                sb.AppendLine(string.Format(s_options.AttributeFomat, string.Empty, attribute));
            return sb.ToString();
        }

        /// <summary>
        /// 获取继承数据
        /// </summary>
        /// <param name="inheritances">继承</param>
        /// <returns>格式化的继承数据</returns>
        private static string GetInheritance(IEnumerable<string> inheritances)
        {
            var str = string.Join(", ", inheritances);
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            return string.Format(s_options.InheritanceFormat, str);
        }

        /// <summary>
        /// 获取值数据
        /// </summary>
        /// <param name="values">值</param>
        /// <returns>格式化的值数据</returns>
        private static string GetValues(IEnumerable<TomlClassValue> values)
        {
            var sb = new StringBuilder();
            foreach (var item in values)
                sb.AppendLine(item.ToString());
            return sb.ToString();
        }
    }

    /// <summary>
    /// toml类值
    /// </summary>
    [DebuggerDisplay("{TypeName}, {Name}")]
    private class TomlClassValue
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 特性
        /// </summary>
        public HashSet<string> Attributes { get; set; } = new();

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="typeName">类型名称</param>
        public TomlClassValue(string name, string typeName)
        {
            Name = name;
            TypeName = typeName;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="node">类值(推断类型名称)</param>
        public TomlClassValue(string name, TomlNode node)
        {
            Name = name;
            TypeName = s_options.GetConvertName(node, TomlType.GetTypeCode(node));
        }

        /// <summary>
        /// 转化为格式化字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            var valueData = string.Format(s_options.ValueFormat, s_options.Indent, TypeName, Name);
            return GetComment(Comment) + GetAttribute(Attributes) + valueData;
        }

        /// <summary>
        /// 获取注释
        /// </summary>
        /// <param name="comment">注释</param>
        /// <returns>格式化的注释</returns>
        private static string GetComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return comment;
            var comments = comment.Split(
                new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries
            );
            if (comments.Length is 1)
                return string.Format(s_options.CommentFormat, s_options.Indent, comments[0]);
            var multiLineComment =
               comments[0]
               + "\n"
               + string.Join(
                   "\n",
                   comments[1..].Select(
                       s => string.Format(s_options.CommentParaFormat, s_options.Indent, s)
                   )
               );
            return string.Format(s_options.CommentFormat, s_options.Indent, multiLineComment);
        }

        /// <summary>
        /// 获取特性数据
        /// </summary>
        /// <param name="attributes">特性</param>
        /// <returns>格式化的特性数据</returns>
        private static string GetAttribute(IEnumerable<string> attributes)
        {
            var sb = new StringBuilder();
            foreach (var attribute in attributes)
                sb.AppendLine(string.Format(s_options.AttributeFomat, s_options.Indent, attribute));
            return sb.ToString();
        }
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