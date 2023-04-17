using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML.TomlDeserializer;
using HKW.TOML.TomlSerializer;

namespace HKW.TOML.TomlInterface;

/// <summary>
/// Toml值转换接口
/// </summary>
/// <typeparam name="T">值类型</typeparam>
public interface ITomlConverter<T>
    where T : notnull
{
    /// <summary>
    /// 从Toml节点读取值 用于 <see cref="TomlDeserializer.TomlDeserializer"/>
    /// </summary>
    /// <param name="node">Toml节点</param>
    /// <returns>转换后的值</returns>
    public T Read(TomlNode node);

    /// <summary>
    /// 从值转换成Toml节点 用于 <see cref="TomlSerializer.TomlSerializer"/>
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>转换后的Toml节点</returns>
    public TomlNode Write(T value);
}
