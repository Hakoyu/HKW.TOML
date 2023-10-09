using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace HKW.HKWTOML.Serializer;

/// <summary>
/// Toml序列化设置
/// </summary>
public class TOMLSerializerOptions
{
    /// <summary>
    /// 属性排序比较器
    /// </summary>
    public IComparer<PropertyInfo>? PropertiesOrderComparer { get; set; }

    /// <summary>
    /// 属性倒序排列
    /// </summary>
    [DefaultValue(false)]
    public bool PropertiesReverseOrder { get; set; } = false;

    /// <summary>
    /// 将枚举转换为 <see cref="TomlInteger"/> 而不是 <see cref="TomlString"/>
    /// </summary>
    [DefaultValue(false)]
    public bool EnumToInteger { get; set; } = false;
}
