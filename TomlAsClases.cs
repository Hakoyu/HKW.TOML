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
    public string GetConvertName(TomlNode node)
    {
        return node switch
        {
            { IsTomlBoolean: true } => TomlBooleanConvertName,
            { IsTomlString: true } => TomlStringConvertName,
            { IsTomlFloat: true } => TomlFloatConvertName,
            { IsTomlInteger: true } => TomlIntegerConvertName,
            { IsTomlDateTimeLocal: true } => TomlDateTimeLocalConvertName,
            { IsTomlDateTimeOffset: true } => TomlDateTimeOffsetConvertName,
            { IsTomlDateTime: true } => TomlDateTimeConvertName,
            _ => string.Empty
        };
    }
}

public partial class TomlAsClasses
{
    private static string doubleListName = string.Empty;
    private static string intListName = string.Empty;
    private static readonly Dictionary<string, TomlClass> _tomlClasses = new();
    private static readonly Dictionary<string, TomlClass> _tempClasses = new();
    private static TomlAsClassesOptions _options = new();

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
            _options = options;
        doubleListName = string.Format(_options.TomlArrayFormat, _options.TomlFloatConvertName);
        intListName = string.Format(_options.TomlArrayFormat, _options.TomlIntegerConvertName);
        ParseTable(rootClassName, string.Empty, table);
        var sb = new StringBuilder();
        foreach (var tomlClass in _tomlClasses.Values)
            sb.AppendLine(tomlClass.ToString());
        foreach (var tomlClass in _tempClasses.Values)
            sb.AppendLine(tomlClass.ToString());
        return sb.ToString();
    }

    private static void ParseTable(string className, string parentClassName, TomlTable table)
    {
        var tomlClass = new TomlClass(className, parentClassName);
        _tomlClasses.Add(className, tomlClass);
        foreach (var kv in table)
        {
            var name = kv.Key;
            if (_options.KeyNameToPascal)
                name = ToPascal(name);
            var node = kv.Value;
            ParseTableValue(tomlClass, name, node, className, parentClassName);
        }
    }

    private static void ParseTableValue(TomlClass tomlClass, string name, TomlNode node, string className, string parentClassName)
    {
        if (node.IsTomlTable)
        {
            var nestedClassName = string.Format(_options.ClassNameFormat, ToPascal(name));
            if (string.IsNullOrWhiteSpace(parentClassName))
                tomlClass.Add(name, new(name, nestedClassName));
            else
                _tomlClasses[className].Add(name, new(name, nestedClassName));
            ParseTable(nestedClassName, className, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            var arrayTypeName = ParseArray(name, node.AsTomlArray);
            tomlClass.Add(name, new(name, arrayTypeName));
        }
        else
        {
            tomlClass.Add(name, new(name, node));
        }
    }

    private static string ParseArray(string tableName, TomlArray array) =>
        string.Format(_options.TomlArrayFormat, ParseArrayValueType(tableName, array));

    private static string ParseArrayValueType(string tableName, TomlArray array)
    {
        var typeData = new (bool IsType, List<int> Indexs)[9];
        var index = 0;
        foreach (var node in array)
        {
            var countIndex = GetTomlTypeIndex(node);
            var temp = typeData[countIndex];
            temp.IsType = true;
            temp.Indexs ??= new();
            temp.Indexs.Add(index++);
            typeData[countIndex] = temp;
        }
        var typeName = string.Empty;
        var typeCount = typeData.Count(i => i.IsType);
        if (
            _options.MergeIntegerAndFloat
            && typeData[2].IsType
            && typeData[3].IsType
            && typeCount is 2
        )
            typeName = _options.TomlFloatConvertName;
        else if (typeData[7].IsType && typeCount is 1)
        {
            typeName = ParseArrayInArrayValueType(tableName, typeData[7].Indexs, array);
        }
        else if (typeData[8].IsType && typeCount is 1)
        {
            typeName = ParseTableInArrayValue(tableName, array);
        }
        else
            typeName = typeCount is 1 ? _options.GetConvertName(array[0]) : nameof(TomlNode);
        if (string.IsNullOrWhiteSpace(typeName))
            typeName = nameof(TomlNode);
        return typeName;
    }
    private static string ParseArrayInArrayValueType(string tableName, IEnumerable<int> indexs, TomlArray array)
    {
        var typeNames = new HashSet<string>();
        foreach (var arrayIndex in indexs)
            typeNames.Add(ParseArray(tableName, array[arrayIndex].AsTomlArray));
        if (typeNames.Count is 1)
            return typeNames.First();
        else if (
            _options.MergeIntegerAndFloat
            && typeNames.Count is 2
            && typeNames.Contains(intListName)
            && typeNames.Contains(doubleListName)
        )
            return doubleListName;
        return string.Empty;
    }

    private static string ParseTableInArrayValue(string tableName, TomlArray array)
    {
        var tempClassName =
            string.Format(_options.TomlTableInArrayFormat, tableName, _tempClasses.Count);
        foreach (var item in array)
        {
            var table = item.AsTomlTable;
            ParseAnonymousTable(tempClassName, string.Empty, table);
        }
        return tempClassName;
    }

    private static int GetTomlTypeIndex(TomlNode node)
    {
        return node switch
        {
            { IsTomlBoolean: true } => 0,
            { IsTomlString: true } => 1,
            { IsTomlFloat: true } => 2,
            { IsTomlInteger: true } => 3,
            { IsTomlDateTimeLocal: true } => 4,
            { IsTomlDateTimeOffset: true } => 5,
            { IsTomlDateTime: true } => 6,
            { IsTomlArray: true } => 7,
            { IsTomlTable: true } => 8,
            _ => throw new ArgumentOutOfRangeException(nameof(node), node, null)
        };
    }
    private static void ParseAnonymousTable(
        string className,
        string parentClassName,
        TomlTable table
    )
    {
        if (_tempClasses.TryGetValue(className, out var tempClass) is false)
        {
            tempClass = new(className, parentClassName);
            _tempClasses.Add(className, tempClass);
        }
        foreach (var kv in table)
        {
            var name = kv.Key;
            if (_options.KeyNameToPascal)
                name = ToPascal(name);
            var node = kv.Value;
            ParseAnonymousTableValue(tempClass, name, node, className, parentClassName);
        }
    }

    private static void ParseAnonymousTableValue(TomlClass tempClass, string name, TomlNode node, string className, string parentClassName)
    {
        if (node.IsTomlTable)
        {
            var nestedClassName = string.Format(_options.ClassNameFormat, ToPascal(name));
            if (string.IsNullOrWhiteSpace(parentClassName))
                tempClass.TryAdd(name, new(name, nestedClassName));
            else
                _tempClasses[className].TryAdd(name, new(name, nestedClassName));
            ParseAnonymousTable(nestedClassName, className, node.AsTomlTable);
        }
        else if (node.IsTomlArray)
        {
            var arrayTypeName = ParseArray(name, node.AsTomlArray);
            tempClass.TryAdd(name, new(name, arrayTypeName));
        }
        else
        {
            tempClass.TryAdd(name, new(name, node));
        }
    }

    private static string ToPascal(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;
        var strs = str.Split(_options.KeyWordSeparator);
        var newStrs = strs.Select(s => FirstLetterToUpper(s));
        if (_options.RemoveKeyWordSeparator)
            return string.Join("", newStrs);
        else
            return string.Join("_", newStrs);
    }

    private static string FirstLetterToUpper(string str) => $"{char.ToUpper(str[0])}{str[1..]}";

    [DebuggerDisplay("{Name},Count = {Count}")]
    private class TomlClass : IDictionary<string, TomlClassValue>
    {
        public string Name { get; set; }
        public string ParentName { get; set; }

        private readonly Dictionary<string, TomlClassValue> _baseDictionary = new();

        public TomlClass(string name, string parentName = "")
        {
            Name = name;
            ParentName = parentName;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in _baseDictionary.Values)
                sb.AppendLine(item.ToString());
            return string.Format(_options.TomlTableFormat, Name, sb.ToString());
        }

        public TomlClassValue this[string key]
        {
            get => _baseDictionary[key];
            set => _baseDictionary[key] = value;
        }

        public ICollection<string> Keys => _baseDictionary.Keys;

        public ICollection<TomlClassValue> Values => _baseDictionary.Values;

        public int Count => _baseDictionary.Count;

        public bool IsReadOnly =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)_baseDictionary).IsReadOnly;

        public void Add(string key, TomlClassValue value) => _baseDictionary.Add(key, value);

        public void Add(KeyValuePair<string, TomlClassValue> item) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)_baseDictionary).Add(item);

        public bool TryAdd(string key, TomlClassValue value)
        {
            if (_baseDictionary.TryAdd(key, value) is false)
            {
                var classValue = _baseDictionary[key];
                if (
                    classValue.TypeName != value.TypeName && classValue.TypeName != nameof(TomlNode)
                )
                    classValue.TypeName = nameof(TomlNode);
                return false;
            }
            return true;
        }

        public void Clear() => _baseDictionary.Clear();

        public bool Contains(KeyValuePair<string, TomlClassValue> item) =>
            _baseDictionary.Contains(item);

        public bool ContainsKey(string key) => _baseDictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, TomlClassValue>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)_baseDictionary).CopyTo(
                array,
                arrayIndex
            );

        public IEnumerator<KeyValuePair<string, TomlClassValue>> GetEnumerator() =>
            _baseDictionary.GetEnumerator();

        public bool Remove(string key) => _baseDictionary.Remove(key);

        public bool Remove(KeyValuePair<string, TomlClassValue> item) =>
            ((ICollection<KeyValuePair<string, TomlClassValue>>)_baseDictionary).Remove(item);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TomlClassValue value) =>
            _baseDictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_baseDictionary).GetEnumerator();
    }

    [DebuggerDisplay("Type = {TypeName},Name = {Name}")]
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
            TypeName = GetTypeName(node);
        }

        private static string GetTypeName(TomlNode node)
        {
            return node switch
            {
                { IsTomlBoolean: true } => _options.TomlBooleanConvertName,
                { IsTomlString: true } => _options.TomlStringConvertName,
                { IsTomlFloat: true } => _options.TomlFloatConvertName,
                { IsTomlInteger: true } => _options.TomlIntegerConvertName,
                { IsTomlDateTimeLocal: true } => _options.TomlDateTimeLocalConvertName,
                { IsTomlDateTimeOffset: true } => _options.TomlDateTimeOffsetConvertName,
                { IsTomlDateTime: true } => _options.TomlDateTimeConvertName,
                _ => throw new ArgumentOutOfRangeException(nameof(node), node, null)
            };
        }

        public override string ToString() =>
            string.Format(_options.TomlValueFormat, TypeName, Name);
    }
}
