using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Interfaces;
using Newtonsoft.Json;

namespace HKW.HKWTOML.Benchmark;

#pragma warning disable CS8618

/// <summary>
/// TOML 测试数据 - 覆盖所有数据类型和格式
/// <para>基于 TOML v1.0.0 规范</para>
/// </summary>
public class ExampleObject : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    /// <summary>
    /// ============ 基本字符串 ============
    /// </summary>
    [TomlPropertyOrder(0)]
    [TomlPropertyName("title")]
    [JsonProperty("title")]
    public string Title { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("description")]
    [JsonProperty("description")]
    public string Description { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("basic_string")]
    [JsonProperty("basic_string")]
    public string BasicString { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("unicode_string")]
    [JsonProperty("unicode_string")]
    public string UnicodeString { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("empty_string")]
    [JsonProperty("empty_string")]
    public string EmptyString { get; set; }

    /// <summary>
    /// ============ 多行字符串 ============
    /// </summary>
    [TomlPropertyOrder(5)]
    [TomlPropertyName("multiline_basic1")]
    [JsonProperty("multiline_basic1")]
    public string MultilineBasic1 { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("multiline_basic2")]
    [JsonProperty("multiline_basic2")]
    public string MultilineBasic2 { get; set; }

    [TomlPropertyOrder(7)]
    [TomlPropertyName("multiline_literal")]
    [JsonProperty("multiline_literal")]
    public string MultilineLiteral { get; set; }

    /// <summary>
    /// ============ 字面字符串 ============
    /// </summary>
    [TomlPropertyOrder(8)]
    [TomlPropertyName("literal_string")]
    [JsonProperty("literal_string")]
    public string LiteralString { get; set; }

    [TomlPropertyOrder(9)]
    [TomlPropertyName("windows_path")]
    [JsonProperty("windows_path")]
    public string WindowsPath { get; set; }

    [TomlPropertyOrder(10)]
    [TomlPropertyName("regex_pattern")]
    [JsonProperty("regex_pattern")]
    public string RegexPattern { get; set; }

    /// <summary>
    /// ============ 转义字符串 ============
    /// </summary>
    [TomlPropertyOrder(11)]
    [TomlPropertyName("escaped_string")]
    [JsonProperty("escaped_string")]
    public string EscapedString { get; set; }

    [TomlPropertyOrder(12)]
    [TomlPropertyName("unicode_escape")]
    [JsonProperty("unicode_escape")]
    public string UnicodeEscape { get; set; }

    /// <summary>
    /// ============ 整数 ============
    /// </summary>
    [TomlPropertyOrder(13)]
    [TomlPropertyName("positive_int")]
    [JsonProperty("positive_int")]
    public int PositiveInt { get; set; }

    [TomlPropertyOrder(14)]
    [TomlPropertyName("negative_int")]
    [JsonProperty("negative_int")]
    public int NegativeInt { get; set; }

    [TomlPropertyOrder(15)]
    [TomlPropertyName("zero_int")]
    [JsonProperty("zero_int")]
    public int ZeroInt { get; set; }

    [TomlPropertyOrder(16)]
    [TomlPropertyName("underscore_int")]
    [JsonProperty("underscore_int")]
    public int UnderscoreInt { get; set; }

    [TomlPropertyOrder(17)]
    [TomlPropertyName("hex_int")]
    [JsonProperty("hex_int")]
    public long HexInt { get; set; }

    [TomlPropertyOrder(18)]
    [TomlPropertyName("octal_int")]
    [JsonProperty("octal_int")]
    public int OctalInt { get; set; }

    [TomlPropertyOrder(19)]
    [TomlPropertyName("binary_int")]
    [JsonProperty("binary_int")]
    public int BinaryInt { get; set; }

    /// <summary>
    /// ============ 浮点数 ============
    /// </summary>
    [TomlPropertyOrder(20)]
    [TomlPropertyName("positive_float")]
    [JsonProperty("positive_float")]
    public double PositiveFloat { get; set; }

    [TomlPropertyOrder(21)]
    [TomlPropertyName("negative_float")]
    [JsonProperty("negative_float")]
    public double NegativeFloat { get; set; }

    [TomlPropertyOrder(22)]
    [TomlPropertyName("exponent_float")]
    [JsonProperty("exponent_float")]
    public double ExponentFloat { get; set; }

    [TomlPropertyOrder(23)]
    [TomlPropertyName("negative_exponent")]
    [JsonProperty("negative_exponent")]
    public double NegativeExponent { get; set; }

    [TomlPropertyOrder(24)]
    [TomlPropertyName("underscore_float")]
    [JsonProperty("underscore_float")]
    public double UnderscoreFloat { get; set; }

    [TomlPropertyOrder(25)]
    [TomlPropertyName("infinity")]
    [JsonProperty("infinity")]
    public double Infinity { get; set; }

    [TomlPropertyOrder(26)]
    [TomlPropertyName("negative_infinity")]
    [JsonProperty("negative_infinity")]
    public double NegativeInfinity { get; set; }

    [TomlPropertyOrder(27)]
    [TomlPropertyName("not_a_number")]
    [JsonProperty("not_a_number")]
    public double NotANumber { get; set; }

    /// <summary>
    /// ============ 布尔值 ============
    /// </summary>
    [TomlPropertyOrder(28)]
    [TomlPropertyName("is_enabled")]
    [JsonProperty("is_enabled")]
    public bool IsEnabled { get; set; }

    [TomlPropertyOrder(29)]
    [TomlPropertyName("is_disabled")]
    [JsonProperty("is_disabled")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// ============ 日期时间 ============
    /// <para>RFC 3339 格式</para>
    /// </summary>
    [TomlPropertyOrder(30)]
    [TomlPropertyName("offset_datetime")]
    [JsonProperty("offset_datetime")]
    public DateTimeOffset OffsetDatetime { get; set; }

    [TomlPropertyOrder(31)]
    [TomlPropertyName("local_datetime")]
    [JsonProperty("local_datetime")]
    public DateTime LocalDatetime { get; set; }

    [TomlPropertyOrder(32)]
    [TomlPropertyName("local_date")]
    [JsonProperty("local_date")]
    public DateTime LocalDate { get; set; }

    [TomlPropertyOrder(33)]
    [TomlPropertyName("local_time")]
    [JsonProperty("local_time")]
    public DateTime LocalTime { get; set; }

    /// <summary>
    /// 带毫秒的日期时间
    /// </summary>
    [TomlPropertyOrder(34)]
    [TomlPropertyName("precise_datetime")]
    [JsonProperty("precise_datetime")]
    public DateTimeOffset PreciseDatetime { get; set; }

    [TomlPropertyOrder(35)]
    [TomlPropertyName("timezone_datetime")]
    [JsonProperty("timezone_datetime")]
    public DateTimeOffset TimezoneDatetime { get; set; }

    /// <summary>
    /// ============ 数组 ============
    /// </summary>
    [TomlPropertyOrder(36)]
    [TomlPropertyName("simple_array")]
    [JsonProperty("simple_array")]
    public List<int> SimpleArray { get; set; }

    [TomlPropertyOrder(37)]
    [TomlPropertyName("string_array")]
    [JsonProperty("string_array")]
    public List<string> StringArray { get; set; }

    [TomlPropertyOrder(38)]
    [TomlPropertyName("nested_array")]
    [JsonProperty("nested_array")]
    public List<List<TomlNode>> NestedArray { get; set; }

    [TomlPropertyOrder(39)]
    [TomlPropertyName("mixed_array")]
    [JsonProperty("mixed_array")]
    public List<TomlNode> MixedArray { get; set; }

    [TomlPropertyOrder(40)]
    [TomlPropertyName("empty_array")]
    [JsonProperty("empty_array")]
    public List<TomlNode> EmptyArray { get; set; }

    /// <summary>
    /// 多行数组
    /// </summary>
    [TomlPropertyOrder(41)]
    [TomlPropertyName("multiline_array")]
    [JsonProperty("multiline_array")]
    public List<int> MultilineArray { get; set; }

    /// <summary>
    /// 数组中的数组
    /// </summary>
    [TomlPropertyOrder(42)]
    [TomlPropertyName("array_of_arrays")]
    [JsonProperty("array_of_arrays")]
    public List<List<TomlNode>> ArrayOfArrays { get; set; }

    [TomlPropertyOrder(43)]
    [TomlPropertyName("products")]
    [JsonProperty("products")]
    public List<ProductsAnonymousClass> Products { get; set; }

    [TomlPropertyOrder(44)]
    [TomlPropertyName("fruit")]
    [JsonProperty("fruit")]
    public List<FruitAnonymousClass> Fruit { get; set; }

    /// <summary>
    /// ============ 表 ============
    /// </summary>
    [TomlPropertyOrder(45)]
    [TomlPropertyName("database")]
    [JsonProperty("database")]
    public DatabaseClass Database { get; set; }

    /// <summary>
    /// ============ 嵌套表 ============
    /// </summary>
    [TomlPropertyOrder(46)]
    [TomlPropertyName("clients")]
    [JsonProperty("clients")]
    public ClientsClass Clients { get; set; }

    /// <summary>
    /// ============ 复杂嵌套结构 ============
    /// </summary>
    [TomlPropertyOrder(47)]
    [TomlPropertyName("servers")]
    [JsonProperty("servers")]
    public ServersClass Servers { get; set; }

    /// <summary>
    /// ============ 特殊值测试 ============
    /// </summary>
    [TomlPropertyOrder(48)]
    [TomlPropertyName("special_values")]
    [JsonProperty("special_values")]
    public SpecialValuesClass SpecialValues { get; set; }

    /// <summary>
    /// ============ 复杂数据结构 ============
    /// </summary>
    [TomlPropertyOrder(49)]
    [TomlPropertyName("config")]
    [JsonProperty("config")]
    public ConfigClass Config { get; set; }

    /// <summary>
    /// ============ 国际化测试 ============
    /// </summary>
    [TomlPropertyOrder(50)]
    [TomlPropertyName("i18n")]
    [JsonProperty("i18n")]
    public I18nClass I18n { get; set; }

    /// <summary>
    /// ============ 数据类型边界测试 ============
    /// </summary>
    [TomlPropertyOrder(51)]
    [TomlPropertyName("boundaries")]
    [JsonProperty("boundaries")]
    public BoundariesClass Boundaries { get; set; }

    /// <summary>
    /// ============ 引用和转义测试 ============
    /// </summary>
    [TomlPropertyOrder(52)]
    [TomlPropertyName("quotes_and_escapes")]
    [JsonProperty("quotes_and_escapes")]
    public QuotesAndEscapesClass QuotesAndEscapes { get; set; }
}

public class ProductsAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("sku")]
    [JsonProperty("sku")]
    public int Sku { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("price")]
    [JsonProperty("price")]
    public double Price { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("tags")]
    [JsonProperty("tags")]
    public List<string> Tags { get; set; }
}

public class FruitAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("color")]
    [JsonProperty("color")]
    public string Color { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("shape")]
    [JsonProperty("shape")]
    public string Shape { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("variety")]
    [JsonProperty("variety")]
    public List<VarietyAnonymousClass> Variety { get; set; }
}

public class VarietyAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("sweetness")]
    [JsonProperty("sweetness")]
    public int Sweetness { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("inline_table")]
    [JsonProperty("inline_table")]
    public InlineTableClass InlineTable { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("empty_inline_table")]
    [JsonProperty("empty_inline_table")]
    public EmptyInlineTableClass EmptyInlineTable { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("nested_inline")]
    [JsonProperty("nested_inline")]
    public NestedInlineClass NestedInline { get; set; }
}

/// <summary>
/// ============ 内联表 ============
/// </summary>
public class InlineTableClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("age")]
    [JsonProperty("age")]
    public int Age { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("city")]
    [JsonProperty("city")]
    public string City { get; set; }
}

public class EmptyInlineTableClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();
}

public class NestedInlineClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("point")]
    [JsonProperty("point")]
    public PointClass Point { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("color")]
    [JsonProperty("color")]
    public string Color { get; set; }
}

public class PointClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("x")]
    [JsonProperty("x")]
    public int X { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("y")]
    [JsonProperty("y")]
    public int Y { get; set; }
}

/// <summary>
/// ============ 表 ============
/// </summary>
public class DatabaseClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("server")]
    [JsonProperty("server")]
    public string Server { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("ports")]
    [JsonProperty("ports")]
    public List<int> Ports { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("connection_max")]
    [JsonProperty("connection_max")]
    public int ConnectionMax { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("enabled")]
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("credentials")]
    [JsonProperty("credentials")]
    public CredentialsClass Credentials { get; set; }
}

public class CredentialsClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("username")]
    [JsonProperty("username")]
    public string Username { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("password")]
    [JsonProperty("password")]
    public string Password { get; set; }
}

/// <summary>
/// ============ 嵌套表 ============
/// </summary>
public class ClientsClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("data")]
    [JsonProperty("data")]
    public DataClass Data { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("hosts")]
    [JsonProperty("hosts")]
    public HostsClass Hosts { get; set; }
}

public class DataClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("hosts")]
    [JsonProperty("hosts")]
    public List<string> Hosts { get; set; }
}

public class HostsClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("alpha")]
    [JsonProperty("alpha")]
    public string Alpha { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("omega")]
    [JsonProperty("omega")]
    public string Omega { get; set; }
}

/// <summary>
/// ============ 复杂嵌套结构 ============
/// </summary>
public class ServersClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("alpha")]
    [JsonProperty("alpha")]
    public AlphaClass Alpha { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("beta")]
    [JsonProperty("beta")]
    public BetaClass Beta { get; set; }
}

public class AlphaClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("ip")]
    [JsonProperty("ip")]
    public string Ip { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("dc")]
    [JsonProperty("dc")]
    public string Dc { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("country")]
    [JsonProperty("country")]
    public string Country { get; set; }
}

public class BetaClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("ip")]
    [JsonProperty("ip")]
    public string Ip { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("dc")]
    [JsonProperty("dc")]
    public string Dc { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("country")]
    [JsonProperty("country")]
    public string Country { get; set; }
}

/// <summary>
/// ============ 特殊值测试 ============
/// </summary>
public class SpecialValuesClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("empty_string")]
    [JsonProperty("empty_string")]
    public string EmptyString { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("zero_integer")]
    [JsonProperty("zero_integer")]
    public int ZeroInteger { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("zero_float")]
    [JsonProperty("zero_float")]
    public int ZeroFloat { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("positive_infinity")]
    [JsonProperty("positive_infinity")]
    public double PositiveInfinity { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("negative_infinity")]
    [JsonProperty("negative_infinity")]
    public double NegativeInfinity { get; set; }

    [TomlPropertyOrder(5)]
    [TomlPropertyName("not_a_number")]
    [JsonProperty("not_a_number")]
    public double NotANumber { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("true_boolean")]
    [JsonProperty("true_boolean")]
    public bool TrueBoolean { get; set; }

    [TomlPropertyOrder(7)]
    [TomlPropertyName("false_boolean")]
    [JsonProperty("false_boolean")]
    public bool FalseBoolean { get; set; }

    [TomlPropertyOrder(8)]
    [TomlPropertyName("empty_array")]
    [JsonProperty("empty_array")]
    public List<TomlNode> EmptyArray { get; set; }

    /// <summary>
    /// ============ 注释测试 ============
    /// <para>这是一个注释</para>
    /// </summary>
    [TomlPropertyOrder(9)]
    [TomlPropertyName("commented_key1")]
    [JsonProperty("commented_key1")]
    public string CommentedKey1 { get; set; }

    [TomlPropertyOrder(10)]
    [TomlPropertyName("commented_key2")]
    [JsonProperty("commented_key2")]
    public string CommentedKey2 { get; set; }

    /// <summary>
    /// 多行注释
    /// <para>可以有多行</para>
    /// <para>每行都以 # 开头</para>
    /// </summary>
    [TomlPropertyOrder(11)]
    [TomlPropertyName("commented_key3")]
    [JsonProperty("commented_key3")]
    public string CommentedKey3 { get; set; }

    /// <summary>
    /// 注释
    /// <para>可以有多行</para>
    /// </summary>
    [TomlPropertyOrder(12)]
    [TomlPropertyName("commented_key4")]
    [JsonProperty("commented_key4")]
    public string CommentedKey4 { get; set; }

    /// <summary>
    /// 这是一个带有注释的数组
    /// <para>可以在数组末尾注释</para>
    /// </summary>
    [TomlPropertyOrder(13)]
    [TomlPropertyName("commented_array1")]
    [JsonProperty("commented_array1")]
    public List<int> CommentedArray1 { get; set; }

    /// <summary>
    /// 单行数组可以有行尾注释
    /// </summary>
    [TomlPropertyOrder(14)]
    [TomlPropertyName("commented_array2")]
    [JsonProperty("commented_array2")]
    public List<int> CommentedArray2 { get; set; }

    [TomlPropertyOrder(15)]
    [TomlPropertyName("empty_inline_table")]
    [JsonProperty("empty_inline_table")]
    public EmptyInlineTableClass EmptyInlineTable { get; set; }

    /// <summary>
    /// 单行表格可以有行尾注释
    /// </summary>
    [TomlPropertyOrder(16)]
    [TomlPropertyName("commented_table")]
    [JsonProperty("commented_table")]
    public CommentedTableClass CommentedTable { get; set; }
}

/// <summary>
/// 单行表格可以有行尾注释
/// </summary>
public class CommentedTableClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("value")]
    [JsonProperty("value")]
    public int Value { get; set; }
}

/// <summary>
/// ============ 复杂数据结构 ============
/// </summary>
public class ConfigClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("version")]
    [JsonProperty("version")]
    public string Version { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("debug")]
    [JsonProperty("debug")]
    public bool Debug { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("max_connections")]
    [JsonProperty("max_connections")]
    public int MaxConnections { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("timeout")]
    [JsonProperty("timeout")]
    public double Timeout { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("endpoints")]
    [JsonProperty("endpoints")]
    public List<EndpointsAnonymousClass> Endpoints { get; set; }

    [TomlPropertyOrder(5)]
    [TomlPropertyName("logging")]
    [JsonProperty("logging")]
    public LoggingClass Logging { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("features")]
    [JsonProperty("features")]
    public FeaturesClass Features { get; set; }
}

public class EndpointsAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("url")]
    [JsonProperty("url")]
    public string Url { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("methods")]
    [JsonProperty("methods")]
    public List<string> Methods { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("timeout")]
    [JsonProperty("timeout")]
    public int Timeout { get; set; }
}

public class LoggingClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("level")]
    [JsonProperty("level")]
    public string Level { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("file")]
    [JsonProperty("file")]
    public string File { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("rotate")]
    [JsonProperty("rotate")]
    public bool Rotate { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("max_size")]
    [JsonProperty("max_size")]
    public string MaxSize { get; set; }
}

public class FeaturesClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("authentication")]
    [JsonProperty("authentication")]
    public bool Authentication { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("caching")]
    [JsonProperty("caching")]
    public bool Caching { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("compression")]
    [JsonProperty("compression")]
    public bool Compression { get; set; }
}

/// <summary>
/// ============ 国际化测试 ============
/// </summary>
public class I18nClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("english")]
    [JsonProperty("english")]
    public string English { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("chinese")]
    [JsonProperty("chinese")]
    public string Chinese { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("japanese")]
    [JsonProperty("japanese")]
    public string Japanese { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("korean")]
    [JsonProperty("korean")]
    public string Korean { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("arabic")]
    [JsonProperty("arabic")]
    public string Arabic { get; set; }

    [TomlPropertyOrder(5)]
    [TomlPropertyName("russian")]
    [JsonProperty("russian")]
    public string Russian { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("emoji")]
    [JsonProperty("emoji")]
    public string Emoji { get; set; }
}

/// <summary>
/// ============ 数据类型边界测试 ============
/// </summary>
public class BoundariesClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("max_int")]
    [JsonProperty("max_int")]
    public long MaxInt { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("min_int")]
    [JsonProperty("min_int")]
    public long MinInt { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("small_float")]
    [JsonProperty("small_float")]
    public double SmallFloat { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("large_float")]
    [JsonProperty("large_float")]
    public long LargeFloat { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("very_long_string")]
    [JsonProperty("very_long_string")]
    public string VeryLongString { get; set; }
}

/// <summary>
/// ============ 引用和转义测试 ============
/// </summary>
public class QuotesAndEscapesClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("single_quote")]
    [JsonProperty("single_quote")]
    public string SingleQuote { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("double_quote")]
    [JsonProperty("double_quote")]
    public string DoubleQuote { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("backslash")]
    [JsonProperty("backslash")]
    public string Backslash { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("newline")]
    [JsonProperty("newline")]
    public string Newline { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("tab")]
    [JsonProperty("tab")]
    public string Tab { get; set; }

    [TomlPropertyOrder(5)]
    [TomlPropertyName("carriage_return")]
    [JsonProperty("carriage_return")]
    public string CarriageReturn { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("unicode")]
    [JsonProperty("unicode")]
    public string Unicode { get; set; }
}
