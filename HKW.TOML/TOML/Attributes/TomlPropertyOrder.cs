using System;

namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml参数顺序
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlPropertyOrderAttribute : Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Value { get; }

    /// <inheritdoc/>
    public TomlPropertyOrderAttribute(int order) => Value = order;
}
