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
    public string Title { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("description")]
    public string Description { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("basic_string")]
    public string Basic_string { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("unicode_string")]
    public string Unicode_string { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("empty_string")]
    public string Empty_string { get; set; }

    /// <summary>
    /// ============ 多行字符串 ============
    /// </summary>
    [TomlPropertyOrder(5)]
    [TomlPropertyName("multiline_basic1")]
    public string Multiline_basic1 { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("multiline_basic2")]
    public string Multiline_basic2 { get; set; }

    [TomlPropertyOrder(7)]
    [TomlPropertyName("multiline_literal")]
    public string Multiline_literal { get; set; }

    /// <summary>
    /// ============ 字面字符串 ============
    /// </summary>
    [TomlPropertyOrder(8)]
    [TomlPropertyName("literal_string")]
    public string Literal_string { get; set; }

    [TomlPropertyOrder(9)]
    [TomlPropertyName("windows_path")]
    public string Windows_path { get; set; }

    [TomlPropertyOrder(10)]
    [TomlPropertyName("regex_pattern")]
    public string Regex_pattern { get; set; }

    /// <summary>
    /// ============ 转义字符串 ============
    /// </summary>
    [TomlPropertyOrder(11)]
    [TomlPropertyName("escaped_string")]
    public string Escaped_string { get; set; }

    [TomlPropertyOrder(12)]
    [TomlPropertyName("unicode_escape")]
    public string Unicode_escape { get; set; }

    /// <summary>
    /// ============ 整数 ============
    /// </summary>
    [TomlPropertyOrder(13)]
    [TomlPropertyName("positive_int")]
    public int Positive_int { get; set; }

    [TomlPropertyOrder(14)]
    [TomlPropertyName("negative_int")]
    public int Negative_int { get; set; }

    [TomlPropertyOrder(15)]
    [TomlPropertyName("zero_int")]
    public int Zero_int { get; set; }

    [TomlPropertyOrder(16)]
    [TomlPropertyName("underscore_int")]
    public int Underscore_int { get; set; }

    [TomlPropertyOrder(17)]
    [TomlPropertyName("hex_int")]
    public long Hex_int { get; set; }

    [TomlPropertyOrder(18)]
    [TomlPropertyName("octal_int")]
    public int Octal_int { get; set; }

    [TomlPropertyOrder(19)]
    [TomlPropertyName("binary_int")]
    public int Binary_int { get; set; }

    /// <summary>
    /// ============ 浮点数 ============
    /// </summary>
    [TomlPropertyOrder(20)]
    [TomlPropertyName("positive_float")]
    public double Positive_float { get; set; }

    [TomlPropertyOrder(21)]
    [TomlPropertyName("negative_float")]
    public double Negative_float { get; set; }

    [TomlPropertyOrder(22)]
    [TomlPropertyName("exponent_float")]
    public double Exponent_float { get; set; }

    [TomlPropertyOrder(23)]
    [TomlPropertyName("negative_exponent")]
    public double Negative_exponent { get; set; }

    [TomlPropertyOrder(24)]
    [TomlPropertyName("underscore_float")]
    public double Underscore_float { get; set; }

    [TomlPropertyOrder(25)]
    [TomlPropertyName("infinity")]
    public double Infinity { get; set; }

    [TomlPropertyOrder(26)]
    [TomlPropertyName("negative_infinity")]
    public double Negative_infinity { get; set; }

    [TomlPropertyOrder(27)]
    [TomlPropertyName("not_a_number")]
    public double Not_a_number { get; set; }

    /// <summary>
    /// ============ 布尔值 ============
    /// </summary>
    [TomlPropertyOrder(28)]
    [TomlPropertyName("is_enabled")]
    public bool Is_enabled { get; set; }

    [TomlPropertyOrder(29)]
    [TomlPropertyName("is_disabled")]
    public bool Is_disabled { get; set; }

    /// <summary>
    /// ============ 日期时间 ============
    /// <para>RFC 3339 格式</para>
    /// </summary>
    [TomlPropertyOrder(30)]
    [TomlPropertyName("offset_datetime")]
    public DateTimeOffset Offset_datetime { get; set; }

    [TomlPropertyOrder(31)]
    [TomlPropertyName("local_datetime")]
    public DateTime Local_datetime { get; set; }

    [TomlPropertyOrder(32)]
    [TomlPropertyName("local_date")]
    public DateTime Local_date { get; set; }

    [TomlPropertyOrder(33)]
    [TomlPropertyName("local_time")]
    public DateTime Local_time { get; set; }

    /// <summary>
    /// 带毫秒的日期时间
    /// </summary>
    [TomlPropertyOrder(34)]
    [TomlPropertyName("precise_datetime")]
    public DateTimeOffset Precise_datetime { get; set; }

    [TomlPropertyOrder(35)]
    [TomlPropertyName("timezone_datetime")]
    public DateTimeOffset Timezone_datetime { get; set; }

    /// <summary>
    /// ============ 数组 ============
    /// </summary>
    [TomlPropertyOrder(36)]
    [TomlPropertyName("simple_array")]
    public List<int> Simple_array { get; set; }

    [TomlPropertyOrder(37)]
    [TomlPropertyName("string_array")]
    public List<string> String_array { get; set; }

    [TomlPropertyOrder(38)]
    [TomlPropertyName("nested_array")]
    public List<List<TomlNode>> Nested_array { get; set; }

    [TomlPropertyOrder(39)]
    [TomlPropertyName("mixed_array")]
    public List<TomlNode> Mixed_array { get; set; }

    [TomlPropertyOrder(40)]
    [TomlPropertyName("empty_array")]
    public List<TomlNode> Empty_array { get; set; }

    /// <summary>
    /// 多行数组
    /// </summary>
    [TomlPropertyOrder(41)]
    [TomlPropertyName("multiline_array")]
    public List<int> Multiline_array { get; set; }

    /// <summary>
    /// 数组中的数组
    /// </summary>
    [TomlPropertyOrder(42)]
    [TomlPropertyName("array_of_arrays")]
    public List<List<TomlNode>> Array_of_arrays { get; set; }

    [TomlPropertyOrder(43)]
    [TomlPropertyName("products")]
    public List<ProductsAnonymousClass> Products { get; set; }

    [TomlPropertyOrder(44)]
    [TomlPropertyName("fruit")]
    public List<FruitAnonymousClass> Fruit { get; set; }

    /// <summary>
    /// ============ 表 ============
    /// </summary>
    [TomlPropertyOrder(45)]
    [TomlPropertyName("database")]
    public DatabaseClass Database { get; set; }

    /// <summary>
    /// ============ 嵌套表 ============
    /// </summary>
    [TomlPropertyOrder(46)]
    [TomlPropertyName("clients")]
    public ClientsClass Clients { get; set; }

    /// <summary>
    /// ============ 复杂嵌套结构 ============
    /// </summary>
    [TomlPropertyOrder(47)]
    [TomlPropertyName("servers")]
    public ServersClass Servers { get; set; }

    /// <summary>
    /// ============ 特殊值测试 ============
    /// </summary>
    [TomlPropertyOrder(48)]
    [TomlPropertyName("special_values")]
    public Special_valuesClass Special_values { get; set; }

    /// <summary>
    /// ============ 复杂数据结构 ============
    /// </summary>
    [TomlPropertyOrder(49)]
    [TomlPropertyName("config")]
    public ConfigClass Config { get; set; }

    /// <summary>
    /// ============ 国际化测试 ============
    /// </summary>
    [TomlPropertyOrder(50)]
    [TomlPropertyName("i18n")]
    public I18nClass I18n { get; set; }

    /// <summary>
    /// ============ 数据类型边界测试 ============
    /// </summary>
    [TomlPropertyOrder(51)]
    [TomlPropertyName("boundaries")]
    public BoundariesClass Boundaries { get; set; }

    /// <summary>
    /// ============ 引用和转义测试 ============
    /// </summary>
    [TomlPropertyOrder(52)]
    [TomlPropertyName("quotes_and_escapes")]
    public Quotes_and_escapesClass Quotes_and_escapes { get; set; }
}

public class ProductsAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("sku")]
    public int Sku { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("price")]
    public double Price { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("tags")]
    public List<string> Tags { get; set; }
}

public class FruitAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("color")]
    public string Color { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("shape")]
    public string Shape { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("variety")]
    public List<VarietyAnonymousClass> Variety { get; set; }
}

public class VarietyAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("sweetness")]
    public int Sweetness { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("inline_table")]
    public Inline_tableClass Inline_table { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("empty_inline_table")]
    public Empty_inline_tableClass Empty_inline_table { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("nested_inline")]
    public Nested_inlineClass Nested_inline { get; set; }
}

/// <summary>
/// ============ 内联表 ============
/// </summary>
public class Inline_tableClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("age")]
    public int Age { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("city")]
    public string City { get; set; }
}

public class Empty_inline_tableClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();
}

public class Nested_inlineClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("point")]
    public PointClass Point { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("color")]
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
    public int X { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("y")]
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
    public string Server { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("ports")]
    public List<int> Ports { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("connection_max")]
    public int Connection_max { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("enabled")]
    public bool Enabled { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("credentials")]
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
    public string Username { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("password")]
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
    public DataClass Data { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("hosts")]
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
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("hosts")]
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
    public string Alpha { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("omega")]
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
    public AlphaClass Alpha { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("beta")]
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
    public string Ip { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("dc")]
    public string Dc { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("country")]
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
    public string Ip { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("dc")]
    public string Dc { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("country")]
    public string Country { get; set; }
}

/// <summary>
/// ============ 特殊值测试 ============
/// </summary>
public class Special_valuesClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("empty_string")]
    public string Empty_string { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("zero_integer")]
    public int Zero_integer { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("zero_float")]
    public int Zero_float { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("positive_infinity")]
    public double Positive_infinity { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("negative_infinity")]
    public double Negative_infinity { get; set; }

    [TomlPropertyOrder(5)]
    [TomlPropertyName("not_a_number")]
    public double Not_a_number { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("true_boolean")]
    public bool True_boolean { get; set; }

    [TomlPropertyOrder(7)]
    [TomlPropertyName("false_boolean")]
    public bool False_boolean { get; set; }

    [TomlPropertyOrder(8)]
    [TomlPropertyName("empty_array")]
    public List<TomlNode> Empty_array { get; set; }

    /// <summary>
    /// ============ 注释测试 ============
    /// <para>这是一个注释</para>
    /// </summary>
    [TomlPropertyOrder(9)]
    [TomlPropertyName("commented_key1")]
    public string Commented_key1 { get; set; }

    [TomlPropertyOrder(10)]
    [TomlPropertyName("commented_key2")]
    public string Commented_key2 { get; set; }

    /// <summary>
    /// 多行注释
    /// <para>可以有多行</para>
    /// <para>每行都以 # 开头</para>
    /// </summary>
    [TomlPropertyOrder(11)]
    [TomlPropertyName("commented_key3")]
    public string Commented_key3 { get; set; }

    /// <summary>
    /// 注释
    /// <para>可以有多行</para>
    /// </summary>
    [TomlPropertyOrder(12)]
    [TomlPropertyName("commented_key4")]
    public string Commented_key4 { get; set; }

    /// <summary>
    /// 这是一个带有注释的数组
    /// <para>可以在数组末尾注释</para>
    /// </summary>
    [TomlPropertyOrder(13)]
    [TomlPropertyName("commented_array1")]
    public List<int> Commented_array1 { get; set; }

    /// <summary>
    /// 单行数组可以有行尾注释
    /// </summary>
    [TomlPropertyOrder(14)]
    [TomlPropertyName("commented_array2")]
    public List<int> Commented_array2 { get; set; }

    [TomlPropertyOrder(15)]
    [TomlPropertyName("empty_inline_table")]
    public Empty_inline_tableClass Empty_inline_table { get; set; }

    /// <summary>
    /// 单行表格可以有行尾注释
    /// </summary>
    [TomlPropertyOrder(16)]
    [TomlPropertyName("commented_table")]
    public Commented_tableClass Commented_table { get; set; }
}

/// <summary>
/// 单行表格可以有行尾注释
/// </summary>
public class Commented_tableClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("value")]
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
    public string Version { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("debug")]
    public bool Debug { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("max_connections")]
    public int Max_connections { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("timeout")]
    public double Timeout { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("endpoints")]
    public List<EndpointsAnonymousClass> Endpoints { get; set; }

    [TomlPropertyOrder(5)]
    [TomlPropertyName("logging")]
    public LoggingClass Logging { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("features")]
    public FeaturesClass Features { get; set; }
}

public class EndpointsAnonymousClass
{
    [TomlPropertyOrder(0)]
    [TomlPropertyName("name")]
    public string Name { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("url")]
    public string Url { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("methods")]
    public List<string> Methods { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("timeout")]
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
    public string Level { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("file")]
    public string File { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("rotate")]
    public bool Rotate { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("max_size")]
    public string Max_size { get; set; }
}

public class FeaturesClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("authentication")]
    public bool Authentication { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("caching")]
    public bool Caching { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("compression")]
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
    public string English { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("chinese")]
    public string Chinese { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("japanese")]
    public string Japanese { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("korean")]
    public string Korean { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("arabic")]
    public string Arabic { get; set; }

    [TomlPropertyOrder(5)]
    [TomlPropertyName("russian")]
    public string Russian { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("emoji")]
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
    public long Max_int { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("min_int")]
    public long Min_int { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("small_float")]
    public double Small_float { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("large_float")]
    public long Large_float { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("very_long_string")]
    public string Very_long_string { get; set; }
}

/// <summary>
/// ============ 引用和转义测试 ============
/// </summary>
public class Quotes_and_escapesClass : ITomlObjectComment
{
    /// <inheritdoc/>
    public TomlComment ObjectComment { get; set; } = new();

    /// <inheritdoc/>
    public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

    [TomlPropertyOrder(0)]
    [TomlPropertyName("single_quote")]
    public string Single_quote { get; set; }

    [TomlPropertyOrder(1)]
    [TomlPropertyName("double_quote")]
    public string Double_quote { get; set; }

    [TomlPropertyOrder(2)]
    [TomlPropertyName("backslash")]
    public string Backslash { get; set; }

    [TomlPropertyOrder(3)]
    [TomlPropertyName("newline")]
    public string Newline { get; set; }

    [TomlPropertyOrder(4)]
    [TomlPropertyName("tab")]
    public string Tab { get; set; }

    [TomlPropertyOrder(5)]
    [TomlPropertyName("carriage_return")]
    public string Carriage_return { get; set; }

    [TomlPropertyOrder(6)]
    [TomlPropertyName("unicode")]
    public string Unicode { get; set; }
}
