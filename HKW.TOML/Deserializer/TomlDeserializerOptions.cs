using System.ComponentModel;
using System.Reflection;

namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化设置
/// </summary>
public class TomlDeserializerOptions
{
    /// <summary>
    /// 允许非公有属性
    /// </summary>
    [DefaultValue(false)]
    public bool AllowNonPublicProperty { get; set; } = false;

    /// <summary>
    /// 允许静态属性
    /// </summary>
    [DefaultValue(false)]
    public bool AllowStaticProperty { get; set; } = false;

    /// <summary>
    /// 枚举转换时忽略大小写
    /// </summary>
    [DefaultValue(false)]
    public bool EnumIgnoreCase { get; set; } = false;

    /// <summary>
    /// 抛出异常
    /// </summary>
    [DefaultValue(true)]
    public bool ThrowException { get; set; } = true;
}
