namespace HKW.TOML.TomlAttribute;

/// <summary>
/// Toml键名称
/// <para>指定Toml键的名称</para>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TomlPropertyNameAttribute : Attribute
{
    /// <summary>
    /// 键名
    /// </summary>
    public string Value { get; }

    /// <inheritdoc/>
    public TomlPropertyNameAttribute(string name) => Value = name;
}
