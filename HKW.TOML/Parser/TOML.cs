#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Text;

namespace HKW.HKWTOML;

/// <summary>
/// TOML
/// </summary>
public static class TOML
{
    /// <summary>
    /// TOML 文件拓展名
    /// </summary>
    public const string ExtensionName = ".toml";

    /// <summary>
    /// 强制ASCII编码
    /// </summary>
    public static bool ForceASCII { get; set; } = false;

    /// <summary>
    /// 从文本读取器解析
    /// </summary>
    /// <param name="reader">读取器</param>
    /// <returns>解析完成的Toml表格</returns>
    public static TomlTable Parse(TextReader reader)
    {
        using var parser = new TomlParser(reader) { ForceASCII = ForceASCII };
        return parser.Parse();
    }

    /// <summary>
    /// 从字符串解析
    /// </summary>
    /// <param name="tomlData">Toml数据</param>
    /// <returns>解析完成的Toml表格</returns>
    public static TomlTable Parse(string tomlData)
    {
        using var sr = new StringReader(tomlData);
        using var parser = new TomlParser(sr) { ForceASCII = ForceASCII };
        return parser.Parse();
    }

    /// <summary>
    /// 从文件解析
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    /// <returns>解析完成的Toml表格</returns>
    public static TomlTable ParseFromFile(string tomlFile)
    {
        using var reader = File.OpenText(tomlFile);
        using var parser = new TomlParser(reader) { ForceASCII = ForceASCII };
        return parser.Parse();
    }

    /// <summary>
    /// 从文本读取器异步解析
    /// </summary>
    /// <param name="reader">读取器</param>
    /// <returns>解析完成的Toml表格</returns>
    public static Task<TomlTable> ParseAsync(TextReader reader)
    {
        return Task.FromResult(Parse(reader));
    }

    /// <summary>
    /// 从字符串异步解析
    /// </summary>
    /// <param name="tomlData">Toml数据</param>
    /// <returns>解析完成的Toml表格</returns>
    public static Task<TomlTable> ParseAsync(string tomlData)
    {
        return Task.FromResult(Parse(tomlData));
    }

    /// <summary>
    /// 从文件异步解析
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    /// <returns>解析完成的Toml表格</returns>
    public static Task<TomlTable> ParseFromFileAsync(string tomlFile)
    {
        return Task.FromResult(ParseFromFile(tomlFile));
    }

    /// <summary>
    /// 添加TOML文件拓展名
    /// </summary>
    /// <param name="tomlFile">TOML文件</param>
    /// <returns>带有TOML文件拓展名的文件名</returns>
    public static string AddTOMLExtension(string tomlFile)
    {
        if (tomlFile.EndsWith(ExtensionName) is false)
            tomlFile += ExtensionName;
        return tomlFile;
    }

    /// <summary>
    /// 添加TOML文件拓展名
    /// </summary>
    /// <param name="tomlFile">TOML文件</param>
    /// <returns>带有TOML文件拓展名的文件名</returns>
    public static StringBuilder AddTOMLExtension(StringBuilder tomlFile)
    {
        var index = tomlFile.Length - ExtensionName.Length;
        if (index <= 0 || ExtensionName.Any(c => c != tomlFile[index++]))
            return tomlFile.Append(ExtensionName);
        return tomlFile;
    }
}
