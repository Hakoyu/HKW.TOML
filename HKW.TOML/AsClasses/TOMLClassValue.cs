using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HKW.HKWTOML.AsClasses;

/// <summary>
/// toml类值
/// </summary>
[DebuggerDisplay("{TypeName}, {Name}")]
internal class TOMLClassValue
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

    private readonly TOMLAsClasses r_tomlAsClasses;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="tomlAsClass"></param>
    /// <param name="name">名称</param>
    /// <param name="typeName">类型名称</param>
    public TOMLClassValue(TOMLAsClasses tomlAsClass, string name, string typeName)
    {
        r_tomlAsClasses = tomlAsClass;
        Name = name;
        TypeName = typeName;
    }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="tomlAsClass"></param>
    /// <param name="name">名称</param>
    /// <param name="node">类值(推断类型名称)</param>
    public TOMLClassValue(TOMLAsClasses tomlAsClass, string name, TomlNode node)
    {
        r_tomlAsClasses = tomlAsClass;
        Name = name;
        TypeName = r_tomlAsClasses.r_options.GetConvertName(node, TomlType.GetTypeCode(node));
    }

    /// <summary>
    /// 转化为格式化字符串
    /// </summary>
    /// <returns>字符串</returns>
    public override string ToString()
    {
        var valueData = string.Format(
            r_tomlAsClasses.r_options.PropertyFormat,
            r_tomlAsClasses.r_options.Indent,
            TypeName,
            Name
        );
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
            return string.Format(
                r_tomlAsClasses.r_options.CommentFormat,
                r_tomlAsClasses.r_options.Indent,
                comments[0]
            );
        var multiLineComment =
            comments[0]
            + Environment.NewLine
            + string.Join(
                Environment.NewLine,
                comments[1..].Select(
                    s =>
                        string.Format(
                            r_tomlAsClasses.r_options.CommentParaFormat,
                            r_tomlAsClasses.r_options.Indent,
                            s
                        )
                )
            );
        return string.Format(
            r_tomlAsClasses.r_options.CommentFormat,
            r_tomlAsClasses.r_options.Indent,
            multiLineComment
        );
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
            sb.AppendLine(
                string.Format(
                    r_tomlAsClasses.r_options.AttributeFormat,
                    r_tomlAsClasses.r_options.Indent,
                    attribute
                )
            );
        return sb.ToString();
    }
}
