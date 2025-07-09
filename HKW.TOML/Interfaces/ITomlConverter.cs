using System.Text.Json.Serialization;
using HKW.HKWTOML.Deserializer;
using HKW.HKWTOML.Serializer;

namespace HKW.HKWTOML.Interfaces;

/// <summary>
/// Toml值转换接口
/// </summary>
public interface ITomlConverter
{
    /// <summary>
    /// 从Toml节点读取值
    /// </summary>
    /// <param name="node">Toml节点</param>
    /// <returns>转换后的值</returns>
    public object Converte(TomlNode node);

    /// <summary>
    /// 从值转换成Toml节点
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>转换后的Toml节点</returns>
    public TomlNode ConverteBack(object value);
}
