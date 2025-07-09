namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml属性名称
/// <para>指定Toml键的名称</para>
/// </summary>
/// <param name="name">名称</param>
[AttributeUsage(AttributeTargets.Property)]
public class TomlPropertyNameAttribute(string name) : Attribute
{
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; } = name;
}
