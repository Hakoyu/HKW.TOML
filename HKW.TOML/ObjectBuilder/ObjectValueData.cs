using System.Diagnostics;
using System.Text;

namespace HKW.HKWTOML.ObjectBuilder;

/// <summary>
/// toml类值
/// </summary>
[DebuggerDisplay("{TypeName}, {Name}")]
internal class ObjectValueData
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
    /// 设置
    /// </summary>
    private readonly ObjectBuilderOptions _options;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="options">设置</param>
    /// <param name="name">名称</param>
    /// <param name="typeName">类型名称</param>
    public ObjectValueData(ObjectBuilderOptions options, string name, string typeName)
    {
        _options = options;
        Name = name;
        TypeName = typeName;
    }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="options">设置</param>
    /// <param name="name">名称</param>
    /// <param name="node">类值(推断类型名称)</param>
    public ObjectValueData(ObjectBuilderOptions options, string name, TomlNode node)
        : this(options, name, options.GetConvertName(node, TomlType.GetTypeCode(node))) { }

    /// <summary>
    /// 转化为格式化字符串
    /// </summary>
    /// <returns>字符串</returns>
    public override string ToString()
    {
        var valueData = string.Format(_options.PropertyFormat, _options.Indent, TypeName, Name);
        return GetComment(Comment) + Environment.NewLine + GetAttribute(Attributes) + valueData;
    }

    /// <summary>
    /// 获取注释
    /// </summary>
    /// <param name="comment">注释</param>
    /// <returns>格式化的注释</returns>
    private string GetComment(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            return comment;
        var comments = comment.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (comments.Length is 1)
            return string.Format(_options.CommentFormat, _options.Indent, comments[0]);
        var multiLineComment =
            comments[0]
            + Environment.NewLine
            + string.Join(
                Environment.NewLine,
                comments[1..].Select(
                    s => string.Format(_options.CommentParaFormat, _options.Indent, s)
                )
            );
        return string.Format(_options.CommentFormat, _options.Indent, multiLineComment);
    }

    /// <summary>
    /// 获取特性数据
    /// </summary>
    /// <param name="attributes">特性</param>
    /// <returns>格式化的特性数据</returns>
    private string GetAttribute(IEnumerable<string> attributes)
    {
        var sb = new StringBuilder();
        foreach (var attribute in attributes)
            sb.AppendLine(string.Format(_options.AttributeFormat, _options.Indent, attribute));
        return sb.ToString();
    }
}
