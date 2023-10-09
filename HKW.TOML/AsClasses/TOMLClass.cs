using System.Diagnostics;
using System.Text;

namespace HKW.HKWTOML.AsClasses;

/// <summary>
/// Toml构造类
/// </summary>
[DebuggerDisplay("{Name}, Count = {Count}")]
internal class TOMLClass
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
    /// 是匿名类
    /// </summary>
    public bool IsAnonymous { get; set; } = false;

    /// <summary>
    /// 值字典
    /// <para>(值名称, 值)</para>
    /// </summary>
    public Dictionary<string, TOMLClassValue> Values { get; set; } = new();

    /// <summary>
    /// 特性
    /// </summary>
    public HashSet<string> Attributes { get; set; } = new();

    private readonly TOMLAsClasses _tomlAsClasses;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="tomlAsClasses"></param>
    /// <param name="name">名称</param>
    /// <param name="parentName">父类名称</param>
    public TOMLClass(TOMLAsClasses tomlAsClasses, string name, string? parentName)
    {
        _tomlAsClasses = tomlAsClasses;
        Name = name;
        FullName = name + parentName;
        ParentName = parentName ?? string.Empty;
        IsAnonymous = parentName is not null && string.IsNullOrWhiteSpace(parentName);
    }

    /// <summary>
    /// 转化为格式化字符串
    /// </summary>
    /// <returns>格式化字符串</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        var classname = Name;
        var iTomlClassCommentValue = string.Empty;
        // 为匿名函数时,不设置注释,特性,继承
        if (IsAnonymous is false)
        {
            if (
                GetComment(Comment) is string comment && string.IsNullOrWhiteSpace(comment) is false
            )
                sb.AppendLine(comment);
            if (
                GetAttribute(_tomlAsClasses._options.ClassAttributes) is string attribute
                && string.IsNullOrWhiteSpace(attribute) is false
            )
                sb.AppendLine(attribute);
            classname += GetInheritance(_tomlAsClasses._options.MultipleInheritance);
            // 添加ITomlClass接口中的值
            if (_tomlAsClasses._options.AddITomlClassCommentInterface)
                iTomlClassCommentValue =
                    string.Format(
                        _tomlAsClasses._options.ITomlClassInterfaceValueFormat,
                        _tomlAsClasses._options.Indent
                    ) + Environment.NewLine;
        }

        return string.Format(
            _tomlAsClasses._options.ClassFormat,
            sb.ToString(),
            classname,
            iTomlClassCommentValue + GetValues(Values.Values)
        );
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
            return string.Format(_tomlAsClasses._options.CommentFormat, string.Empty, comments[0]);
        var multiLineComment =
            comments[0]
            + Environment.NewLine
            + string.Join(
                Environment.NewLine,
                comments[1..].Select(
                    s => string.Format(_tomlAsClasses._options.CommentParaFormat, string.Empty, s)
                )
            );
        return string.Format(_tomlAsClasses._options.CommentFormat, string.Empty, multiLineComment);
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
                string.Format(_tomlAsClasses._options.AttributeFormat, string.Empty, attribute)
            );
        return sb.ToString();
    }

    /// <summary>
    /// 获取继承数据
    /// </summary>
    /// <param name="inheritances">继承</param>
    /// <returns>格式化的继承数据</returns>
    private string GetInheritance(IEnumerable<string> inheritances)
    {
        var str = string.Join(", ", inheritances);
        if (string.IsNullOrWhiteSpace(str))
            return string.Empty;
        return string.Format(_tomlAsClasses._options.InheritanceFormat, str);
    }

    /// <summary>
    /// 获取值数据
    /// </summary>
    /// <param name="values">值</param>
    /// <returns>格式化的值数据</returns>
    private static string GetValues(IEnumerable<TOMLClassValue> values)
    {
        var sb = new StringBuilder();
        sb.AppendJoin(Environment.NewLine + Environment.NewLine, values);
        return sb.ToString();
    }
}
