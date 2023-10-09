#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion


namespace HKW.HKWTOML;

/// <summary>
/// Toml格式化错误
/// </summary>
public class TomlFormatException : Exception
{
    /// <summary>
    /// Toml格式化错误
    /// </summary>
    /// <param name="message">信息</param>
    public TomlFormatException(string message)
        : base(message) { }
}
