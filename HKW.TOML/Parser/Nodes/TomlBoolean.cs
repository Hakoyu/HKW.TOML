#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Diagnostics;

namespace HKW.HKWTOML;

/// <summary>
/// Toml布尔类型
/// </summary>
[DebuggerDisplay("{Value}")]
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

    /// <inheritdoc/>
    public TomlBoolean(bool value)
    {
        Value = value;
    }
}
