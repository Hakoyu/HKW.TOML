namespace HKW.HKWTOML.Deserializer;

/// <summary>
/// Toml反序列化异常
/// </summary>
/// <inheritdoc/>
public class TomlDeserializeException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    /// <summary>
    /// 属性异常
    /// </summary>
    public List<TomlDeserializePropertyException> Exceptions { get; set; } = [];

    /// <summary>
    /// 缺失的必要属性
    /// </summary>
    public HashSet<string> MissingRequiredProperties { get; set; } = [];
}

/// <summary>
/// Toml反序列化属性异常
/// </summary>
/// <inheritdoc/>
public class TomlDeserializePropertyException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    /// <summary>
    /// 属性全名
    /// </summary>
    public string PropertyFullName { get; set; } = string.Empty;
}
