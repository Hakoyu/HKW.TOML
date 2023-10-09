#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

namespace HKW.HKWTOML;

/// <summary>
/// TOML
/// </summary>
public static class TOML
{
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
        using var parser = new TOMLParser(reader) { ForceASCII = ForceASCII };
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
        using var parser = new TOMLParser(sr) { ForceASCII = ForceASCII };
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
        using var parser = new TOMLParser(reader) { ForceASCII = ForceASCII };
        return parser.Parse();
    }

    /// <summary>
    /// 从文本读取器异步解析
    /// </summary>
    /// <param name="reader">读取器</param>
    /// <returns>解析完成的Toml表格</returns>
    public static async Task<TomlTable> ParseAsync(TextReader reader)
    {
        return await Task.Run(() => Parse(reader));
    }

    /// <summary>
    /// 从字符串异步解析
    /// </summary>
    /// <param name="tomlData">Toml数据</param>
    /// <returns>解析完成的Toml表格</returns>
    public static async Task<TomlTable> ParseAsync(string tomlData)
    {
        return await Task.Run(() => Parse(tomlData));
    }

    /// <summary>
    /// 从文件异步解析
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    /// <returns>解析完成的Toml表格</returns>
    public static async Task<TomlTable> ParseFromFileAsync(string tomlFile)
    {
        return await Task.Run(() => ParseFromFile(tomlFile));
    }
}
