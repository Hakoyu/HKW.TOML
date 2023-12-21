namespace HKW.HKWTOML.Serializer;

/// <summary>
/// Toml序列化异常
/// </summary>
public class TomlSerializeException : Exception
{
    /// <inheritdoc/>
    /// <param name="message">信息</param>
    public TomlSerializeException(string message)
        : base(message) { }
}
