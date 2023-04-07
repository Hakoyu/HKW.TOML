using System.Reflection;

namespace HKW.TOML;

/// <summary>
/// Toml序列化设置
/// </summary>
public class TomlSerializerOptions
{
    /// <summary>
    /// 属性排序比较器
    /// </summary>
    public IComparer<PropertyInfo>? PropertiesOrderComparer { get; set; }

    /// <summary>
    /// 属性倒序排列
    /// </summary>
    public bool PropertiesReverseOrder { get; set; } = false;
}