using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HKW.Libs.TOML;

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

    private TomlAsClasses() { }

    /// <summary>
    /// 从文件中构造
    /// </summary>
    /// <param name="tomlFile">toml文件</param>
    /// <param name="options">设置</param>
    /// <returns>构造的数据</returns>
    public static string ConstructFromFile(string tomlFile, TomlAsClassesOptions? options = null)
    {
        var toml = TOML.Parse(tomlFile);
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
        var toml = TOML.Parse(tomlFile);
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
        InitializeArrayTypeNames();

        // 解析tbale
        ParseTable(rootClassName, string.Empty, table);

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
    private static void InitializeArrayTypeNames()
    {
        s_arrayTypeNames.Add(
            s_options.TomlFloatConvertName,
            string.Format(s_options.ListFormat, s_options.TomlFloatConvertName)
        );
        s_arrayTypeNames.Add(
            s_options.TomlIntegerConvertName,
            string.Format(s_options.ListFormat, s_options.TomlIntegerConvertName)
        );
        s_arrayTypeNames.Add(
            s_options.TomlInteger64ConvertName,
            string.Format(s_options.ListFormat, s_options.TomlInteger64ConvertName)
        );
        s_arrayTypeNames.Add(
            nameof(TomlNode),
            string.Format(s_options.ListFormat, nameof(TomlNode))
        );
    }

    /// <summary>
    /// 解析表格
    /// </summary>
    /// <param name="className">类名称</param>
    /// <param name="parentClassName">父类名称</param>
    /// <param name="table">表格</param>
    /// <exception cref="Exception">toml中使用的Csharp的关键字</exception>
    private static void ParseTable(
        string className,
        string parentClassName,
        TomlTable table,
        bool isAnonymousClass = false
    )
    {
        var tomlClass = GetTomlClass(className, parentClassName, table, isAnonymousClass);

        foreach (var kv in table)
        {
            var name = kv.Key;
            var node = kv.Value;
            if (s_options.KeyNameToPascal)
                name = ToPascal(name);

            // 检测关键词
            if (s_csharpKeywords.Contains(name))
                throw new Exception($"Used CsharpKeywords \"{name}\" in \"{className}\"");
            // 解析表格的值
            ParseTableValue(tomlClass, name, node);
            if (s_options.AddComment)
                tomlClass[name].Comment = node.Comment;
        }
    }

    private static TomlClass GetTomlClass(
        string className,
        string parentClassName,
        TomlTable table,
        bool isAnonymousClass = false
    )
    {
        // 检测关键字
        if (s_csharpKeywords.Contains(className))
            throw new Exception($"Used CsharpKeywords \"{className}\"");
        // 获取已存在的类
        if (s_tomlClasses.TryGetValue(className, out var tomlClass) is false)
        {
            tomlClass = new(className, parentClassName);
            if (s_options.AddComment)
                tomlClass.Comment = table.Comment;
            s_tomlClasses.TryAdd(tomlClass.FullName, tomlClass);
            tomlClass.AddInterfaces(s_options.Interfaces);
            if (isAnonymousClass is false && s_options.AddITomlClassInterface)
                tomlClass.AddInterface(s_options.ITomlClassInterface);
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
                tomlClass.Add(name, new(name, className));
            else
                s_tomlClasses[tomlClass.FullName].Add(name, new(name, className));
            // 解析类
            ParseTable(className, tomlClass.Name, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            // 获取数组类名称
            var arrayTypeName = ParseArray(name, node.AsTomlArray);
            tomlClass.TryAdd(name, new(name, arrayTypeName));
        }
        else
        {
            tomlClass.TryAdd(name, new(name, node));
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
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlIntegerConvertName])
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlInteger64ConvertName])
        )
        {
            // 如果同时为int和int64,则变为int64
            return s_arrayTypeNames[s_options.TomlInteger64ConvertName];
        }
        else if (
            s_options.MergeIntegerAndFloat
            && typeNames.Count is 2
            && (
                typeNames.Contains(s_arrayTypeNames[s_options.TomlIntegerConvertName])
                || typeNames.Contains(s_arrayTypeNames[s_options.TomlInteger64ConvertName])
            )
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlFloatConvertName])
        )
        {
            // 如果同时为int或int64和float则返回float
            return s_arrayTypeNames[s_options.TomlFloatConvertName];
        }
        else if (
            s_options.MergeIntegerAndFloat
            && typeNames.Count is 3
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlIntegerConvertName])
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlInteger64ConvertName])
            && typeNames.Contains(s_arrayTypeNames[s_options.TomlFloatConvertName])
        )
        {
            // 如果同时为int和int64和float则返回float
            return s_arrayTypeNames[s_options.TomlFloatConvertName];
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
    private class TomlClass : IDictionary<string, TomlClassValue>
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
        /// 接口集合
        /// </summary>
        public HashSet<string> Interfaces { get; private set; } = new();

        /// <summary>
        /// 值字典
        /// <para>(值名称, 值)</para>
        /// </summary>
        private readonly Dictionary<string, TomlClassValue> s_tomlClassValues = new();

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="parentName">父类名称</param>
        public TomlClass(string name, string parentName = "")
        {
            Name = name;
            FullName = name + parentName;
            ParentName = parentName;
        }

        /// <summary>
        /// 添加接口
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
        public bool AddInterface(string interfaceName) => Interfaces.Add(interfaceName);

        /// <summary>
        /// 添加多个接口
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
        public void AddInterfaces(IEnumerable<string> interfaceNames)
        {
            foreach (string interfaceName in interfaceNames)
                Interfaces.Add(interfaceName);
        }

        /// <summary>
        /// 转化为格式化字符串
        /// </summary>
        /// <returns>格式化字符串</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            // 拼接接口
            string? nameAndInterfaces;
            if (Interfaces.Any())
            {
                if (Interfaces.Contains(s_options.ITomlClassInterface))
                    sb.AppendLine(
                        string.Format(s_options.ITomlClassInterfaceValueFomat, s_options.Indent)
                    );
                nameAndInterfaces =
                    Name + string.Format(s_options.InterfaceFormat, string.Join(", ", Interfaces));
            }
            else
                nameAndInterfaces = Name;

            // 获取值数据
            foreach (var item in s_tomlClassValues.Values)
                sb.AppendLine(item.ToString());

            var classData = string.Format(s_options.ClassFormat, nameAndInterfaces, sb.ToString());

            // 设置注释
            if (string.IsNullOrWhiteSpace(Comment))
                return classData;
            else
                return string.Format(s_options.CommentFormat, string.Empty, Comment) + classData;
        }

        public TomlClassValue this[string key]
        {
            get => s_tomlClassValues[key];
            set => s_tomlClassValues[key] = value;
        }

        public ICollection<string> Keys => s_tomlClassValues.Keys;

        public ICollection<TomlClassValue> Values => s_tomlClassValues.Values;

        public int Count => s_tomlClassValues.Count;

        public bool IsReadOnly =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)s_tomlClassValues).IsReadOnly;

        public void Add(string key, TomlClassValue value) => s_tomlClassValues.Add(key, value);

        public void Add(KeyValuePair<string, TomlClassValue> item) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)s_tomlClassValues).Add(item);

        public bool TryAdd(string key, TomlClassValue value)
        {
            if (s_tomlClassValues.TryAdd(key, value) is false)
            {
                var classValue = s_tomlClassValues[key];
                if (
                    classValue.TypeName != value.TypeName && classValue.TypeName != nameof(TomlNode)
                )
                    classValue.TypeName = nameof(TomlNode);
                return false;
            }
            return true;
        }

        public void Clear() => s_tomlClassValues.Clear();

        public bool Contains(KeyValuePair<string, TomlClassValue> item) =>
            s_tomlClassValues.Contains(item);

        public bool ContainsKey(string key) => s_tomlClassValues.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, TomlClassValue>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)s_tomlClassValues).CopyTo(
                array,
                arrayIndex
            );

        public IEnumerator<KeyValuePair<string, TomlClassValue>> GetEnumerator() =>
            s_tomlClassValues.GetEnumerator();

        public bool Remove(string key) => s_tomlClassValues.Remove(key);

        public bool Remove(KeyValuePair<string, TomlClassValue> item) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)s_tomlClassValues).Remove(item);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TomlClassValue value) =>
            s_tomlClassValues.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)s_tomlClassValues).GetEnumerator();
    }

    /// <summary>
    /// toml类值
    /// </summary>
    [DebuggerDisplay("{TypeName}, {Name}")]
    public class TomlClassValue
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
            // 添加注释
            if (string.IsNullOrWhiteSpace(Comment))
                return valueData;

            var comments = Comment.Split(
                new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries
            );
            if (comments.Length is 1)
                return string.Format(s_options.CommentFormat, s_options.Indent, Comment)
                    + valueData;

            var multiLineComment =
                comments[0]
                + "\n"
                + string.Join(
                    "\n",
                    comments[1..].Select(
                        s => string.Format(s_options.CommentParaFormat, s_options.Indent, s)
                    )
                );
            return string.Format(s_options.CommentFormat, s_options.Indent, multiLineComment)
                + valueData;
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

/// <summary>
/// Toml转换为类设置
/// </summary>
public class TomlAsClassesOptions
{
    /// <summary>
    /// 将键名称转换为帕斯卡命名格式(属性命名格式)
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool KeyNameToPascal { get; set; } = true;

    /// <summary>
    /// 合并int和float
    /// (如果一个数组中同时存在int和float类型,则会被转换成float)
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool MergeIntegerAndFloat { get; set; } = true;

    /// <summary>
    /// 删除键的单词分隔符 如 "_"
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool RemoveKeyWordSeparator { get; set; } = true;

    /// <summary>
    /// 单词分隔符
    /// <para>默认为 "<see langword="_"/>"</para>
    /// </summary>
    public string KeyWordSeparator { get; set; } = "_";

    /// <summary>
    /// 添加注释
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool AddComment { get; set; } = false;

    /// <summary>
    /// 为所有非匿名类添加的接口
    /// </summary>
    public HashSet<string> Interfaces { get; set; } = new();

    /// <summary>
    /// 为所有非匿名类添加ITomlClass接口
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool AddITomlClassInterface { get; set; } = false;

    /// <summary>
    /// ITomlClass接口名称
    /// <para>默认为 <see langword="nameof(ITomlClass)"/></para>
    /// </summary>
    public string ITomlClassInterface { get; set; } = nameof(ITomlClass);

    /// <summary>
    /// 缩进
    /// <para>默认为 "<see langword="    "/>"</para>
    /// </summary>
    public string Indent { get; set; } = "    ";

    #region ConvertName
    /// <summary>
    /// TomlBoolean类型转换名称
    /// <para>默认为 "<see langword="bool"/></para>
    /// </summary>
    public string TomlBooleanConvertName { get; set; } = "bool";

    /// <summary>
    /// TomlString类型转换名称
    /// <para>默认为 "<see langword="string"/>"</para>
    /// </summary>
    public string TomlStringConvertName { get; set; } = "string";

    /// <summary>
    /// TomlFloat类型转换名称
    /// <para>默认为 "<see langword="double"/>"</para>
    /// </summary>
    public string TomlFloatConvertName { get; set; } = "double";

    /// <summary>
    /// TomlInteger类型转换名称
    /// <para>默认为 "<see langword="int"/>"</para>
    /// </summary>
    public string TomlIntegerConvertName { get; set; } = "int";

    /// <summary>
    /// TomlInteger64类型转换名称
    /// <para>默认为 "<see langword="long"/>"</para>
    /// </summary>
    public string TomlInteger64ConvertName { get; set; } = "long";

    /// <summary>
    /// TomlDateTime类型转换名称
    /// <para>默认为 "<see langword="DateTime"/>"</para>
    /// </summary>
    public string TomlDateTimeConvertName { get; set; } = "DateTime";

    /// <summary>
    /// TomlDateTimeLocal类型转换名称
    /// <para>默认为 "<see langword="DateTime"/>"</para>
    /// </summary>
    public string TomlDateTimeLocalConvertName { get; set; } = "DateTime";

    /// <summary>
    /// TomlDateTimeOffset类型转换名称
    /// <para>默认为 "<see langword="DateTimeOffset"/>"</para>
    /// </summary>
    public string TomlDateTimeOffsetConvertName { get; set; } = "DateTimeOffset";
    #endregion
    #region Format
    /// <summary>
    /// 类名称格式化文本
    /// <para>默认为 "<see langword="{0}Class"/>"</para>
    /// </summary>
    public string ClassNameFormat { get; set; } = "{0}Class";

    /// <summary>
    /// 数组格式化文本
    /// <para>默认为 "<see langword="List&lt;{0}&gt;"/>"</para>
    /// </summary>
    public string ListFormat { get; set; } = "List<{0}>";

    /// <summary>
    /// 匿名类名称格式化文本
    /// <para>默认为 "<see langword="{0}Class{1}"/>"</para>
    /// </summary>
    public string AnonymousClassNameFormat { get; set; } = "{0}Class{1}";

    /// <summary>
    /// 属性格式化文本
    /// <para>默认为 "<see langword="{0}public {1} {2} {{ get; set; }}"/>"</para>
    /// </summary>
    public string ValueFormat { get; set; } = "{0}public {1} {2} {{ get; set; }}";

    /// <summary>
    /// 类格式化文本
    /// <para>默认为 "<see langword="public class {0} \n{{\n{1}}}\n"/>"</para>
    /// </summary>
    public string ClassFormat { get; set; } = "public class {0} \n{{\n{1}}}\n";

    /// <summary>
    /// 接口格式化文本
    /// <para>默认为 "<see langword=" : {0}"/>"</para>
    /// </summary>
    public string InterfaceFormat { get; set; } = " : {0}";

    /// <summary>
    /// 注释格式化文本
    /// <para>默认为 "<see langword="{0}/// &lt;summary&gt;\n{0}/// {1}\n{0}/// &lt;/summary&gt;\n"/>"</para>
    /// </summary>
    public string CommentFormat { get; set; } = "{0}/// <summary>\n{0}/// {1}\n{0}/// </summary>\n";

    /// <summary>
    /// 多行注释格式化文本
    /// <para>"<see langword="{0}/// &lt;para&gt;{1}&lt;/para&gt;"/>"</para>
    /// </summary>
    public string CommentParaFormat { get; set; } = "{0}/// <para>{1}</para>";

    /// <summary>
    /// TomlClass接口值格式化文本
    /// <para>默认为
    /// <![CDATA[
    /// {0}/// <inheritdoc/>
    /// {0}public string ClassComment { get; set; } = string.Empty;
    /// {0}/// <inheritdoc/>
    /// {0}public Dictionary<string, string> ValueComments { get; set; } = new();
    /// ]]>
    /// </para>
    /// </summary>
    public string ITomlClassInterfaceValueFomat { get; set; } =
        "{0}/// <inheritdoc/>\n{0}public string TableComment { get; set; } = string.Empty;\n"
        + "{0}/// <inheritdoc/>\n{0}public Dictionary<string, string> ValueComments { get; set; } = new();\n";
    #endregion
    /// <summary>
    /// 获取转换后的名称
    /// </summary>
    /// <param name="typeCode">类型标识</param>
    /// <param name="isInt64">是否为64位整型</param>
    /// <returns>标识转换的字符串</returns>
    public string GetConvertName(TomlNode node, TomlTypeCode typeCode)
    {
        return typeCode switch
        {
            TomlTypeCode.Boolean => TomlBooleanConvertName,
            TomlTypeCode.String => TomlStringConvertName,
            TomlTypeCode.Float => TomlFloatConvertName,
            TomlTypeCode.DateTime => TomlDateTimeConvertName,
            TomlTypeCode.DateTimeLocal => TomlDateTimeLocalConvertName,
            TomlTypeCode.DateTimeOffset => TomlDateTimeOffsetConvertName,
            TomlTypeCode.Integer when node.AsTomlInteger.IsInteger64 => TomlInteger64ConvertName,
            TomlTypeCode.Integer => TomlIntegerConvertName,
            _ => nameof(TomlNode)
        };
    }
}
