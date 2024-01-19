using HKW.HKWTOML.Interfaces;

namespace HKW.HKWTOML.Utils;

/// <summary>
/// Toml值转换基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class TomlConverterBase<T> : ITomlConverter
{
    /// <inheritdoc cref="ITomlConverter.Converte(TomlNode)"/>
    public abstract T Converte(TomlNode node);

    /// <inheritdoc cref="ITomlConverter.ConverteBack(object)"/>
    public abstract TomlNode ConverteBack(T value);

    object ITomlConverter.Converte(TomlNode node)
    {
        throw new NotImplementedException();
    }

    TomlNode ITomlConverter.ConverteBack(object value)
    {
        throw new NotImplementedException();
    }
}
