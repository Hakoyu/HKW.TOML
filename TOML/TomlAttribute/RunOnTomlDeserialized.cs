using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.TOML.TomlAttribute;

/// <summary>
/// 运行于反序列化后
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RunOnTomlDeserializedAttribute : Attribute
{
    /// <summary>
    /// 多个值
    /// </summary>
    public object[]? Values { get; }

    /// <inheritdoc/>
    public RunOnTomlDeserializedAttribute() { }

    /// <inheritdoc/>
    public RunOnTomlDeserializedAttribute(params object[] values)
    {
        Values = values;
    }
}
