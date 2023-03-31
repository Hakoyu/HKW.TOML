using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HKW.Libs.TOML;

public class TomlAsClassesOptions
{
    public bool KeyNameToPascal { get; set; } = true;
    public bool MergeIntegerAndFloat { get; set; } = true;
    public bool RemoveKeyWordSeparator { get; set; } = true;
    public string KeyWordSeparator { get; set; } = "_";

    #region ConvertName
    public string TomlBooleanConvertName { get; set; } = "bool";
    public string TomlStringConvertName { get; set; } = "string";
    public string TomlFloatConvertName { get; set; } = "double";
    public string TomlIntegerConvertName { get; set; } = "int";
    public string TomlInteger64ConvertName { get; set; } = "long";
    public string TomlDateTimeLocalConvertName { get; set; } = "DateTime";
    public string TomlDateTimeOffsetConvertName { get; set; } = "DateTimeOffset";
    public string TomlDateTimeConvertName { get; set; } = "DateTime";
    #endregion
    #region Format
    public string ClassNameFormat { get; set; } = "{0}Class";
    public string TomlArrayFormat { get; set; } = "List<{0}>";
    public string TomlTableInArrayFormat { get; set; } = "{0}Class{1}";
    public string TomlValueFormat { get; set; } = "    public {0} {1} {{ get; set; }}";
    public string TomlTableFormat { get; set; } = "public class {0} \n{{\n{1}}}";
    #endregion
    public string GetConvertName(TomlTypeCode typeCode, bool isInt64 = false)
    {
        return typeCode switch
        {
            TomlTypeCode.Boolean => TomlBooleanConvertName,
            TomlTypeCode.String => TomlStringConvertName,
            TomlTypeCode.Float => TomlFloatConvertName,
            TomlTypeCode.DateTime => TomlDateTimeConvertName,
            TomlTypeCode.DateTimeLocal => TomlDateTimeLocalConvertName,
            TomlTypeCode.DateTimeOffset => TomlDateTimeOffsetConvertName,
            TomlTypeCode.Integer when isInt64 => TomlInteger64ConvertName,
            TomlTypeCode.Integer => TomlIntegerConvertName,
            _ => nameof(TomlNode)
        };
    }
}

public partial class TomlAsClasses
{
    private static string s_doubleListName = string.Empty;
    private static string s_intListName = string.Empty;
    private static int s_anonymousTableCount = 0;
    private static readonly Dictionary<string, TomlClass> s_tomlClasses = new();
    private static TomlAsClassesOptions s_options = new();

    private TomlAsClasses() { }

    public static string ConstructFromFile(string tomlFile, TomlAsClassesOptions? options = null)
    {
        var toml = TOML.Parse(tomlFile);
        var rootClassName = Path.GetFileNameWithoutExtension(tomlFile);
        return Construct(rootClassName, toml, options);
    }

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

    public static string Construct(
        string rootClassName,
        TomlTable table,
        TomlAsClassesOptions? options = null
    )
    {
        if (options is not null)
            s_options = options;
        s_doubleListName = string.Format(s_options.TomlArrayFormat, s_options.TomlFloatConvertName);
        s_intListName = string.Format(s_options.TomlArrayFormat, s_options.TomlIntegerConvertName);
        ParseTable(rootClassName, string.Empty, table);
        var sb = new StringBuilder();
        foreach (var tomlClass in s_tomlClasses.Values)
            sb.AppendLine(tomlClass.ToString());
        s_tomlClasses.Clear();
        s_anonymousTableCount = 0;
        s_options = new();
        return sb.ToString();
    }

    private static void ParseTable(string className, string parentClassName, TomlTable table)
    {
        if (s_csharpKeywords.Contains(className))
            throw new Exception($"Used CsharpKeywords \"{className}\"");
        if (s_tomlClasses.TryGetValue(className, out var tomlClass) is false)
        {
            tomlClass = new(className, parentClassName);
            s_tomlClasses.Add(tomlClass.FullName, tomlClass);
        }
        foreach (var kv in table)
        {
            var name = kv.Key;
            var node = kv.Value;
            if (s_csharpKeywords.Contains(name))
                throw new Exception($"Used CsharpKeywords \"{name}\" in \"{className}\"");
            if (s_options.KeyNameToPascal)
                name = ToPascal(name);
            ParseTableValue(tomlClass, name, node);
        }
    }

    private static void ParseTableValue(TomlClass tomlClass, string name, TomlNode node)
    {
        if (node.IsTomlTable)
        {
            var nestedClassName = string.Format(s_options.ClassNameFormat, ToPascal(name));
            if (string.IsNullOrWhiteSpace(tomlClass.ParentName))
                tomlClass.Add(name, new(name, nestedClassName));
            else
                s_tomlClasses[tomlClass.FullName].Add(name, new(name, nestedClassName));
            ParseTable(nestedClassName, tomlClass.Name, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            var arrayTypeName = ParseArray(name, node.AsTomlArray);
            tomlClass.TryAdd(name, new(name, arrayTypeName));
        }
        else
        {
            tomlClass.TryAdd(name, new(name, node));
        }
    }

    private static string ParseArray(string tableName, TomlArray array) =>
        string.Format(s_options.TomlArrayFormat, ParseArrayValueType(tableName, array));

    private static string ParseArrayValueType(string tableName, TomlArray array)
    {
        if (array.ChildrenCount is 0)
            return nameof(TomlNode);

        var isInt64 = false;
        var tomlTypeCode = array[0].TomlTypeCode;
        foreach (var node in array)
        {
            tomlTypeCode |= node.TomlTypeCode;
            if (
                tomlTypeCode == TomlTypeCode.Integer
                && int.TryParse(node.AsTomlInteger.Value.ToString(), out var _) is false
            )
                isInt64 = true;
        }

        var typeName = string.Empty;
        if (tomlTypeCode is TomlTypeCode.Array)
        {
            typeName = ParseValueInArray(tableName, array);
        }
        else if (tomlTypeCode is TomlTypeCode.Table)
        {
            typeName = ParseTableInArrayValue(tableName, array);
        }
        else
            typeName = s_options.GetConvertName(MergeTomlTypeCode(tomlTypeCode), isInt64);
        return typeName;
    }

    private static TomlTypeCode MergeTomlTypeCode(TomlTypeCode tomlTypeCode)
    {
        if (
            s_options.MergeIntegerAndFloat
            && tomlTypeCode is (TomlTypeCode.Integer | TomlTypeCode.Float)
        )
            return TomlTypeCode.Float;
        else
            return tomlTypeCode;
    }

    private static string ParseValueInArray(string tableName, TomlArray array)
    {
        var typeNames = new HashSet<string>();
        foreach (var node in array)
            typeNames.Add(ParseArray(tableName, node.AsTomlArray));
        if (typeNames.Count is 1)
            return typeNames.First();
        else if (
            s_options.MergeIntegerAndFloat
            && typeNames.Count is 2
            && typeNames.Contains(s_intListName)
            && typeNames.Contains(s_doubleListName)
        )
            return s_doubleListName;
        return string.Empty;
    }

    private static string ParseTableInArrayValue(string tableName, TomlArray array)
    {
        var tempClassName = string.Format(
            s_options.TomlTableInArrayFormat,
            tableName,
            s_anonymousTableCount++
        );
        foreach (var item in array)
        {
            var table = item.AsTomlTable;
            ParseTable(tempClassName, string.Empty, table);
        }
        return tempClassName;
    }

    private static string ToPascal(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;
        var strs = str.Split(s_options.KeyWordSeparator);
        var newStrs = strs.Select(s => FirstLetterToUpper(s));
        if (s_options.RemoveKeyWordSeparator)
            return string.Join("", newStrs);
        else
            return string.Join("_", newStrs);
    }

    private static string FirstLetterToUpper(string str) => $"{char.ToUpper(str[0])}{str[1..]}";

    [DebuggerDisplay("{Name},Count = {Count}")]
    private class TomlClass : IDictionary<string, TomlClassValue>
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string ParentName { get; set; }

        private readonly Dictionary<string, TomlClassValue> s_baseDictionary = new();

        public TomlClass(string name, string parentName = "")
        {
            Name = name;
            FullName = name + parentName;
            ParentName = parentName;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in s_baseDictionary.Values)
                sb.AppendLine(item.ToString());
            return string.Format(s_options.TomlTableFormat, Name, sb.ToString());
        }

        public TomlClassValue this[string key]
        {
            get => s_baseDictionary[key];
            set => s_baseDictionary[key] = value;
        }

        public ICollection<string> Keys => s_baseDictionary.Keys;

        public ICollection<TomlClassValue> Values => s_baseDictionary.Values;

        public int Count => s_baseDictionary.Count;

        public bool IsReadOnly =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)s_baseDictionary).IsReadOnly;

        public void Add(string key, TomlClassValue value) => s_baseDictionary.Add(key, value);

        public void Add(KeyValuePair<string, TomlClassValue> item) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)s_baseDictionary).Add(item);

        public bool TryAdd(string key, TomlClassValue value)
        {
            if (s_baseDictionary.TryAdd(key, value) is false)
            {
                var classValue = s_baseDictionary[key];
                if (
                    classValue.TypeName != value.TypeName && classValue.TypeName != nameof(TomlNode)
                )
                    classValue.TypeName = nameof(TomlNode);
                return false;
            }
            return true;
        }

        public void Clear() => s_baseDictionary.Clear();

        public bool Contains(KeyValuePair<string, TomlClassValue> item) =>
            s_baseDictionary.Contains(item);

        public bool ContainsKey(string key) => s_baseDictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, TomlClassValue>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)s_baseDictionary).CopyTo(
                array,
                arrayIndex
            );

        public IEnumerator<KeyValuePair<string, TomlClassValue>> GetEnumerator() =>
            s_baseDictionary.GetEnumerator();

        public bool Remove(string key) => s_baseDictionary.Remove(key);

        public bool Remove(KeyValuePair<string, TomlClassValue> item) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)s_baseDictionary).Remove(item);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TomlClassValue value) =>
            s_baseDictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)s_baseDictionary).GetEnumerator();
    }

    [DebuggerDisplay("{TypeName}, {Name}")]
    public class TomlClassValue
    {
        public string TypeName { get; set; }
        public string Name { get; set; }

        public TomlClassValue(string name, string typeName)
        {
            Name = name;
            TypeName = typeName;
        }

        public TomlClassValue(string name, TomlNode node)
        {
            Name = name;
            TypeName = s_options.GetConvertName(node.TomlTypeCode);
        }

        public override string ToString() =>
            string.Format(s_options.TomlValueFormat, TypeName, Name);
    }

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
