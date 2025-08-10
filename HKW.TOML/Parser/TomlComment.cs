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
                throw new TomlFormatException("Line breaks cannot be added to inline comments");

            _inlineComment = value;
        }
    }

    /// <summary>
    /// 有顶部注释
    /// </summary>
    public bool HasPrecedingComment => string.IsNullOrWhiteSpace(PrecedingComment) is false;

    /// <summary>
    /// 有行内注释
    /// </summary>
    public bool HasInlineComment => string.IsNullOrWhiteSpace(InlineComment) is false;

    /// <summary>
    /// 有注释
    /// </summary>
    public bool HasComments => HasPrecedingComment || HasInlineComment;

    /// <inheritdoc/>
    public override string ToString()
    {
        if (HasInlineComment)
            return $"{PrecedingComment}{Environment.NewLine}{InlineComment}";
        else
            return $"{PrecedingComment}";
    }
}
