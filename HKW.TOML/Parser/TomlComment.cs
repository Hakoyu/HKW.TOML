using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWUtils;

namespace HKW.HKWTOML;

/// <summary>
/// Toml注释
/// </summary>
public class TomlComment
{
    /// <summary>
    /// 顶部注释
    /// </summary>
    public string PrecedingComment { get; set; } = string.Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal string _inlineComment = string.Empty;

    /// <summary>
    /// 行内注释
    /// </summary>
    public string InlineComment
    {
        get => _inlineComment;
        set
        {
            if (value.Any(TomlSyntax.IsNewLine))
                throw new TomlFormatException("行内注释无法添加换行符");

            _inlineComment = value;
        }
    }

    /// <summary>
    /// 有注释
    /// </summary>
    public bool HasComments =>
        string.IsNullOrWhiteSpace(PrecedingComment) is false
        || string.IsNullOrWhiteSpace(InlineComment) is false;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{PrecedingComment}{Environment.NewLine}{InlineComment}";
    }
}


//public class TomlCommentData
//{
//    private string? _inlineComment;

//    public string? PrecedingComment { get; set; }

//    public string? InlineComment
//    {
//        get
//        {
//            return _inlineComment;
//        }
//        set
//        {
//            if (value == null)
//            {
//                _inlineComment = null;
//                return;
//            }

//            if (value.Contains("\n") || value.Contains("\r"))
//            {
//                throw new TomlNewlineInInlineCommentException();
//            }

//            _inlineComment = value;
//        }
//    }

//    public bool ThereAreNoComments
//    {
//        get
//        {
//            if (InlineComment == null)
//            {
//                return PrecedingComment == null;
//            }

//            return false;
//        }
//    }

//    internal string FormatPrecedingComment(int indentCount = 0)
//    {
//        if (PrecedingComment == null)
//        {
//            throw new Exception("Preceding comment is null");
//        }

//        StringBuilder stringBuilder = new StringBuilder();
//        string[] array = PrecedingComment.Split('\n');
//        bool flag = true;
//        string[] array2 = array;
//        foreach (string value in array2)
//        {
//            if (!flag)
//            {
//                stringBuilder.Append('\n');
//            }

//            flag = false;
//            string value2 = new string('\t', indentCount);
//            stringBuilder.Append(value2).Append("# ").Append(value);
//        }

//        return stringBuilder.ToString();
//    }
//}
