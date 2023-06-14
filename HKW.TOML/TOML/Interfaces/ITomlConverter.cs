using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Serializer;

namespace HKW.HKWTOML.Interfaces;

/// <summary>
/// Toml值转换接口
/// </summary>
/// <typeparam name="T">值类型</typeparam>
public interface ITomlConverter<T>
    where T : notnull
{
    /// <summary>
    /// 从Toml节点读取值 用于 <see cref="Deserializer.TomlDeserializer"/>
    /// </summary>
    /// <param name="node">Toml节点</param>
    /// <returns>转换后的值</returns>
    public T Read(TomlNode node);

    /// <summary>
    /// 从值转换成Toml节点 用于 <see cref="Serializer.TomlSerializer"/>
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>转换后的Toml节点</returns>
    public TomlNode Write(T value);
}
