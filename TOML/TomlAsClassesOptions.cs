namespace HKW.TOML;

/// <summary>
/// Toml转换为类设置
/// </summary>
public class TomlAsClassesOptions
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
    /// <para>默认为 "<see langword="{0}/// &lt;summary&gt;\n{0}/// {1}\n{0}/// &lt;/summary&gt;\n"/>"</para>
    /// </summary>
    public string CommentFormat { get; set; } = "{0}/// <summary>\n{0}/// {1}\n{0}/// </summary>\n";

    /// <summary>
    /// 多行注释格式化文本
    /// <para>默认为 "<see langword="{0}/// &lt;para&gt;{1}&lt;/para&gt;"/>"</para>
    /// </summary>
    public string CommentParaFormat { get; set; } = "{0}/// <para>{1}</para>";
    #endregion
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
    #endregion

    #region Attribute
    /// <summary>
    /// 添加Toml参数顺序特性
    /// <para>默认为 <see langword="false"/></para>
    /// </summary>
    public bool AddTomlParameterOrderAttribute { get; set; } = false;

    /// <summary>
    /// 添加Toml参数顺序特性
    /// <para>默认为 "<see langword="TomlSortOrder({0})"/>"</para>
    /// </summary>
    public string TomlParameterOrderAttributeFomat { get; set; } = "TomlParameterOrder({0})";

    /// <summary>
    /// 特性格式化文本
    /// <para>默认为 "<see langword="{0}[{1}]"/>"</para>
    /// </summary>
    public string AttributeFomat { get; set; } = "{0}[{1}]";


    /// <summary>
    /// 类添加的特性
    /// </summary>
    public HashSet<string> ClassAttributes { get; set; } = new();

    /// <summary>
    /// 属性添加的特性
    /// </summary>
    public HashSet<string> PropertyAttributes { get; set; } = new();
    #endregion
    #region KeyWordSeparator
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
    #endregion

    #region Interfaces
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
    /// <para>默认为 <see langword="nameof(ITomlClassComment)"/></para>
    /// </summary>
    public string ITomlClassInterface { get; set; } = nameof(ITomlClassComment);

    /// <summary>
    /// 继承格式化文本
    /// <para>默认为 "<see langword=" : {0}"/>"</para>
    /// </summary>
    public string InheritanceFormat { get; set; } = " : {0}";

    /// <summary>
    /// ITomlClass接口值格式化文本
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
    public string ClassFormat { get; set; } = "{0}public class {1} \n{{\n{2}}}\n";

    #endregion
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
