#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace HKW.TOML;

#region TOML TypeCode

/// <summary>
/// Toml类型代码
/// </summary>
[Flags]
public enum TomlTypeCode
{
    /// <summary>
    /// 空
    /// </summary>
    None = 0,

    /// <summary>
    /// 整型
    /// </summary>
    Integer = 1,

    /// <summary>
    /// 浮点型
    /// </summary>
    Float = 2,

    /// <summary>
    /// 字符串
    /// </summary>
    String = 4,

    /// <summary>
    /// 布尔类型
    /// </summary>
    Boolean = 8,

    /// <summary>
    /// 日期时间
    /// </summary>
    DateTime = 16,

    /// <summary>
    /// 地区日期时间
    /// </summary>
    DateTimeLocal = 32,

    /// <summary>
    /// 日期时间偏移量
    /// </summary>
    DateTimeOffset = 64,

    /// <summary>
    /// 数组
    /// </summary>
    Array = 128,

    /// <summary>
    /// 表格
    /// </summary>
    Table = 256,
}

/// <summary>
/// Toml类
/// </summary>
public class TomlType
{
    /// <summary>
    /// 获取Toml类型代码
    /// </summary>
    /// <param name="node">Toml节点</param>
    /// <returns>Toml类型代码</returns>
    public static TomlTypeCode GetTypeCode(TomlNode node)
    {
        return node switch
        {
            TomlBoolean => TomlTypeCode.Boolean,
            TomlString => TomlTypeCode.String,
            TomlInteger => TomlTypeCode.Integer,
            TomlFloat => TomlTypeCode.Float,
            TomlDateTimeOffset => TomlTypeCode.DateTimeOffset,
            TomlDateTimeLocal => TomlTypeCode.DateTime,
            TomlDateTime => TomlTypeCode.DateTime,
            TomlArray => TomlTypeCode.Array,
            TomlTable => TomlTypeCode.Table,
            _ => TomlTypeCode.None,
        };
    }
}

#endregion

#region TOML Nodes

/// <summary>
/// Toml节点
/// </summary>
public abstract class TomlNode : IEnumerable
{
    /// <summary>
    /// 有值
    /// </summary>
    public virtual bool HasValue { get; } = false;

    #region TypeCheck

    /// <summary>
    /// 是Toml数组
    /// </summary>
    public virtual bool IsTomlArray { get; } = false;

    /// <summary>
    /// 是Toml表格
    /// </summary>
    public virtual bool IsTomlTable { get; } = false;

    /// <summary>
    /// 是Toml字符串
    /// </summary>
    public virtual bool IsTomlString { get; } = false;

    /// <summary>
    /// 是Toml整型
    /// </summary>
    public virtual bool IsTomlInteger { get; } = false;

    /// <summary>
    /// 是Toml浮点型
    /// </summary>
    public virtual bool IsTomlFloat { get; } = false;

    /// <summary>
    /// 是Toml日期时间
    /// </summary>
    public bool IsTomlDateTime => IsTomlDateTimeLocal || IsTomlDateTimeOffset;

    /// <summary>
    /// 是Toml地区日期时间
    /// </summary>
    public virtual bool IsTomlDateTimeLocal { get; } = false;

    /// <summary>
    /// 是Toml偏移日期时间
    /// </summary>
    public virtual bool IsTomlDateTimeOffset { get; } = false;

    /// <summary>
    /// 是Toml布尔类型
    /// </summary>
    public virtual bool IsTomlBoolean { get; } = false;

    #endregion

    /// <summary>
    /// 注释
    /// </summary>
    public virtual string Comment { get; set; } = null!;

    /// <summary>
    /// 层级
    /// </summary>
    public virtual int CollapseLevel { get; set; }

    #region NativeType

    /// <summary>
    /// 转换为Int32
    /// </summary>
    public virtual int AsInt32 => (int)this;

    /// <summary>
    /// 转换为Int64
    /// </summary>
    public virtual long AsInt64 => (long)this;

    /// <summary>
    /// 转换为浮点
    /// </summary>
    public virtual float AsFloat => (float)Convert.ChangeType(this, TypeCode.Int64);

    /// <summary>
    /// 转换为双精度浮点
    /// </summary>
    public virtual double AsDouble => (double)this;

    /// <summary>
    /// 转换为字符串
    /// </summary>
    public virtual string AsString => AsTomlString;

    /// <summary>
    /// 转换为布尔
    /// </summary>
    public virtual bool AsBoolean => AsTomlBoolean;

    /// <summary>
    /// 转换为日期时间
    /// </summary>
    public virtual DateTime AsDateTime => AsTomlDateTimeLocal;

    /// <summary>
    /// 转换为日期时间偏移量
    /// </summary>
    public virtual DateTimeOffset AsDateTimeOffset => AsTomlDateTimeOffset;

    /// <summary>
    /// 转换为列表
    /// </summary>
    public virtual List<TomlNode> AsList => AsTomlArray.RawArray;

    /// <summary>
    /// 转换为字典
    /// </summary>
    public virtual Dictionary<string, TomlNode> AsDictionary => AsTomlTable.RawTable;

    #endregion

    #region TomlType

    /// <summary>
    /// 转换为Toml表格
    /// </summary>
    public virtual TomlTable AsTomlTable => (this as TomlTable)!;

    /// <summary>
    /// 转换为Toml字符串
    /// </summary>
    public virtual TomlString AsTomlString => (this as TomlString)!;

    /// <summary>
    /// 转换为Toml整型
    /// </summary>
    public virtual TomlInteger AsTomlInteger => (this as TomlInteger)!;

    /// <summary>
    /// 转换为Toml浮点型
    /// </summary>
    public virtual TomlFloat AsTomlFloat => (this as TomlFloat)!;

    /// <summary>
    /// 转换为Toml布尔型
    /// </summary>
    public virtual TomlBoolean AsTomlBoolean => (this as TomlBoolean)!;

    /// <summary>
    /// 转换为Toml日期时间
    /// </summary>
    public virtual TomlDateTime AsTomlDateTime => (this as TomlDateTime)!;

    /// <summary>
    /// 转换为Toml地区日期时间
    /// </summary>
    public virtual TomlDateTimeLocal AsTomlDateTimeLocal => (this as TomlDateTimeLocal)!;

    /// <summary>
    /// 转换为Toml日期时间偏移量
    /// </summary>
    public virtual TomlDateTimeOffset AsTomlDateTimeOffset => (this as TomlDateTimeOffset)!;

    /// <summary>
    /// 转化为Toml数组
    /// </summary>
    public virtual TomlArray AsTomlArray => (this as TomlArray)!;

    #endregion

    /// <summary>
    /// 子数量
    /// </summary>
    public virtual int ChildrenCount => 0;

    /// <summary>
    /// 使用键获取值(用于Toml表格)
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>值</returns>
    public virtual TomlNode this[string key]
    {
        get => null!;
        set { }
    }

    /// <summary>
    /// 使用索引获取值(用于Toml数组)
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>值</returns>
    public virtual TomlNode this[int index]
    {
        get => null!;
        set { }
    }

    /// <summary>
    /// 子
    /// </summary>
    public virtual IEnumerable<TomlNode> Children
    {
        get { yield break; }
    }

    /// <summary>
    /// 所有键
    /// </summary>
    public virtual IEnumerable<string> Keys
    {
        get { yield break; }
    }

    /// <inheritdoc/>
    public IEnumerator GetEnumerator() => Children.GetEnumerator();

    /// <summary>
    /// 尝试获取值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="node">值</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public virtual bool TryGetNode(string key, out TomlNode node)
    {
        node = null!;
        return false;
    }

    /// <summary>
    /// 拥有键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public virtual bool HasKey(string key) => false;

    /// <summary>
    /// 拥有索引
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public virtual bool HasItemAt(int index) => false;

    /// <summary>
    /// 添加键值对(用于Toml表格)
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="node">值</param>
    public virtual void Add(string key, TomlNode node) { }

    /// <summary>
    /// 添加值(用于Toml数组)
    /// </summary>
    /// <param name="node">值</param>
    public virtual void Add(TomlNode node) { }

    /// <summary>
    /// 删除值(用于Toml数组)
    /// </summary>
    /// <param name="node">值</param>
    public virtual void Delete(TomlNode node) { }

    /// <summary>
    /// 使用键删除值(用于Toml表格)
    /// </summary>
    /// <param name="key"></param>
    public virtual void Delete(string key) { }

    /// <summary>
    /// 使用索引删除值(用于Toml数组)
    /// </summary>
    /// <param name="index"></param>
    public virtual void Delete(int index) { }

    /// <summary>
    /// 添加多个值(用于Toml数组)
    /// </summary>
    /// <param name="nodes">多个值</param>
    public virtual void AddRange(IEnumerable<TomlNode> nodes)
    {
        foreach (var tomlNode in nodes)
            Add(tomlNode);
    }

    /// <summary>
    /// 写入至
    /// </summary>
    /// <param name="tw">文本写入流</param>
    /// <param name="name">名称</param>
    public virtual void WriteTo(TextWriter tw, string name = null!) => tw.WriteLine(ToInlineToml());

    /// <summary>
    /// 转换为行内Toml格式字符串
    /// </summary>
    /// <returns>Toml格式字符串</returns>
    public virtual string ToInlineToml() => ToString()!;

    #region Native type to TOML cast

    /// <summary>
    /// 隐式转换 string -> TomlString
    /// </summary>
    /// <param name="value">字符串</param>
    public static implicit operator TomlNode(string value) => new TomlString { Value = value };

    /// <summary>
    /// 隐式转换 bool -> TomlBoolean
    /// </summary>
    /// <param name="value">布尔类型</param>

    public static implicit operator TomlNode(bool value) => new TomlBoolean { Value = value };

    /// <summary>
    /// 隐式转换 int -> TomlInteger
    /// </summary>
    /// <param name="value">整型</param>

    public static implicit operator TomlNode(int value) => new TomlInteger { Value = value };

    /// <summary>
    /// 隐式转换 long -> TomlInteger
    /// </summary>
    /// <param name="value">64位整型</param>

    public static implicit operator TomlNode(long value) => new TomlInteger { Value = value };

    /// <summary>
    /// 隐式转换 float -> TomlFloat
    /// </summary>
    /// <param name="value">浮点型</param>

    public static implicit operator TomlNode(float value) => new TomlFloat { Value = value };

    /// <summary>
    /// 隐式转换 double -> TomlFloat
    /// </summary>
    /// <param name="value">双精度浮点</param>

    public static implicit operator TomlNode(double value) => new TomlFloat { Value = value };

    /// <summary>
    /// 隐式转换 DateTime -> TomlDateTimeLocal
    /// </summary>
    /// <param name="value">日期时间</param>
    public static implicit operator TomlNode(DateTime value) =>
        new TomlDateTimeLocal { Value = value };

    /// <summary>
    /// 隐式转换 DateTimeOffset -> TomlDateTimeOffset
    /// </summary>
    /// <param name="value">日期时间偏移量</param>
    public static implicit operator TomlNode(DateTimeOffset value) =>
        new TomlDateTimeOffset { Value = value };

    /// <summary>
    /// 隐式转换 TomlNode[] -> TomlArray
    /// </summary>
    /// <param name="nodes">TomlNode数组</param>
    public static implicit operator TomlNode(TomlNode[] nodes)
    {
        var result = new TomlArray();
        result.AddRange(nodes);
        return result;
    }

    #endregion

    #region TOML to native type cast

    /// <summary>
    /// 隐式转换 TomlNode -> string
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator string(TomlNode value) => value.ToString()!;

    /// <summary>
    /// 隐式转换 TomlNode -> int
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator int(TomlNode value)
    {
        if (value.IsTomlInteger)
            return (int)value.AsTomlInteger.Value;
        else
            return (int)value.AsTomlFloat.Value;
    }

    /// <summary>
    /// 隐式转换 TomlNode -> long
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator long(TomlNode value)
    {
        if (value.IsTomlInteger)
            return value.AsTomlInteger.Value;
        else
            return (long)value.AsTomlFloat.Value;
    }

    /// <summary>
    /// 隐式转换 TomlNode -> float
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator float(TomlNode value)
    {
        if (value.IsTomlInteger)
            return value.AsTomlInteger.Value;
        else
            return (float)value.AsTomlFloat.Value;
    }

    /// <summary>
    /// 隐式转换 TomlNode -> double
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator double(TomlNode value)
    {
        if (value.IsTomlInteger)
            return value.AsTomlInteger.Value;
        else
            return (double)value.AsTomlFloat.Value;
    }

    /// <summary>
    /// 隐式转换 TomlNode -> bool
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator bool(TomlNode value) => value.AsTomlBoolean.Value;

    /// <summary>
    /// 隐式转换 TomlNode -> DateTime
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator DateTime(TomlNode value) => value.AsTomlDateTimeLocal.Value;

    /// <summary>
    /// 隐式转换 TomlNode -> DateTimeOffset
    /// </summary>
    /// <param name="value">Toml节点</param>
    public static implicit operator DateTimeOffset(TomlNode value) =>
        value.AsTomlDateTimeOffset.Value;

    #endregion
}

/// <summary>
/// Toml字符串
/// </summary>
public class TomlString : TomlNode
{
    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <inheritdoc/>
    public override bool IsTomlString { get; } = true;

    /// <summary>
    /// 是多行
    /// </summary>
    public bool IsMultiline { get; set; }

    /// <summary>
    /// 是多行首行
    /// </summary>
    public bool MultilineTrimFirstLine { get; set; }

    /// <summary>
    /// 首选文字
    /// </summary>
    public bool PreferLiteral { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <inheritdoc/>
    public override string ToString() => Value;

    /// <inheritdoc/>
    public override string ToInlineToml()
    {
        // Automatically convert literal to non-literal if there are too many literal string symbols
        if (
            Value.IndexOf(
                new string(TomlSyntax.LITERAL_STRING_SYMBOL, IsMultiline ? 3 : 1),
                StringComparison.Ordinal
            ) != -1
            && PreferLiteral
        )
            PreferLiteral = false;
        var quotes = new string(
            PreferLiteral ? TomlSyntax.LITERAL_STRING_SYMBOL : TomlSyntax.BASIC_STRING_SYMBOL,
            IsMultiline ? 3 : 1
        );
        var result = PreferLiteral ? Value : Value.Escape(IsMultiline is false);
        if (IsMultiline)
        {
            result = result.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
            if (
                MultilineTrimFirstLine
                || MultilineTrimFirstLine is false && result.StartsWith(Environment.NewLine)
            )
                result = $"{Environment.NewLine}{result}";
        }
        return $"{quotes}{result}{quotes}";
    }
}

/// <summary>
/// Toml整型
/// </summary>
public class TomlInteger : TomlNode
{
    /// <summary>
    /// 进位制
    /// </summary>
    public enum Base
    {
        /// <summary>
        /// 二进制
        /// </summary>
        Binary = 2,

        /// <summary>
        /// 八进制
        /// </summary>
        Octal = 8,

        /// <summary>
        /// 十进制
        /// </summary>
        Decimal = 10,

        /// <summary>
        /// 十六进制
        /// </summary>
        Hexadecimal = 16
    }

    /// <inheritdoc/>
    public override bool IsTomlInteger { get; } = true;

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <summary>
    /// 整型进位制
    /// </summary>
    public Base IntegerBase { get; set; } = Base.Decimal;

    /// <summary>
    /// 值
    /// </summary>
    public long Value { get; set; }

    /// <summary>
    /// 是64位整型
    /// </summary>
    public bool IsInteger64 => int.TryParse(Value.ToString(), out _) is false;

    /// <inheritdoc/>
    public override string ToString() => Value.ToString();

    /// <inheritdoc/>
    public override string ToInlineToml() =>
        IntegerBase != Base.Decimal
            ? $"0{TomlSyntax.BaseIdentifiers[(int)IntegerBase]}{Convert.ToString(Value, (int)IntegerBase)}"
            : Value.ToString(CultureInfo.InvariantCulture);
}

/// <summary>
/// Toml浮点型
/// </summary>
public class TomlFloat : TomlNode, IFormattable
{
    /// <inheritdoc/>
    public override bool IsTomlFloat { get; } = true;

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <summary>
    /// 值
    /// </summary>
    public double Value { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        Value.ToString(format, formatProvider);

    /// <inheritdoc/>
    public string ToString(IFormatProvider formatProvider) => Value.ToString(formatProvider);

    /// <inheritdoc/>
    public override string ToInlineToml() =>
        Value switch
        {
            var v when double.IsNaN(v) => TomlSyntax.NAN_VALUE,
            var v when double.IsPositiveInfinity(v) => TomlSyntax.INF_VALUE,
            var v when double.IsNegativeInfinity(v) => TomlSyntax.NEG_INF_VALUE,
            var v => v.ToString("G", CultureInfo.InvariantCulture).ToLowerInvariant()
        };
}

/// <summary>
/// Toml布尔类型
/// </summary>
public class TomlBoolean : TomlNode
{
    /// <inheritdoc/>
    public override bool IsTomlBoolean { get; } = true;

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <summary>
    /// 值
    /// </summary>
    public bool Value { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Value.ToString();

    /// <inheritdoc/>
    public override string ToInlineToml() => Value ? TomlSyntax.TRUE_VALUE : TomlSyntax.FALSE_VALUE;
}

/// <summary>
/// Toml日期时间
/// </summary>
public class TomlDateTime : TomlNode, IFormattable
{
    /// <summary>
    /// 秒级精度
    /// </summary>
    public int SecondsPrecision { get; set; }

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <inheritdoc/>
    public virtual string ToString(string? format, IFormatProvider? formatProvider) => string.Empty;

    /// <inheritdoc/>
    public virtual string ToString(IFormatProvider formatProvider) => string.Empty;

    /// <summary>
    /// 转换为行内内部Toml格式字符串
    /// </summary>
    /// <returns>Toml格式字符串</returns>
    protected virtual string ToInlineTomlInternal() => string.Empty;

    /// <inheritdoc/>
    public override string ToInlineToml() =>
        ToInlineTomlInternal()
            .Replace(TomlSyntax.RFC3339EmptySeparator, TomlSyntax.ISO861Separator)
            .Replace(TomlSyntax.ISO861ZeroZone, TomlSyntax.RFC3339ZeroZone);
}

/// <summary>
/// Toml日期时间偏移量
/// </summary>
public class TomlDateTimeOffset : TomlDateTime
{
    /// <inheritdoc/>
    public override bool IsTomlDateTimeOffset { get; } = true;

    /// <summary>
    /// 值
    /// </summary>
    public DateTimeOffset Value { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);

    /// <inheritdoc/>
    public override string ToString(IFormatProvider formatProvider) =>
        Value.ToString(formatProvider);

    /// <inheritdoc/>
    public override string ToString(string? format, IFormatProvider? formatProvider) =>
        Value.ToString(format, formatProvider);

    /// <inheritdoc/>
    protected override string ToInlineTomlInternal() =>
        Value.ToString(TomlSyntax.RFC3339Formats[SecondsPrecision]);
}

/// <summary>
/// Toml地区日期时间
/// </summary>
public class TomlDateTimeLocal : TomlDateTime
{
    /// <summary>
    /// 日期时间类型
    /// </summary>
    public enum DateTimeStyle
    {
        /// <summary>
        /// 日期
        /// </summary>
        Date,

        /// <summary>
        /// 时间
        /// </summary>
        Time,

        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime
    }

    /// <inheritdoc/>
    public override bool IsTomlDateTimeLocal { get; } = true;

    /// <summary>
    /// 日期时间类型
    /// </summary>
    public DateTimeStyle Style { get; set; } = DateTimeStyle.DateTime;

    /// <summary>
    /// 值
    /// </summary>
    public DateTime Value { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);

    /// <inheritdoc/>
    public override string ToString(IFormatProvider formatProvider) =>
        Value.ToString(formatProvider);

    /// <inheritdoc/>
    public override string ToString(string? format, IFormatProvider? formatProvider) =>
        Value.ToString(format, formatProvider);

    /// <inheritdoc/>
    public override string ToInlineToml() =>
        Style switch
        {
            DateTimeStyle.Date => Value.ToString(TomlSyntax.LocalDateFormat),
            DateTimeStyle.Time
                => Value.ToString(TomlSyntax.RFC3339LocalTimeFormats[SecondsPrecision]),
            var _ => Value.ToString(TomlSyntax.RFC3339LocalDateTimeFormats[SecondsPrecision])
        };
}

/// <summary>
/// Toml数组
/// </summary>
public class TomlArray : TomlNode, IEnumerable<TomlNode>
{
    /// <inheritdoc/>
    public new IEnumerator<TomlNode> GetEnumerator() => RawArray.GetEnumerator();

    /// <summary>
    /// 原始值
    /// </summary>
    public List<TomlNode> RawArray { get; private set; } = new();

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <inheritdoc/>
    public override bool IsTomlArray { get; } = true;

    /// <summary>
    /// 是多行
    /// </summary>
    public bool IsMultiline { get; set; }

    /// <summary>
    /// 是Toml表格数组
    /// </summary>
    public bool IsTableArray { get; set; }

    /// <inheritdoc/>
    public override TomlNode this[int index]
    {
        get
        {
            if (index < RawArray.Count)
                return RawArray[index];
            var lazy = new TomlLazy(this);
            this[index] = lazy;
            return lazy;
        }
        set
        {
            if (index == RawArray.Count)
                RawArray.Add(value);
            else
                RawArray[index] = value;
        }
    }

    /// <inheritdoc/>
    public override int ChildrenCount => RawArray.Count;

    /// <inheritdoc/>
    public override IEnumerable<TomlNode> Children => RawArray.AsEnumerable();

    /// <inheritdoc/>
    public override void Add(TomlNode node) => RawArray.Add(node);

    /// <inheritdoc/>
    public override void AddRange(IEnumerable<TomlNode> nodes) => RawArray.AddRange(nodes);

    /// <inheritdoc/>
    public override void Delete(TomlNode node) => RawArray.Remove(node);

    /// <inheritdoc/>
    public override void Delete(int index) => RawArray.RemoveAt(index);

    /// <inheritdoc/>
    public override string ToString() => ToString(false);

    /// <summary>
    /// 转化为多行字符串
    /// </summary>
    /// <param name="multiline">是多行</param>
    /// <returns>多行字符串</returns>
    public string ToString(bool multiline)
    {
        var sb = new StringBuilder();
        sb.Append(TomlSyntax.ARRAY_START_SYMBOL);
        if (ChildrenCount != 0)
        {
            var arrayStart = multiline ? $"{Environment.NewLine}  " : " ";
            var arraySeparator = multiline
                ? $"{TomlSyntax.ITEM_SEPARATOR}{Environment.NewLine}  "
                : $"{TomlSyntax.ITEM_SEPARATOR} ";
            var arrayEnd = multiline ? Environment.NewLine : " ";
            sb.Append(arrayStart)
                .Append(arraySeparator.Join(RawArray.Select(n => n.ToInlineToml())))
                .Append(arrayEnd);
        }
        sb.Append(TomlSyntax.ARRAY_END_SYMBOL);
        return sb.ToString();
    }

    /// <inheritdoc/>
    public override void WriteTo(TextWriter tw, string name = null!)
    {
        // If it's a normal array, write it as usual
        if (IsTableArray is false)
        {
            tw.WriteLine(ToString(IsMultiline));
            return;
        }

        if (string.IsNullOrWhiteSpace(Comment) is false)
        {
            tw.WriteLine();
            Comment.AsComment(tw);
        }
        tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
        tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
        tw.Write(name);
        tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
        tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
        tw.WriteLine();

        var first = true;

        foreach (var tomlNode in RawArray)
        {
            if (tomlNode is not TomlTable tbl)
                throw new TomlFormatException(
                    "The array is marked as array table but contains non-table nodes!"
                );

            // Ensure it's parsed as a section
            tbl.IsInline = false;

            if (first is false)
            {
                tw.WriteLine();

                if (string.IsNullOrWhiteSpace(Comment) is false)
                    Comment?.AsComment(tw);
                tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
                tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
                tw.Write(name);
                tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
                tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
                tw.WriteLine();
            }

            first = false;

            // Don't write section since it's already written here
            tbl.WriteTo(tw, name, false);
        }
    }
}

/// <summary>
/// Toml表格
/// </summary>
public class TomlTable : TomlNode, IDictionary<string, TomlNode>
{
    /// <inheritdoc/>
    public new IEnumerator<KeyValuePair<string, TomlNode>> GetEnumerator() =>
        RawTable.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// 是隐式的
    /// </summary>
    internal bool isImplicit;

    /// <inheritdoc/>
    public override bool HasValue { get; } = false;

    /// <inheritdoc/>
    public override bool IsTomlTable { get; } = true;

    /// <summary>
    /// 在行内
    /// </summary>
    public bool IsInline { get; set; }

    /// <summary>
    /// 原始值
    /// </summary>
    public Dictionary<string, TomlNode> RawTable { get; private set; } = new();

    /// <inheritdoc/>
    public override TomlNode this[string key]
    {
        get
        {
            if (RawTable.TryGetValue(key, out var result))
                return result;
            var lazy = new TomlLazy(this);
            RawTable[key] = lazy;
            return lazy;
        }
        set => RawTable[key] = value;
    }

    /// <inheritdoc/>
    public override int ChildrenCount => RawTable.Count;

    /// <inheritdoc/>
    public override IEnumerable<TomlNode> Children => RawTable.Select(kv => kv.Value);

    /// <inheritdoc/>
    public override IEnumerable<string> Keys => RawTable.Select(kv => kv.Key);

    /// <inheritdoc/>
    ICollection<string> IDictionary<string, TomlNode>.Keys =>
        ((IDictionary<string, TomlNode>)RawTable).Keys;

    /// <inheritdoc/>
    public ICollection<TomlNode> Values => ((IDictionary<string, TomlNode>)RawTable).Values;

    /// <inheritdoc/>
    public int Count => ((ICollection<KeyValuePair<string, TomlNode>>)RawTable).Count;

    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<KeyValuePair<string, TomlNode>>)RawTable).IsReadOnly;

    /// <inheritdoc/>
    public override bool HasKey(string key) => RawTable.ContainsKey(key);

    /// <inheritdoc/>
    public override void Add(string key, TomlNode node) => RawTable.Add(key, node);

    /// <summary>
    /// 添加多个键值对
    /// </summary>
    /// <param name="table">Toml表格</param>
    public void AddRange(TomlTable table) => AddRange(table.AsDictionary);

    /// <summary>
    /// 添加多个键值对
    /// </summary>
    /// <param name="dic">多个键值对</param>
    public void AddRange(IDictionary<string, TomlNode> dic)
    {
        foreach (var kv in dic)
            RawTable.Add(kv.Key, kv.Value);
    }

    /// <inheritdoc/>
    public override bool TryGetNode(string key, out TomlNode node) =>
        RawTable.TryGetValue(key, out node!);

    /// <inheritdoc/>
    public override void Delete(TomlNode node) =>
        RawTable.Remove(RawTable.First(kv => kv.Value == node).Key);

    /// <inheritdoc/>
    public override void Delete(string key) => RawTable.Remove(key);

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TomlSyntax.INLINE_TABLE_START_SYMBOL);

        if (ChildrenCount != 0)
        {
            var collapsed = CollectCollapsedItems(normalizeOrder: false);

            if (collapsed.Count != 0)
            {
                sb.Append(' ');
                sb.Append(
                    $"{TomlSyntax.ITEM_SEPARATOR} ".Join(
                        collapsed.Select(
                            n =>
                                $"{n.Key} {TomlSyntax.KEY_VALUE_SEPARATOR} {n.Value.ToInlineToml()}"
                        )
                    )
                );
            }
            sb.Append(' ');
        }

        sb.Append(TomlSyntax.INLINE_TABLE_END_SYMBOL);
        return sb.ToString();
    }

    /// <summary>
    /// 收集折叠的项目
    /// </summary>
    /// <param name="prefix">前缀</param>
    /// <param name="level">等级</param>
    /// <param name="normalizeOrder">正常顺序</param>
    /// <returns></returns>
    private LinkedList<KeyValuePair<string, TomlNode>> CollectCollapsedItems(
        string prefix = "",
        int level = 0,
        bool normalizeOrder = true
    )
    {
        var nodes = new LinkedList<KeyValuePair<string, TomlNode>>();
        var postNodes = normalizeOrder ? new LinkedList<KeyValuePair<string, TomlNode>>() : nodes;

        foreach (var kv in RawTable)
        {
            var node = kv.Value;
            var key = kv.Key.AsKey();

            if (node is TomlTable table)
            {
                var subnodes = table.CollectCollapsedItems(
                    $"{prefix}{key}.",
                    level + 1,
                    normalizeOrder
                );
                // Write main table first before writing collapsed items
                if (subnodes.Count == 0 && node.CollapseLevel == level)
                {
                    postNodes.AddLast(new KeyValuePair<string, TomlNode>($"{prefix}{key}", node));
                }
                foreach (var subKV in subnodes)
                    postNodes.AddLast(subKV);
            }
            else if (node.CollapseLevel == level)
                nodes.AddLast(new KeyValuePair<string, TomlNode>($"{prefix}{key}", node));
        }

        if (normalizeOrder)
            foreach (var kv in postNodes)
                nodes.AddLast(kv);

        return nodes;
    }

    /// <summary>
    /// 保存至
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    public void SaveToFile(string tomlFile)
    {
        using var sw = new StreamWriter(tomlFile);
        WriteTo(sw, null!, false);
    }

    /// <summary>
    /// 转换为Toml格式文本
    /// </summary>
    /// <returns></returns>
    public string ToTomlString()
    {
        using var ms = new MemoryStream();
        using (var sw = new StreamWriter(ms, leaveOpen: true))
        {
            WriteTo(sw, null!, false);
        }
        ms.Position = 0;
        using var rw = new StreamReader(ms);
        return rw.ReadToEnd();
    }

    /// <inheritdoc/>
    public override void WriteTo(TextWriter tw, string tomlFile) => WriteTo(tw, tomlFile, true);

    /// <summary>
    /// 写入至
    /// </summary>
    /// <param name="tw">文本写入器</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="writeSectionName">写入章节名</param>
    internal void WriteTo(TextWriter tw, string tomlFile, bool writeSectionName)
    {
        // The table is inline table
        if (IsInline && tomlFile is not null)
        {
            tw.WriteLine(ToInlineToml());
            return;
        }

        var collapsedItems = CollectCollapsedItems();

        if (collapsedItems.Count == 0)
            return;

        var hasRealValues = !collapsedItems.All(
            n => n.Value is TomlTable { IsInline: false } or TomlArray { IsTableArray: true }
        );

        if (string.IsNullOrWhiteSpace(Comment) is false)
            Comment.AsComment(tw);

        if (
            tomlFile is not null
            && (hasRealValues || string.IsNullOrWhiteSpace(Comment) is false)
            && writeSectionName
        )
        {
            tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
            tw.Write(tomlFile);
            tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
            tw.WriteLine();
        }
        else if (string.IsNullOrWhiteSpace(Comment) is false) // Add some spacing between the first node and the comment
        {
            tw.WriteLine();
        }

        var namePrefix = tomlFile == null ? "" : $"{tomlFile}.";
        var first = true;

        foreach (var collapsedItem in collapsedItems)
        {
            var key = collapsedItem.Key;
            if (
                collapsedItem.Value
                is TomlArray { IsTableArray: true }
                    or TomlTable { IsInline: false }
            )
            {
                if (first is false)
                    tw.WriteLine();
                first = false;
                collapsedItem.Value.WriteTo(tw, $"{namePrefix}{key}");
                continue;
            }
            first = false;
            if (string.IsNullOrWhiteSpace(collapsedItem.Value.Comment) is false)
                collapsedItem.Value.Comment.AsComment(tw);
            tw.Write(key);
            tw.Write(' ');
            tw.Write(TomlSyntax.KEY_VALUE_SEPARATOR);
            tw.Write(' ');

            collapsedItem.Value.WriteTo(tw, $"{namePrefix}{key}");
        }
    }

    /// <inheritdoc/>
    public bool ContainsKey(string key) =>
        ((IDictionary<string, TomlNode>)RawTable).ContainsKey(key);

    /// <inheritdoc/>
    public bool Remove(string key) => ((IDictionary<string, TomlNode>)RawTable).Remove(key);

    /// <inheritdoc/>
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out TomlNode value) =>
        ((IDictionary<string, TomlNode>)RawTable).TryGetValue(key, out value);

    /// <inheritdoc/>
    public void Add(KeyValuePair<string, TomlNode> item) =>
        ((ICollection<KeyValuePair<string, TomlNode>>)RawTable).Add(item);

    /// <inheritdoc/>
    public void Clear() => ((ICollection<KeyValuePair<string, TomlNode>>)RawTable).Clear();

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<string, TomlNode> item) =>
        ((ICollection<KeyValuePair<string, TomlNode>>)RawTable).Contains(item);

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<string, TomlNode>[] array, int arrayIndex) =>
        ((ICollection<KeyValuePair<string, TomlNode>>)RawTable).CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<string, TomlNode> item) =>
        ((ICollection<KeyValuePair<string, TomlNode>>)RawTable).Remove(item);
}

internal class TomlLazy : TomlNode
{
    private readonly TomlNode parent;
    private TomlNode replacement = null!;

    public TomlLazy(TomlNode parent) => this.parent = parent;

    /// <inheritdoc/>
    public override TomlNode this[int index]
    {
        get => Set<TomlArray>()[index];
        set => Set<TomlArray>()[index] = value;
    }

    /// <inheritdoc/>
    public override TomlNode this[string key]
    {
        get => Set<TomlTable>()[key];
        set => Set<TomlTable>()[key] = value;
    }

    /// <inheritdoc/>
    public override void Add(TomlNode node) => Set<TomlArray>().Add(node);

    /// <inheritdoc/>
    public override void Add(string key, TomlNode node) => Set<TomlTable>().Add(key, node);

    /// <inheritdoc/>
    public override void AddRange(IEnumerable<TomlNode> nodes) => Set<TomlArray>().AddRange(nodes);

    /// <summary>
    /// 设置Toml节点
    /// </summary>
    /// <typeparam name="T">TomlNode</typeparam>
    /// <returns>Toml节点</returns>
    private TomlNode Set<T>()
        where T : TomlNode, new()
    {
        if (replacement is not null)
            return replacement;

        var newNode = new T { Comment = Comment };

        if (parent.IsTomlTable)
        {
            var key = parent.Keys.FirstOrDefault(
                s => parent.TryGetNode(s, out var node) && node.Equals(this)
            );
            if (key == null)
                return default(T)!;

            parent[key] = newNode;
        }
        else if (parent.IsTomlArray)
        {
            var index = parent.Children.TakeWhile(child => child != this).Count();
            if (index == parent.ChildrenCount)
                return default(T)!;
            parent[index] = newNode;
        }
        else
        {
            return default(T)!;
        }

        replacement = newNode;
        return newNode;
    }
}

#endregion

#region Parser

/// <summary>
/// TOML解析器
/// </summary>
public class TOMLParser : IDisposable
{
    /// <summary>
    /// 解析状态
    /// </summary>
    public enum ParseState
    {
        /// <summary>
        /// 空
        /// </summary>
        None,

        /// <summary>
        /// 键值对
        /// </summary>
        KeyValuePair,

        /// <summary>
        /// 跳过下一行
        /// </summary>
        SkipToNextLine,

        /// <summary>
        /// 表格
        /// </summary>
        Table
    }

    /// <summary>
    /// 强制ASCII编码
    /// </summary>
    public bool ForceASCII { get; set; }

    /// <summary>
    /// 文本写入器
    /// </summary>
    private readonly TextReader r_reader;

    /// <summary>
    /// 当前状态
    /// </summary>
    private ParseState _currentState;

    /// <summary>
    /// 行
    /// </summary>
    private int _line;

    /// <summary>
    /// 列
    /// </summary>
    private int _column;

    private List<TomlSyntaxException> _syntaxErrors = null!;

    /// <summary>
    /// 从文本读取器解析
    /// </summary>
    /// <param name="reader">文明读取器</param>
    public TOMLParser(TextReader reader)
    {
        r_reader = reader;
        _line = _column = 0;
    }

    /// <summary>
    /// 弃置
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        r_reader?.Dispose();
    }

    /// <summary>
    /// 解析
    /// </summary>
    /// <returns>解析完成的Toml表格</returns>
    /// <exception cref="TomlParseException">解析错误</exception>
    public TomlTable Parse()
    {
        _syntaxErrors = new List<TomlSyntaxException>();
        _line = _column = 1;
        var rootTable = new TomlTable();
        var currentTable = rootTable;
        _currentState = ParseState.None;
        var keyParts = new List<string>();
        var isArrayTable = false;
        StringBuilder latestComment = null!;
        var isFirstComment = true;

        int currentChar;
        while ((currentChar = r_reader.Peek()) >= 0)
        {
            var c = (char)currentChar;

            if (_currentState is ParseState.None)
            {
                if (
                    ParseNone(c, rootTable, ref isFirstComment, ref latestComment)
                    is bool noneParsed
                )
                {
                    if (noneParsed)
                        ConsumeCharacter();
                    continue;
                }
            }
            if (_currentState is ParseState.KeyValuePair)
            {
                if (ParseKeyValuePair(currentTable, keyParts, ref latestComment))
                    continue;
            }
            if (_currentState is ParseState.Table)
            {
                if (
                    ParseTable(
                        c,
                        rootTable,
                        ref currentTable,
                        keyParts,
                        ref isArrayTable,
                        ref latestComment
                    )
                    is bool tableParsed
                )
                {
                    if (tableParsed)
                        ConsumeCharacter();
                    continue;
                }
            }
            if (_currentState is ParseState.SkipToNextLine)
            {
                if (ParseSkipToNextLine(c) is bool skipPased)
                {
                    if (skipPased)
                        ConsumeCharacter();
                    continue;
                }
            }
            ConsumeCharacter();
        }

        if (_currentState != ParseState.None && _currentState != ParseState.SkipToNextLine)
            AddError("Unexpected end of file!");

        if (_syntaxErrors.Count > 0)
            throw new TomlParseException(rootTable, _syntaxErrors);

        return rootTable;
    }

    private void ConsumeCharacter()
    {
        r_reader.Read();
        _column++;
    }

    /// <summary>
    /// 解析空字符
    /// </summary>
    /// <param name="c">字符</param>
    /// <param name="rootTable">原始表格</param>
    /// <param name="isFirstComment">是首个注释</param>
    /// <param name="latestComment">末尾注释</param>
    /// <returns>消耗字符并跳过为 <see langword="true"/> 只跳过为 <see langword="false"/> 不操作为 <see langword="null"/></returns>
    private bool? ParseNone(
        char c,
        TomlTable rootTable,
        ref bool isFirstComment,
        ref StringBuilder latestComment
    )
    {
        // Skip white space
        if (TomlSyntax.IsWhiteSpace(c))
            return true;

        if (TomlSyntax.IsNewLine(c))
        {
            // Check if there are any comments and so far no items being declared
            if (latestComment is not null && isFirstComment)
            {
                rootTable.Comment = latestComment.ToString().TrimEnd();
                latestComment = null!;
                isFirstComment = false;
            }

            if (TomlSyntax.IsLineBreak(c))
                AdvanceLine();

            return true;
        }

        // Start of a comment; ignore until newline
        if (c is TomlSyntax.COMMENT_SYMBOL)
        {
            latestComment ??= new StringBuilder();
            latestComment.AppendLine(ParseComment());
            AdvanceLine(1);
            return false;
        }

        // Encountered a non-comment value. The comment must belong to it (ignore possible newlines)!
        isFirstComment = false;

        if (c is TomlSyntax.TABLE_START_SYMBOL)
        {
            _currentState = ParseState.Table;
            return true;
        }

        if (TomlSyntax.IsBareKey(c) || TomlSyntax.IsQuoted(c))
        {
            _currentState = ParseState.KeyValuePair;
            return null;
        }
        else
        {
            AddError($"Unexpected character \"{c}\"");
            return false;
        }
    }

    /// <summary>
    /// 解析键值对
    /// </summary>
    /// <param name="currentTable">当前表格</param>
    /// <param name="keyParts">键列表</param>
    /// <param name="latestComment">最后注释</param>
    /// <returns>跳过为 <see langword="true"/> 不跳过为 <see langword="false"/></returns>
    private bool ParseKeyValuePair(
        TomlTable currentTable,
        List<string> keyParts,
        ref StringBuilder latestComment
    )
    {
        var keyValuePair = ReadKeyValuePair(keyParts);

        if (keyValuePair == null)
        {
            latestComment = null!;
            keyParts.Clear();

            if (_currentState != ParseState.None)
                AddError("Failed to parse key-value pair!");
            return true;
        }

        keyValuePair.Comment = latestComment?.ToString()?.TrimEnd()!;
        var inserted = InsertNode(keyValuePair, currentTable, keyParts);
        latestComment = null!;
        keyParts.Clear();
        if (inserted)
            _currentState = ParseState.SkipToNextLine;
        return true;
    }

    /// <summary>
    /// 解析表格
    /// </summary>
    /// <param name="c">字符</param>
    /// <param name="rootTable">原始表格</param>
    /// <param name="currentTable">当前表格</param>
    /// <param name="keyParts">键列表</param>
    /// <param name="isArrayTable">是数组表格</param>
    /// <param name="latestComment">末尾注释</param>
    /// <returns>消耗字符并跳过为 <see langword="true"/> 只跳过为 <see langword="false"/> 不操作为 <see langword="null"/></returns>
    private bool? ParseTable(
        char c,
        TomlTable rootTable,
        ref TomlTable currentTable,
        List<string> keyParts,
        ref bool isArrayTable,
        ref StringBuilder latestComment
    )
    {
        if (keyParts.Count == 0)
        {
            // We have array table
            if (c is TomlSyntax.TABLE_START_SYMBOL)
            {
                // Consume the character
                ConsumeChar();
                isArrayTable = true;
            }

            if (ReadKeyName(ref keyParts, TomlSyntax.TABLE_END_SYMBOL) is false)
            {
                keyParts.Clear();
                return false;
            }

            if (keyParts.Count == 0)
            {
                AddError("Table name is emtpy.");
                isArrayTable = false;
                latestComment = null!;
                keyParts.Clear();
            }

            return false;
        }

        if (c is TomlSyntax.TABLE_END_SYMBOL)
        {
            if (isArrayTable)
            {
                // Consume the ending bracket so we can peek the next character
                ConsumeChar();
                var nextChar = r_reader.Peek();
                if (nextChar < 0 || (char)nextChar != TomlSyntax.TABLE_END_SYMBOL)
                {
                    AddError($"Array table {".".Join(keyParts)} has only one closing bracket.");
                    keyParts.Clear();
                    isArrayTable = false;
                    latestComment = null!;
                    return false;
                }
            }

            currentTable = CreateTable(rootTable, keyParts, isArrayTable);
            if (currentTable is not null)
            {
                currentTable.IsInline = false;
                currentTable.Comment = latestComment?.ToString()?.TrimEnd()!;
            }

            keyParts.Clear();
            isArrayTable = false;
            latestComment = null!;

            if (currentTable == null)
            {
                if (_currentState != ParseState.None)
                    AddError("Error creating table array!");
                // Reset a node to root in order to try and continue parsing
                currentTable = rootTable;
                return false;
            }

            _currentState = ParseState.SkipToNextLine;
            return true;
        }

        if (keyParts.Count != 0)
        {
            AddError($"Unexpected character \"{c}\"");
            keyParts.Clear();
            isArrayTable = false;
            latestComment = null!;
        }
        return null;
    }

    /// <summary>
    /// 跳过下一行
    /// </summary>
    /// <param name="c">字符</param>
    /// <returns>消耗字符并跳过为 <see langword="true"/> 只跳过为 <see langword="false"/> 不操作为 <see langword="null"/></returns>
    private bool? ParseSkipToNextLine(char c)
    {
        if (TomlSyntax.IsWhiteSpace(c) || c is TomlSyntax.NEWLINE_CARRIAGE_RETURN_CHARACTER)
            return true;

        if (c is TomlSyntax.COMMENT_SYMBOL or TomlSyntax.NEWLINE_CHARACTER)
        {
            _currentState = ParseState.None;
            AdvanceLine();

            if (c is TomlSyntax.COMMENT_SYMBOL)
            {
                _column++;
                ParseComment();
                return false;
            }

            return true;
        }

        AddError($"Unexpected character \"{c}\" at the end of the line.");
        return null;
    }

    private bool AddError(string message, bool skipLine = true)
    {
        _syntaxErrors.Add(new TomlSyntaxException(message, _currentState, _line, _column));
        // Skip the whole _line in hope that it was only a single faulty value (and non-multiline one at that)
        if (skipLine)
        {
            r_reader.ReadLine();
            AdvanceLine(1);
        }
        _currentState = ParseState.None;
        return false;
    }

    private void AdvanceLine(int startCol = 0)
    {
        _line++;
        _column = startCol;
    }

    private int ConsumeChar()
    {
        _column++;
        return r_reader.Read();
    }

    #region Key-Value pair parsing

    /**
     * Reads a single key-value pair.
     * Assumes the cursor is at the first character that belong to the pair (including possible whitespace).
     * Consumes all characters that belong to the key and the value (ignoring possible trailing whitespace at the end).
     *
     * CreateClassExample:
     * foo = "bar"  ==> foo = "bar"
     * ^                           ^
     */

    private TomlNode ReadKeyValuePair(List<string> keyParts)
    {
        int cur;
        while ((cur = r_reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (TomlSyntax.IsQuoted(c) || TomlSyntax.IsBareKey(c))
            {
                if (keyParts.Count != 0)
                {
                    AddError("Encountered extra characters in key definition!");
                    return null!;
                }

                if (ReadKeyName(ref keyParts, TomlSyntax.KEY_VALUE_SEPARATOR) is false)
                    return null!;

                continue;
            }

            if (TomlSyntax.IsWhiteSpace(c))
            {
                ConsumeChar();
                continue;
            }

            if (c is TomlSyntax.KEY_VALUE_SEPARATOR)
            {
                ConsumeChar();
                return ReadValue();
            }

            AddError($"Unexpected character \"{c}\" in key name.");
            return null!;
        }

        return null!;
    }

    /**
     * Reads a single value.
     * Assumes the cursor is at the first character that belongs to the value (including possible starting whitespace).
     * Consumes all characters belonging to the value (ignoring possible trailing whitespace at the end).
     *
     * CreateClassExample:
     * "Test"  ==> "Test"
     * ^                 ^
     */

    private TomlNode ReadValue(bool skipNewlines = false)
    {
        int cur;
        while ((cur = r_reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (TomlSyntax.IsWhiteSpace(c))
            {
                ConsumeChar();
                continue;
            }

            if (c is TomlSyntax.COMMENT_SYMBOL)
            {
                AddError("No value found!");
                return null!;
            }

            if (TomlSyntax.IsNewLine(c))
            {
                if (skipNewlines)
                {
                    r_reader.Read();
                    AdvanceLine(1);
                    continue;
                }

                AddError("Encountered a newline when expecting a value!");
                return null!;
            }

            if (TomlSyntax.IsQuoted(c))
            {
                var isMultiline = IsTripleQuote(c, out var excess);

                // Error occurred in triple quote parsing
                if (_currentState is ParseState.None)
                    return null!;

                var value = isMultiline
                    ? ReadQuotedValueMultiLine(c)
                    : ReadQuotedValueSingleLine(c, excess);

                if (value is null)
                    return null!;

                return new TomlString
                {
                    Value = value,
                    IsMultiline = isMultiline,
                    PreferLiteral = c is TomlSyntax.LITERAL_STRING_SYMBOL
                };
            }

            return c switch
            {
                TomlSyntax.INLINE_TABLE_START_SYMBOL => ReadInlineTable(),
                TomlSyntax.ARRAY_START_SYMBOL => ReadArray(),
                var _ => ReadTomlValue()
            };
        }

        return null!;
    }

    /**
     * Reads a single key tomlFile.
     * Assumes the cursor is at the first character belonging to the key (with possible trailing whitespace if `skipWhitespace = true`).
     * Consumes all the characters until the `until` character is met (but does not consume the character itself).
     *
     * CreateClassExample 1:
     * foo.bar  ==>  foo.bar           (`skipWhitespace = false`, `until = ' '`)
     * ^                    ^
     *
     * CreateClassExample 2:
     * [ foo . bar ] ==>  [ foo . bar ]     (`skipWhitespace = true`, `until = ']'`)
     * ^                             ^
     */

    private bool ReadKeyName(ref List<string> parts, char until)
    {
        var buffer = new StringBuilder();
        var quoted = false;
        var prevWasSpace = false;
        int cur;
        while ((cur = r_reader.Peek()) >= 0)
        {
            var c = (char)cur;

            // Reached the final character
            if (c == until)
                break;

            if (TomlSyntax.IsWhiteSpace(c))
            {
                prevWasSpace = true;
                ConsumeCharacter();
                continue;
            }

            if (buffer.Length == 0)
                prevWasSpace = false;

            if (c is TomlSyntax.SUBKEY_SEPARATOR)
            {
                if (buffer.Length == 0 && quoted is false)
                    return AddError($"Found an extra subkey separator in {".".Join(parts)}...");

                parts.Add(buffer.ToString());
                buffer.Length = 0;
                quoted = false;
                prevWasSpace = false;
                ConsumeCharacter();
                continue;
            }

            if (prevWasSpace)
                return AddError("Invalid spacing in key name");

            if (TomlSyntax.IsQuoted(c))
            {
                if (quoted)

                    return AddError("Expected a subkey separator but got extra data instead!");

                if (buffer.Length != 0)
                    return AddError("Encountered a quote in the middle of subkey name!");

                // Consume the quote character and read the key tomlFile
                _column++;
                buffer.Append(ReadQuotedValueSingleLine((char)r_reader.Read()));
                quoted = true;
                continue;
            }

            if (TomlSyntax.IsBareKey(c))
            {
                buffer.Append(c);
                ConsumeCharacter();
                continue;
            }

            // If we see an invalid symbol, let the next parser handle it
            break;
        }

        if (buffer.Length == 0 && quoted is false)
            return AddError($"Found an extra subkey separator in {".".Join(parts)}...");

        parts.Add(buffer.ToString());

        return true;
    }

    #endregion

    #region Non-string value parsing

    /**
     * Reads the whole raw value until the first non-value character is encountered.
     * Assumes the cursor start position at the first value character and consumes all characters that may be related to the value.
     * CreateClassExample:
     *
     * 1_0_0_0  ==>  1_0_0_0
     * ^                    ^
     */

    private string ReadRawValue()
    {
        var result = new StringBuilder();
        int cur;
        while ((cur = r_reader.Peek()) >= 0)
        {
            var c = (char)cur;
            if (
                c is TomlSyntax.COMMENT_SYMBOL
                || TomlSyntax.IsNewLine(c)
                || TomlSyntax.IsValueSeparator(c)
            )
                break;
            result.Append(c);
            ConsumeChar();
        }

        // Replace trim with manual space counting?
        return result.ToString().Trim();
    }

    /**
     * Reads and parses a non-string, non-composite TOML value.
     * Assumes the cursor at the first character that is related to the value (with possible spaces).
     * Consumes all the characters that are related to the value.
     *
     * CreateClassExample
     * 1_0_0_0 # This is a comment
     * <newline>
     *     ==>  1_0_0_0 # This is a comment
     *     ^                                                  ^
     * </newline>
     **/

    private TomlNode ReadTomlValue()
    {
        var value = ReadRawValue();
        TomlNode node = value switch
        {
            var v when TomlSyntax.IsBoolean(v) => bool.Parse(v),
            var v when TomlSyntax.IsNaN(v) => double.NaN,
            var v when TomlSyntax.IsPosInf(v) => double.PositiveInfinity,
            var v when TomlSyntax.IsNegInf(v) => double.NegativeInfinity,
            var v when TomlSyntax.IsInteger(v)
                => long.Parse(
                    value.RemoveAll(TomlSyntax.INT_NUMBER_SEPARATOR),
                    CultureInfo.InvariantCulture
                ),
            var v when TomlSyntax.IsFloat(v)
                => double.Parse(
                    value.RemoveAll(TomlSyntax.INT_NUMBER_SEPARATOR),
                    CultureInfo.InvariantCulture
                ),
            var v when TomlSyntax.IsIntegerWithBase(v, out var numberBase)
                => new TomlInteger
                {
                    Value = Convert.ToInt64(
                        value[2..].RemoveAll(TomlSyntax.INT_NUMBER_SEPARATOR),
                        numberBase
                    ),
                    IntegerBase = (TomlInteger.Base)numberBase
                },
            var _ => null!
        };
        if (node is not null)
            return node;

        // Normalize by removing space separator
        value = value.Replace(TomlSyntax.RFC3339EmptySeparator, TomlSyntax.ISO861Separator);
        if (
            StringUtils.TryParseDateTime<DateTime>(
                value,
                TomlSyntax.RFC3339LocalDateTimeFormats,
                DateTimeStyles.AssumeLocal,
                DateTime.TryParseExact,
                out var dateTimeResult,
                out var precision
            )
        )
            return new TomlDateTimeLocal { Value = dateTimeResult, SecondsPrecision = precision };

        if (
            DateTime.TryParseExact(
                value,
                TomlSyntax.LocalDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal,
                out dateTimeResult
            )
        )
            return new TomlDateTimeLocal
            {
                Value = dateTimeResult,
                Style = TomlDateTimeLocal.DateTimeStyle.Date
            };

        if (
            StringUtils.TryParseDateTime(
                value,
                TomlSyntax.RFC3339LocalTimeFormats,
                DateTimeStyles.AssumeLocal,
                DateTime.TryParseExact,
                out dateTimeResult,
                out precision
            )
        )
            return new TomlDateTimeLocal
            {
                Value = dateTimeResult,
                Style = TomlDateTimeLocal.DateTimeStyle.Time,
                SecondsPrecision = precision
            };

        if (
            StringUtils.TryParseDateTime<DateTimeOffset>(
                value,
                TomlSyntax.RFC3339Formats,
                DateTimeStyles.None,
                DateTimeOffset.TryParseExact,
                out var dateTimeOffsetResult,
                out precision
            )
        )
            return new TomlDateTimeOffset
            {
                Value = dateTimeOffsetResult,
                SecondsPrecision = precision
            };

        AddError($"Value \"{value}\" is not a valid TOML value!");
        return null!;
    }

    /**
     * Reads an array value.
     * Assumes the cursor is at the start of the array definition. Reads all character until the array closing bracket.
     *
     * CreateClassExample:
     * [1, 2, 3]  ==>  [1, 2, 3]
     * ^                        ^
     */

    private TomlArray ReadArray()
    {
        // Consume the start of array character
        ConsumeChar();
        var result = new TomlArray();
        TomlNode currentValue = null!;
        var expectValue = true;

        int cur;
        while ((cur = r_reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (c is TomlSyntax.ARRAY_END_SYMBOL)
            {
                ConsumeChar();
                break;
            }

            if (c is TomlSyntax.COMMENT_SYMBOL)
            {
                r_reader.ReadLine();
                AdvanceLine(1);
                continue;
            }

            if (TomlSyntax.IsWhiteSpace(c) || TomlSyntax.IsNewLine(c))
            {
                if (TomlSyntax.IsLineBreak(c))
                    AdvanceLine();
                ConsumeChar();
                continue;
            }

            if (c is TomlSyntax.ITEM_SEPARATOR)
            {
                if (currentValue == null)
                {
                    AddError("Encountered multiple value separators");
                    return null!;
                }

                result.Add(currentValue);
                currentValue = null!;
                expectValue = true;
                ConsumeChar();
                continue;
            }

            if (expectValue is false)
            {
                AddError("Missing separator between values");
                return null!;
            }
            currentValue = ReadValue(true);
            if (currentValue == null)
            {
                if (_currentState != ParseState.None)
                    AddError("Failed to determine and parse a value!");
                return null!;
            }
            expectValue = false;
        }

        if (currentValue is not null)
            result.Add(currentValue);
        return result;
    }

    /**
     * Reads an inline table.
     * Assumes the cursor is at the start of the table definition. Reads all character until the table closing bracket.
     *
     * CreateClassExample:
     * { Test = "foo", value = 1 }  ==>  { Test = "foo", value = 1 }
     * ^                                                            ^
     */

    private TomlNode ReadInlineTable()
    {
        ConsumeChar();
        var result = new TomlTable { IsInline = true };
        TomlNode currentValue = null!;
        var separator = false;
        var keyParts = new List<string>();
        int cur;
        while ((cur = r_reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (c is TomlSyntax.INLINE_TABLE_END_SYMBOL)
            {
                ConsumeChar();
                break;
            }

            if (c is TomlSyntax.COMMENT_SYMBOL)
            {
                AddError("Incomplete inline table definition!");
                return null!;
            }

            if (TomlSyntax.IsNewLine(c))
            {
                AddError("Inline tables are only allowed to be on single line");
                return null!;
            }

            if (TomlSyntax.IsWhiteSpace(c))
            {
                ConsumeChar();
                continue;
            }

            if (c is TomlSyntax.ITEM_SEPARATOR)
            {
                if (currentValue == null)
                {
                    AddError("Encountered multiple value separators in inline table!");
                    return null!;
                }

                if (InsertNode(currentValue, result, keyParts) is false)
                    return null!;
                keyParts.Clear();
                currentValue = null!;
                separator = true;
                ConsumeChar();
                continue;
            }

            separator = false;
            currentValue = ReadKeyValuePair(keyParts);
        }

        if (separator)
        {
            AddError("Trailing commas are not allowed in inline tables.");
            return null!;
        }

        if (currentValue is not null && !InsertNode(currentValue, result, keyParts))
            return null!;

        return result;
    }

    #endregion

    #region String parsing

    /**
     * Checks if the string value a multiline string (i.ex. a triple quoted string).
     * Assumes the cursor is at the first quote character. Consumes the least amount of characters needed to determine if the string is multiline.
     *
     * If the result is false, returns the consumed character through the `excess` variable.
     *
     * CreateClassExample 1:
     * """Test"""  ==>  """Test"""
     * ^                   ^
     *
     * CreateClassExample 2:
     * "Test"  ==>  "Test"         (doesn't return the first quote)
     * ^             ^
     *
     * CreateClassExample 3:
     * ""  ==>  ""        (returns the extra `"` through the `excess` variable)
     * ^          ^
     */

    private bool IsTripleQuote(char quote, out char excess)
    {
        // Copypasta, but it's faster...

        int cur;
        // Consume the first quote
        ConsumeChar();
        if ((cur = r_reader.Peek()) < 0)
        {
            excess = '\0';
            return AddError("Unexpected end of file!");
        }

        if ((char)cur != quote)
        {
            excess = '\0';
            return false;
        }

        // Consume the second quote
        excess = (char)ConsumeChar();
        if ((cur = r_reader.Peek()) < 0 || (char)cur != quote)
            return false;

        // Consume the final quote
        ConsumeChar();
        excess = '\0';
        return true;
    }

    /**
     * A convenience method to process a single character within a quote.
     */

    private bool ProcessQuotedValueCharacter(
        char quote,
        bool isNonLiteral,
        char c,
        StringBuilder sb,
        ref bool escaped
    )
    {
        if (TomlSyntax.MustBeEscaped(c))
            return AddError($"The character U+{(int)c:X8} must be escaped in a string!");

        if (escaped)
        {
            sb.Append(c);
            escaped = false;
            return false;
        }

        if (c == quote)
            return true;
        if (isNonLiteral && c is TomlSyntax.ESCAPE_SYMBOL)
            escaped = true;
        if (c is TomlSyntax.NEWLINE_CHARACTER)
            return AddError("Encountered newline in single line string!");

        sb.Append(c);
        return false;
    }

    /**
     * Reads a single-_line string.
     * Assumes the cursor is at the first character that belongs to the string.
     * Consumes all characters that belong to the string (including the closing quote).
     *
     * CreateClassExample:
     * "Test"  ==>  "Test"
     * ^                 ^
     */

    private string ReadQuotedValueSingleLine(char quote, char initialData = '\0')
    {
        var isNonLiteral = quote is TomlSyntax.BASIC_STRING_SYMBOL;
        var sb = new StringBuilder();
        var escaped = false;

        if (initialData != '\0')
        {
            var shouldReturn = ProcessQuotedValueCharacter(
                quote,
                isNonLiteral,
                initialData,
                sb,
                ref escaped
            );
            if (_currentState is ParseState.None)
                return null!;
            if (shouldReturn)
                if (isNonLiteral)
                {
                    if (sb.ToString().TryUnescape(out var res, out var ex))
                        return res;
                    AddError(ex.Message);
                    return null!;
                }
                else
                    return sb.ToString();
        }

        int cur;
        var readDone = false;
        while ((cur = r_reader.Read()) >= 0)
        {
            // Consume the character
            _column++;
            var c = (char)cur;
            readDone = ProcessQuotedValueCharacter(quote, isNonLiteral, c, sb, ref escaped);
            if (readDone)
            {
                if (_currentState is ParseState.None)
                    return null!;
                break;
            }
        }

        if (readDone is false)
        {
            AddError("Unclosed string.");
            return null!;
        }

        if (isNonLiteral is false)
            return sb.ToString();
        if (sb.ToString().TryUnescape(out var unescaped, out var unescapedEx))
            return unescaped;
        AddError(unescapedEx.Message);
        return null!;
    }

    /**
     * Reads a multiline string.
     * Assumes the cursor is at the first character that belongs to the string.
     * Consumes all characters that belong to the string and the three closing quotes.
     *
     * CreateClassExample:
     * """Test"""  ==>  """Test"""
     * ^                       ^
     */

    private string ReadQuotedValueMultiLine(char quote)
    {
        var isBasic = quote is TomlSyntax.BASIC_STRING_SYMBOL;
        var sb = new StringBuilder();
        var escaped = false;
        var skipWhitespace = false;
        var skipWhitespaceLineSkipped = false;
        var quotesEncountered = 0;
        var first = true;
        int cur;
        while ((cur = ConsumeChar()) >= 0)
        {
            var c = (char)cur;
            if (TomlSyntax.MustBeEscaped(c, true))
            {
                AddError($"The character U+{(int)c:X8} must be escaped!");
                return null!;
            }
            // Trim the first newline
            if (first && TomlSyntax.IsNewLine(c))
            {
                if (TomlSyntax.IsLineBreak(c))
                    first = false;
                else
                    AdvanceLine();
                continue;
            }

            first = false;
            //?TODO: Reuse ProcessQuotedValueCharacter
            // Skip the current character if it is going to be escaped later
            if (escaped)
            {
                sb.Append(c);
                escaped = false;
                continue;
            }

            // If we are currently skipping empty spaces, skip
            if (skipWhitespace)
            {
                if (TomlSyntax.IsEmptySpace(c))
                {
                    if (TomlSyntax.IsLineBreak(c))
                    {
                        skipWhitespaceLineSkipped = true;
                        AdvanceLine();
                    }
                    continue;
                }

                if (skipWhitespaceLineSkipped is false)
                {
                    AddError("Non-whitespace character after trim marker.");
                    return null!;
                }

                skipWhitespaceLineSkipped = false;
                skipWhitespace = false;
            }

            // If we encounter an escape sequence...
            if (isBasic && c is TomlSyntax.ESCAPE_SYMBOL)
            {
                var next = r_reader.Peek();
                var nc = (char)next;
                if (next >= 0)
                {
                    // ...and the next char is empty space, we must skip all whitespaces
                    if (TomlSyntax.IsEmptySpace(nc))
                    {
                        skipWhitespace = true;
                        continue;
                    }

                    // ...and we have \" or \, skip the character
                    if (nc == quote || nc is TomlSyntax.ESCAPE_SYMBOL)
                        escaped = true;
                }
            }

            // Count the consecutive quotes
            if (c == quote)
                quotesEncountered++;
            else
                quotesEncountered = 0;

            // If the are three quotes, count them as closing quotes
            if (quotesEncountered == 3)
                break;

            sb.Append(c);
        }

        // TOML actually allows to have five ending quotes like
        // """"" => "" belong to the string + """ is the actual ending
        quotesEncountered = 0;
        while ((cur = r_reader.Peek()) >= 0)
        {
            var c = (char)cur;
            if (c == quote && ++quotesEncountered < 3)
            {
                sb.Append(c);
                ConsumeChar();
            }
            else
                break;
        }

        // Remove last two quotes (third one wasn't included by default)
        sb.Length -= 2;
        if (isBasic is false)
            return sb.ToString();
        if (sb.ToString().TryUnescape(out var res, out var ex))
            return res;
        AddError(ex.Message);
        return null!;
    }

    #endregion

    #region Node creation

    private bool InsertNode(TomlNode node, TomlNode root, IList<string> path)
    {
        var latestNode = root;
        if (path.Count > 1)
            for (var index = 0; index < path.Count - 1; index++)
            {
                var subkey = path[index];
                if (latestNode.TryGetNode(subkey, out var currentNode))
                {
                    if (currentNode.HasValue)
                        return AddError(
                            $"The key {".".Join(path)} already has a value assigned to it!"
                        );
                }
                else
                {
                    currentNode = new TomlTable();
                    latestNode[subkey] = currentNode;
                }

                latestNode = currentNode;
                if (latestNode is TomlTable { IsInline: true })
                    return AddError(
                        $"Cannot assign {".".Join(path)} because it will edit an immutable table."
                    );
            }

        if (latestNode.HasKey(path[path.Count - 1]))
            return AddError($"The key {".".Join(path)} is already defined!");
        latestNode[path[path.Count - 1]] = node;
        node.CollapseLevel = path.Count - 1;
        return true;
    }

    private TomlTable CreateTable(TomlNode root, IList<string> path, bool arrayTable)
    {
        if (path.Count == 0)
            return null!;
        var latestNode = root;
        for (var index = 0; index < path.Count; index++)
        {
            var subkey = path[index];

            if (latestNode.TryGetNode(subkey, out var node))
            {
                if (node.IsTomlArray && arrayTable)
                {
                    var arr = (TomlArray)node;

                    if (arr.IsTableArray is false)
                    {
                        AddError(
                            $"The array {".".Join(path)} cannot be redefined as an array table!"
                        );
                        return null!;
                    }

                    if (index == path.Count - 1)
                    {
                        latestNode = new TomlTable();
                        arr.Add(latestNode);
                        break;
                    }

                    latestNode = arr[arr.ChildrenCount - 1];
                    continue;
                }

                if (node is TomlTable { IsInline: true })
                {
                    AddError(
                        $"Cannot create table {".".Join(path)} because it will edit an immutable table."
                    );
                    return null!;
                }

                if (node.HasValue)
                {
                    if (node is not TomlArray { IsTableArray: true } array)
                    {
                        AddError($"The key {".".Join(path)} has a value assigned to it!");
                        return null!;
                    }

                    latestNode = array[array.ChildrenCount - 1];
                    continue;
                }

                if (index == path.Count - 1)
                {
                    if (arrayTable && !node.IsTomlArray)
                    {
                        AddError(
                            $"The table {".".Join(path)} cannot be redefined as an array table!"
                        );
                        return null!;
                    }

                    if (node is TomlTable { isImplicit: false })
                    {
                        AddError($"The table {".".Join(path)} is defined multiple times!");
                        return null!;
                    }
                }
            }
            else
            {
                if (index == path.Count - 1 && arrayTable)
                {
                    var table = new TomlTable();
                    var arr = new TomlArray { IsTableArray = true };
                    arr.Add(table);
                    latestNode[subkey] = arr;
                    latestNode = table;
                    break;
                }

                node = new TomlTable { isImplicit = true };
                latestNode[subkey] = node;
            }

            latestNode = node;
        }

        var result = (TomlTable)latestNode;
        result.isImplicit = false;
        return result;
    }

    #endregion

    #region Misc parsing

    private string ParseComment()
    {
        ConsumeChar();
        var commentLine = r_reader.ReadLine()?.Trim() ?? string.Empty;
        if (commentLine.Any(ch => TomlSyntax.MustBeEscaped(ch)))
            AddError("Comment must not contain control characters other than tab.", false);
        return commentLine;
    }

    #endregion
}

#endregion

/// <summary>
/// TOML
/// </summary>
public static class TOML
{
    /// <summary>
    /// 强制ASCII编码
    /// </summary>
    public static bool ForceASCII { get; set; } = false;

    /// <summary>
    /// 从文本读取器解析
    /// </summary>
    /// <param name="reader">读取器</param>
    /// <returns>解析完成的Toml表格</returns>
    public static TomlTable Parse(TextReader reader)
    {
        using var parser = new TOMLParser(reader) { ForceASCII = ForceASCII };
        return parser.Parse();
    }

    /// <summary>
    /// 从字符串解析
    /// </summary>
    /// <param name="tomlData">Toml数据</param>
    /// <returns>解析完成的Toml表格</returns>
    public static TomlTable Parse(string tomlData)
    {
        using var sr = new StringReader(tomlData);
        using var parser = new TOMLParser(sr) { ForceASCII = ForceASCII };
        return parser.Parse();
    }

    /// <summary>
    /// 从文件解析
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    /// <returns>解析完成的Toml表格</returns>
    public static TomlTable ParseFromFile(string tomlFile)
    {
        using var reader = File.OpenText(tomlFile);
        using var parser = new TOMLParser(reader) { ForceASCII = ForceASCII };
        return parser.Parse();
    }
}

#region Exception Types

/// <summary>
/// Toml格式化错误
/// </summary>
public class TomlFormatException : Exception
{
    /// <summary>
    /// Toml格式化错误
    /// </summary>
    /// <param name="message">信息</param>
    public TomlFormatException(string message)
        : base(message) { }
}

/// <summary>
/// Toml解析错误
/// </summary>
public class TomlParseException : Exception
{
    /// <summary>
    /// 解析过的表格
    /// </summary>
    public TomlTable ParsedTable { get; }

    /// <summary>
    /// 语法错误
    /// </summary>
    public IEnumerable<TomlSyntaxException> SyntaxErrors { get; }

    /// <summary>
    /// Toml解析错误
    /// </summary>
    /// <param name="parsed">解析过的表格</param>
    /// <param name="exceptions">语法错误</param>
    public TomlParseException(TomlTable parsed, IEnumerable<TomlSyntaxException> exceptions)
        : base("TOML file contains format errors")
    {
        ParsedTable = parsed;
        SyntaxErrors = exceptions;
    }
}

/// <summary>
/// Toml语法错误
/// </summary>
public class TomlSyntaxException : Exception
{
    /// <summary>
    /// 解析状态
    /// </summary>
    public TOMLParser.ParseState ParseState { get; }

    /// <summary>
    /// 行
    /// </summary>
    public int Line { get; }

    /// <summary>
    /// 列
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Toml语法错误
    /// </summary>
    /// <param name="message">信息</param>
    /// <param name="state">解析状态</param>
    /// <param name="line">行</param>
    /// <param name="column">列</param>
    public TomlSyntaxException(string message, TOMLParser.ParseState state, int line, int column)
        : base(message)
    {
        ParseState = state;
        Line = line;
        Column = column;
    }
}

#endregion

#region Parse utilities

/// <summary>
/// Toml语法
/// </summary>
internal static class TomlSyntax
{
    #region Type Patterns

    /// <summary>
    /// Boolean真值
    /// </summary>
    public const string TRUE_VALUE = "true";

    /// <summary>
    /// Boolean假值
    /// </summary>
    public const string FALSE_VALUE = "false";

    /// <summary>
    /// 无效值
    /// </summary>
    public const string NAN_VALUE = "nan";

    /// <summary>
    /// 正无效值
    /// </summary>
    public const string POS_NAN_VALUE = "+nan";

    /// <summary>
    /// 负无效值
    /// </summary>
    public const string NEG_NAN_VALUE = "-nan";

    /// <summary>
    /// 无穷
    /// </summary>
    public const string INF_VALUE = "inf";

    /// <summary>
    /// 正无穷
    /// </summary>
    public const string POS_INF_VALUE = "+inf";

    /// <summary>
    /// 负无穷
    /// </summary>
    public const string NEG_INF_VALUE = "-inf";

    /// <summary>
    /// 是布尔类型
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>是为 <see langword="true"/> 否为 <see langword="false"/></returns>
    public static bool IsBoolean(string s) => s is TRUE_VALUE or FALSE_VALUE;

    /// <summary>
    /// 是正无穷
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>是为 <see langword="true"/> 否为 <see langword="false"/></returns>
    public static bool IsPosInf(string s) => s is INF_VALUE or POS_INF_VALUE;

    /// <summary>
    /// 是负无穷
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>是为 <see langword="true"/> 否为 <see langword="false"/></returns>
    public static bool IsNegInf(string s) => s == NEG_INF_VALUE;

    /// <summary>
    /// 是无效值
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>是为 <see langword="true"/> 否为 <see langword="false"/></returns>
    public static bool IsNaN(string s) => s is NAN_VALUE or POS_NAN_VALUE or NEG_NAN_VALUE;

    /// <summary>
    /// 是整型
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>是为 <see langword="true"/> 否为 <see langword="false"/></returns>
    public static bool IsInteger(string s) => IntegerPattern.IsMatch(s);

    /// <summary>
    /// 是浮点型
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>是为 <see langword="true"/> 否为 <see langword="false"/></returns>
    public static bool IsFloat(string s) => FloatPattern.IsMatch(s);

    /// <summary>
    /// 是进位制整型
    /// </summary>
    /// <param name="s">字符串</param>
    /// <param name="numberBase">进位制</param>
    /// <returns>是为 <see langword="true"/> 否为 <see langword="false"/></returns>
    public static bool IsIntegerWithBase(string s, out int numberBase)
    {
        numberBase = 10;
        var match = BasedIntegerPattern.Match(s);
        if (match.Success is false)
            return false;
        IntegerBases.TryGetValue(match.Groups["base"].Value, out numberBase);
        return true;
    }

    /**
     * A pattern to verify the integer value according to the TOML specification.
     */

    public static readonly Regex IntegerPattern =
        new(@"^(\+|-)?(?!_)(0|(?!0)(_?\d)*)$", RegexOptions.Compiled);

    /**
     * A pattern to verify a special 0x, 0o and 0b forms of an integer according to the TOML specification.
     */

    public static readonly Regex BasedIntegerPattern =
        new(
            @"^0(?<base>x|b|o)(?!_)(_?[0-9A-F])*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

    /**
     * A pattern to verify the float value according to the TOML specification.
     */

    public static readonly Regex FloatPattern =
        new(
            @"^(\+|-)?(?!_)(0|(?!0)(_?\d)+)(((e(\+|-)?(?!_)(_?\d)+)?)|(\.(?!_)(_?\d)+(e(\+|-)?(?!_)(_?\d)+)?))$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

    /**
     * A helper dictionary to map TOML base codes into the radii.
     */

    public static readonly Dictionary<string, int> IntegerBases =
        new()
        {
            ["x"] = 16,
            ["o"] = 8,
            ["b"] = 2
        };

    /**
     * A helper dictionary to map non-decimal bases to their TOML identifiers
     */

    public static readonly Dictionary<int, string> BaseIdentifiers =
        new()
        {
            [2] = "b",
            [8] = "o",
            [16] = "x"
        };

    public const string RFC3339EmptySeparator = " ";
    public const string ISO861Separator = "T";
    public const string ISO861ZeroZone = "+00:00";
    public const string RFC3339ZeroZone = "Z";

    /**
     * Valid date formats with timezone as per RFC3339.
     */

    public static readonly string[] RFC3339Formats =
    {
        "yyyy'-'MM-ddTHH':'mm':'ssK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'ffK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'ffffK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffffK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'ffffffK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffffffK"
    };

    /**
     * Valid date formats without timezone (assumes local) as per RFC3339.
     */

    public static readonly string[] RFC3339LocalDateTimeFormats =
    {
        "yyyy'-'MM-ddTHH':'mm':'ss",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'f",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'ff",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fff",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'ffff",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffff",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'ffffff",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffffff"
    };

    /**
     * Valid full date format as per TOML spec.
     */
    public static readonly string LocalDateFormat = "yyyy'-'MM'-'dd";

    /**
     * Valid time formats as per TOML spec.
     */

    public static readonly string[] RFC3339LocalTimeFormats =
    {
        "HH':'mm':'ss",
        "HH':'mm':'ss'.'f",
        "HH':'mm':'ss'.'ff",
        "HH':'mm':'ss'.'fff",
        "HH':'mm':'ss'.'ffff",
        "HH':'mm':'ss'.'fffff",
        "HH':'mm':'ss'.'ffffff",
        "HH':'mm':'ss'.'fffffff"
    };

    #endregion

    #region Character definitions

    public const char ARRAY_END_SYMBOL = ']';
    public const char ITEM_SEPARATOR = ',';
    public const char ARRAY_START_SYMBOL = '[';
    public const char BASIC_STRING_SYMBOL = '\"';
    public const char COMMENT_SYMBOL = '#';
    public const char ESCAPE_SYMBOL = '\\';
    public const char KEY_VALUE_SEPARATOR = '=';
    public const char NEWLINE_CARRIAGE_RETURN_CHARACTER = '\r';
    public const char NEWLINE_CHARACTER = '\n';
    public const char SUBKEY_SEPARATOR = '.';
    public const char TABLE_END_SYMBOL = ']';
    public const char TABLE_START_SYMBOL = '[';
    public const char INLINE_TABLE_START_SYMBOL = '{';
    public const char INLINE_TABLE_END_SYMBOL = '}';
    public const char LITERAL_STRING_SYMBOL = '\'';
    public const char INT_NUMBER_SEPARATOR = '_';

    public static readonly char[] NewLineCharacters =
    {
        NEWLINE_CHARACTER,
        NEWLINE_CARRIAGE_RETURN_CHARACTER
    };

    public static bool IsQuoted(char c) => c is BASIC_STRING_SYMBOL or LITERAL_STRING_SYMBOL;

    public static bool IsWhiteSpace(char c) => c is ' ' or '\t';

    public static bool IsNewLine(char c) =>
        c is NEWLINE_CHARACTER or NEWLINE_CARRIAGE_RETURN_CHARACTER;

    public static bool IsLineBreak(char c) => c == NEWLINE_CHARACTER;

    public static bool IsEmptySpace(char c) => IsWhiteSpace(c) || IsNewLine(c);

    public static bool IsBareKey(char c) =>
        c is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '_' or '-';

    public static bool MustBeEscaped(char c, bool allowNewLines = false)
    {
        var result =
            c
                is >= '\u0000'
                    and <= '\u0008'
                    or '\u000b'
                    or '\u000c'
                    or >= '\u000e'
                    and <= '\u001f'
                    or '\u007f';
        if (allowNewLines is false)
            result |= c is >= '\u000a' and <= '\u000e';
        return result;
    }

    public static bool IsValueSeparator(char c) =>
        c is ITEM_SEPARATOR or ARRAY_END_SYMBOL or INLINE_TABLE_END_SYMBOL;

    #endregion
}

/// <summary>
/// 字符串工具
/// </summary>
internal static class StringUtils
{
    /// <summary>
    /// 转换为键
    /// </summary>
    /// <param name="key">值</param>
    /// <returns></returns>
    public static string AsKey(this string key)
    {
        var quote = key == string.Empty || key.Any(c => !TomlSyntax.IsBareKey(c));
        return quote is false
            ? key
            : $"{TomlSyntax.BASIC_STRING_SYMBOL}{key.Escape()}{TomlSyntax.BASIC_STRING_SYMBOL}";
    }

    /// <summary>
    /// 加入值
    /// </summary>
    /// <param name="self"></param>
    /// <param name="subItems"></param>
    /// <returns></returns>
    public static string Join(this string self, IEnumerable<string> subItems)
    {
        var sb = new StringBuilder();
        var first = true;

        foreach (var subItem in subItems)
        {
            if (first is false)
                sb.Append(self);
            first = false;
            sb.Append(subItem);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 尝试解析日期时间委托
    /// </summary>
    /// <typeparam name="T">日期时间</typeparam>
    /// <param name="s">字符串</param>
    /// <param name="format">格式</param>
    /// <param name="ci">样式供应器</param>
    /// <param name="dts">日期时间风格</param>
    /// <param name="dt">日期时间</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public delegate bool TryDateParseDelegate<T>(
        string s,
        string format,
        IFormatProvider ci,
        DateTimeStyles dts,
        out T dt
    );

    /// <summary>
    /// 尝试解析日期时间
    /// </summary>
    /// <typeparam name="T">日期时间</typeparam>
    /// <param name="s">字符串</param>
    /// <param name="formats">格式</param>
    /// <param name="styles">日期时间格式</param>
    /// <param name="parser">尝试解析委托</param>
    /// <param name="dateTime">日期时间</param>
    /// <param name="parsedFormat">解析格式</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public static bool TryParseDateTime<T>(
        string s,
        string[] formats,
        DateTimeStyles styles,
        TryDateParseDelegate<T> parser,
        out T dateTime,
        out int parsedFormat
    )
    {
        parsedFormat = 0;
        dateTime = default!;
        for (var i = 0; i < formats.Length; i++)
        {
            var format = formats[i];
            if (parser(s, format, CultureInfo.InvariantCulture, styles, out dateTime) is false)
                continue;
            parsedFormat = i;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 转换为注释
    /// </summary>
    /// <param name="self">文本</param>
    /// <param name="tw">文本写入器</param>
    public static void AsComment(this string self, TextWriter tw)
    {
        foreach (var line in self.Split(TomlSyntax.NEWLINE_CHARACTER))
            tw.WriteLine($"{TomlSyntax.COMMENT_SYMBOL} {line.Trim()}");
    }

    /// <summary>
    /// 删除所有指定字符
    /// </summary>
    /// <param name="txt">文本</param>
    /// <param name="toRemove">指定字符</param>
    /// <returns>完成的字符串</returns>
    public static string RemoveAll(this string txt, char toRemove)
    {
        var sb = new StringBuilder(txt.Length);
        foreach (var c in txt.Where(c => c != toRemove))
            sb.Append(c);
        return sb.ToString();
    }

    /// <summary>
    /// 编码
    /// </summary>
    /// <param name="txt">文本</param>
    /// <param name="escapeNewlines">编码至新行</param>
    /// <returns>编码完成的字符串</returns>
    public static string Escape(this string txt, bool escapeNewlines = true)
    {
        var stringBuilder = new StringBuilder(txt.Length + 2);
        for (var i = 0; i < txt.Length; i++)
        {
            var c = txt[i];

            static string CodePoint(string txt, ref int i, char c) =>
                char.IsSurrogatePair(txt, i)
                    ? $"\\U{char.ConvertToUtf32(txt, i++):X8}"
                    : $"\\u{(ushort)c:X4}";

            stringBuilder.Append(
                c switch
                {
                    '\b' => @"\b",
                    '\t' => @"\t",
                    '\n' when escapeNewlines => @"\n",
                    '\f' => @"\f",
                    '\r' when escapeNewlines => @"\r",
                    '\\' => @"\\",
                    '\"' => @"\""",
                    var _
                        when TomlSyntax.MustBeEscaped(c, escapeNewlines is false)
                            || TOML.ForceASCII && c > sbyte.MaxValue
                        => CodePoint(txt, ref i, c),
                    var _ => c
                }
            );
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 尝试解码
    /// </summary>
    /// <param name="txt">文本</param>
    /// <param name="unescaped">解码字符串</param>
    /// <param name="exception">异常</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public static bool TryUnescape(this string txt, out string unescaped, out Exception exception)
    {
        try
        {
            exception = null!;
            unescaped = txt.Unescape();
            return true;
        }
        catch (Exception ex)
        {
            exception = ex;
            unescaped = null!;
            return false;
        }
    }

    /// <summary>
    /// 字符串解码
    /// </summary>
    /// <param name="txt">文本</param>
    /// <returns>解码后的字符串</returns>
    /// <exception cref="Exception">没码</exception>
    public static string Unescape(this string txt)
    {
        if (string.IsNullOrWhiteSpace(txt))
            return txt;
        var stringBuilder = new StringBuilder(txt.Length);
        for (var i = 0; i < txt.Length; )
        {
            var num = txt.IndexOf('\\', i);
            var next = num + 1;
            if (num < 0 || num == txt.Length - 1)
                num = txt.Length;
            stringBuilder.Append(txt, i, num - i);
            if (num >= txt.Length)
                break;
            var c = txt[next];

            static string CodePoint(int next, string txt, ref int num, int size)
            {
                if (next + size >= txt.Length)
                    throw new Exception("Undefined escape sequence!");
                num += size;
                return char.ConvertFromUtf32(Convert.ToInt32(txt.Substring(next + 1, size), 16));
            }

            stringBuilder.Append(
                c switch
                {
                    'b' => "\b",
                    't' => "\t",
                    'n' => "\n",
                    'f' => "\f",
                    'r' => "\r",
                    '\'' => "\'",
                    '\"' => "\"",
                    '\\' => "\\",
                    'u' => CodePoint(next, txt, ref num, 4),
                    'U' => CodePoint(next, txt, ref num, 8),
                    var _ => throw new Exception("Undefined escape sequence!")
                }
            );
            i = num + 2;
        }

        return stringBuilder.ToString();
    }
}

#endregion
