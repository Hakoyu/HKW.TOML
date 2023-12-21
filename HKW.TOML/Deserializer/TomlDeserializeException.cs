namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化异常
/// </summary>
public class TomlDeserializeException : Exception
{
    /// <inheritdoc/>
    /// <param name="message">信息</param>
    public TomlDeserializeException(string message)
        : base(message) { }
}
