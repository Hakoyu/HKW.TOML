namespace HKW.HKWTOML.Attributes;

/// <summary>
/// Toml参数顺序
/// </summary>
/// <param name="order">顺序</param>
[AttributeUsage(AttributeTargets.Property)]
public class TomlPropertyOrderAttribute(int order) : Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Value { get; } = order;
}
