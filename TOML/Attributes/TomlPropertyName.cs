using System;

namespace HKW.TOML.Attributes;

/// <summary>
/// Toml属性名称
/// <para>指定Toml键的名称</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlPropertyNameAttribute : Attribute
{
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; }

    /// <inheritdoc/>
    public TomlPropertyNameAttribute(string name) => Value = name;
}
