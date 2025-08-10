namespace HKW.HKWTOML.Serializer;

/// <summary>
/// Toml序列化异常
/// </summary>
/// <inheritdoc/>
public class TomlSerializeException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    /// <summary>
    /// 属性异常
    /// </summary>
    public List<TomlSerializePropertyException> Exceptions { get; set; } = [];
}

/// <summary>
/// Toml序列化属性异常
/// </summary>
public class TomlSerializePropertyException(string message, Exception innerException)
    : Exception(message, innerException)
{
    /// <summary>
    /// 属性全名
    /// </summary>
    public string PropertyFullName { get; set; } = string.Empty;
}
