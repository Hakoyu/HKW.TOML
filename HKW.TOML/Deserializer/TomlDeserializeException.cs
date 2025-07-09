namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化异常
/// </summary>
/// <inheritdoc/>
/// <param name="message">信息</param>
public class TomlDeserializeException(string message) : Exception(message) { }
