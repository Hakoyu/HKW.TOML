using System;

namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml参数顺序
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TOMLPropertyOrderAttribute : Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Value { get; }

    /// <inheritdoc/>
    public TOMLPropertyOrderAttribute(int order) => Value = order;
}
