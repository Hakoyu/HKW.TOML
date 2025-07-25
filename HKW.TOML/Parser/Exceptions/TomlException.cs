#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion


namespace HKW.HKWTOML;

/// <summary>
/// Toml错误
/// </summary>
public class TomlException : Exception
{
    /// <summary>
    /// Toml格式化错误
    /// </summary>
    /// <param name="message">信息</param>
    public TomlException(string message)
        : base(message) { }
}
