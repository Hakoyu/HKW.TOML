using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.TOML.TomlAttribute;

/// <summary>
/// 反序列化前的方法运行顺序
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
internal class RunOnTomlDeserializingOrderAttribute : Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Value { get; }

    /// <inheritdoc/>
    public RunOnTomlDeserializingOrderAttribute(int order) => Value = order;
}
