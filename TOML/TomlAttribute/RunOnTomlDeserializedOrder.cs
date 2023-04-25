using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.TOML.TomlAttribute;

/// <summary>
/// 反序列化后的方法运行顺序
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RunOnTomlDeserializedOrderAttribute : Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Value { get; }

    /// <inheritdoc/>
    public RunOnTomlDeserializedOrderAttribute(int order) => Value = order;
}
