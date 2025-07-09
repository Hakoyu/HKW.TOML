using HKW.HKWTOML.Attributes;
using HKW.HKWTOML.Interfaces;

namespace HKW.HKWTOML.ObjectBuilder;

/// <summary>
/// Toml转换为类设置
/// </summary>
public class ObjectBuilderOptions
{
    /// <summary>
    /// 合并int和float
    /// (如果一个数组中同时存在int和float类型,则会被转换成float)
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool MergeIntegerAndFloat { get; set; } = true;

    /// <summary>
    /// 缩进
    /// <para>默认为 "<see langword="    "/>"</para>
    /// </summary>
    public string Indent { get; set; } = "    ";

    #region Comment

    /// <summary>
    /// 添加注释
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool AddComment { get; set; } = false;

    /// <summary>
    /// 注释格式化文本
    /// <para>默认为
    /// <![CDATA[
    /// {0}/// <summary>
    /// {0}/// {1}
    /// {0}/// </summary>
    /// ]]>
    /// </para>
    /// </summary>
    public string CommentFormat { get; set; } =
        @"{0}/// <summary>
{0}/// {1}
{0}/// </summary>";

    /// <summary>
    /// 多行注释格式化文本
    /// <para>默认为 "<see langword="{0}/// &lt;para&gt;{1}&lt;/para&gt;"/>"</para>
    /// </summary>
    public string CommentParaFormat { get; set; } = "{0}/// <para>{1}</para>";

    #endregion Comment

    #region KeyName

    /// <summary>
    /// 将键名称转换为帕斯卡命名格式(属性命名格式)
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool KeyNameToPascal { get; set; } = true;

    /// <summary>
    /// 键名称转换器
    /// <para>默认为 <see langword="null"/></para>
    /// </summary>
    public Func<string, string>? KeyNameConverterFunc { get; set; }

    #endregion KeyName

    #region Attribute

    /// <summary>
    /// 所有属性添加 <see cref="TomlPropertyOrderAttribute"/>
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool AddTomlPropertyOrderAttribute { get; set; } = false;

    /// <summary>
    /// 特性 <see cref="TomlPropertyOrderAttribute"/> 格式化文本
    /// <para>默认为 "<see langword="TomlPropertyOrder({0})"/>"</para>
    /// </summary>
    public string TomlPropertyOrderAttributeFormat { get; set; } = "TomlPropertyOrder({0})";

    /// <summary>
    /// 所有属性添加 <see cref="TomlPropertyNameAttribute"/>
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool AddTomlPropertyNameAttribute { get; set; } = false;

    /// <summary>
    /// 特性 <see cref="TomlPropertyNameAttribute"/> 格式化文本
    /// <para>默认为 "<see langword="TomlPropertyName(&quot;{0}&quot;)"/>"</para>
    /// </summary>
    public string TomlPropertyNameAttributeFormat { get; set; } = "TomlPropertyName(\"{0}\")";

    /// <summary>
    /// 所有属性添加 <see cref="TomlRequiredAttribute"/>
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool AddTomlRequiredAttribute { get; set; } = false;

    /// <summary>
    /// 特性 <see cref="TomlRequiredAttribute"/> 文本
    /// <para>默认为 "<see langword="TomlRequired"/>"</para>
    /// </summary>
    public string TomlRequiredAttribute { get; set; } = "TomlRequired";

    /// <summary>
    /// 特性格式化文本
    /// <para>默认为 "<see langword="{0}[{1}]"/>"</para>
    /// </summary>
    public string AttributeFormat { get; set; } = "{0}[{1}]";

    /// <summary>
    /// 类添加的特性
    /// <para>注意: 不要加 "[]"</para>
    /// </summary>
    public HashSet<string> ObjectAttributes { get; set; } = [];

    /// <summary>
    /// 属性添加的特性
    /// <para>注意: 不要加 "[]"</para>
    /// </summary>
    public HashSet<string> PropertyAttributes { get; set; } = [];

    #endregion Attribute

    #region KeyWordSeparator

    /// <summary>
    /// 删除键的单词分隔符 如 "_"
    /// <para>默认为 <see langword="true"/></para>
    /// </summary>
    public bool RemoveKeyWordSeparator { get; set; } = true;

    /// <summary>
    /// 单词分隔符
    /// <para>默认为 '<see langword=" "/>'</para>
    /// </summary>
    public char KeyWordSeparator { get; set; } = ' ';

    #endregion KeyWordSeparator

    #region Inheritance

    /// <summary>
    /// 为所有非匿名类添加的继承
    /// </summary>
    public HashSet<string> MultipleInheritance { get; set; } = [];

    /// <summary>
    /// 为所有非匿名类添加 <see cref="ITomlObjectComment"/> 接口
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool AddITomlClassCommentInterface { get; set; } = false;

    /// <summary>
    /// ITomlClassComment接口名称
    /// <para>默认为 <see langword="nameof(ITomlClassComment)"/></para>
    /// </summary>
    public string ITomlClassCommentInterface { get; set; } = nameof(ITomlObjectComment);

    /// <summary>
    /// 继承格式化文本
    /// <para>默认为 "<see langword=" : {0}"/>"</para>
    /// </summary>
    public string InheritanceFormat { get; set; } = " : {0}";

    /// <summary>
    /// ITomlClassComment接口值格式化文本
    /// <para>默认为
    /// <![CDATA[
    /// {0}/// <inheritdoc/>
    /// {0}public string ClassComment { get; set; } = string.Empty;
    /// {0}/// <inheritdoc/>
    /// {0}public Dictionary<string, string> ValueComments { get; set; } = new();
    ///
    /// ]]>
    /// </para>
    /// </summary>
    public string ITomlClassInterfaceValueFormat { get; set; } =
        @"{0}/// <inheritdoc/>
{0}public string ObjectComment {{ get; set; }} = string.Empty;
{0}/// <inheritdoc/>
{0}public Dictionary<string, string> PropertyComments {{ get; set; }} = new();

";

    #endregion Inheritance

    #region ValueTypeNameConvert

    /// <summary>
    /// TomlBoolean类型名称转换
    /// <para>默认为 "<see langword="bool"/></para>
    /// </summary>
    public string TomlBooleanNameConvert { get; set; } = "bool";

    /// <summary>
    /// TomlString类型名称转换
    /// <para>默认为 "<see langword="string"/>"</para>
    /// </summary>
    public string TomlStringNameConvert { get; set; } = "string";

    /// <summary>
    /// TomlFloat类型名称转换
    /// <para>默认为 "<see langword="double"/>"</para>
    /// </summary>
    public string TomlFloatNameConvert { get; set; } = "double";

    /// <summary>
    /// TomlInteger类型名称转换
    /// <para>默认为 "<see langword="int"/>"</para>
    /// </summary>
    public string TomlIntegerNameConvert { get; set; } = "int";

    /// <summary>
    /// TomlInteger64类型名称转换
    /// <para>默认为 "<see langword="long"/>"</para>
    /// </summary>
    public string TomlInteger64NameConvert { get; set; } = "long";

    /// <summary>
    /// TomlDateTime类型名称转换
    /// <para>默认为 "<see langword="DateTime"/>"</para>
    /// </summary>
    public string TomlDateTimeNameConvert { get; set; } = "DateTime";

    /// <summary>
    /// TomlDateTimeLocal类型名称转换
    /// <para>默认为 "<see langword="DateTime"/>"</para>
    /// </summary>
    public string TomlDateTimeLocalNameConvert { get; set; } = "DateTime";

    /// <summary>
    /// TomlDateTimeOffset类型名称转换
    /// <para>默认为 "<see langword="DateTimeOffset"/>"</para>
    /// </summary>
    public string TomlDateTimeOffsetNameConvert { get; set; } = "DateTimeOffset";

    #endregion ValueTypeNameConvert

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
    /// <para>默认为 "<see langword="{0}AnonymousClass"/>"</para>
    /// </summary>
    public string AnonymousClassNameFormat { get; set; } = "{0}AnonymousClass";

    /// <summary>
    /// 属性格式化文本
    /// <para>默认为 "<see langword="{0}public {1} {2} {{ get; set; }}"/>"
    /// </para>
    /// </summary>
    public string PropertyFormat { get; set; } = "{0}public {1} {2} {{ get; set; }}";

    /// <summary>
    /// 类格式化文本
    /// <para>默认为
    /// <![CDATA[
    /// {0}public class {1}
    /// {{
    /// {2}
    /// }}
    /// ]]>
    /// </para>
    /// </summary>
    public string ClassFormat { get; set; } =
        @"{0}public class {1}
{{
{2}
}}
";

    #endregion Format

    /// <summary>
    /// 获取Toml类转换后的类名称
    /// </summary>
    /// <param name="node">Toml节点</param>
    /// <param name="typeCode">类型标识</param>
    /// <returns>转换后的类名称</returns>
    public string GetConvertName(TomlNode node, TomlTypeCode typeCode)
    {
        return typeCode switch
        {
            TomlTypeCode.Boolean => TomlBooleanNameConvert,
            TomlTypeCode.String => TomlStringNameConvert,
            TomlTypeCode.Float => TomlFloatNameConvert,
            TomlTypeCode.DateTime => TomlDateTimeNameConvert,
            TomlTypeCode.DateTimeLocal => TomlDateTimeLocalNameConvert,
            TomlTypeCode.DateTimeOffset => TomlDateTimeOffsetNameConvert,
            TomlTypeCode.Integer when node.AsTomlInteger.IsInteger64 => TomlInteger64NameConvert,
            TomlTypeCode.Integer => TomlIntegerNameConvert,
            _ => nameof(TomlNode)
        };
    }
}
