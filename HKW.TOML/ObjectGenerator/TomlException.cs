#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion


namespace HKW.HKWTOML.ObjectGenerator;

/// <summary>
/// Toml错误
/// </summary>
/// <remarks>
/// Toml格式化错误
/// </remarks>
/// <param name="message">信息</param>
public class TomlObjectGeneratorException(string message) : Exception(message) { }
