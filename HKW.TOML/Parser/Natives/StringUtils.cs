#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Globalization;
using System.Text;

namespace HKW.HKWTOML;

/// <summary>
/// 字符串工具
/// </summary>
internal static class StringUtils
{
    /// <summary>
    /// 转换为键
    /// </summary>
    /// <param name="key">值</param>
    /// <returns></returns>
    public static string AsKey(this string key)
    {
        var quote = key == string.Empty || key.Any(c => !TomlSyntax.IsBareKey(c));
        return quote is false
            ? key
            : $"{TomlSyntax.BASI_STRING_SYMBOL}{key.Escape()}{TomlSyntax.BASI_STRING_SYMBOL}";
    }

    /// <summary>
    /// 加入值
    /// </summary>
    /// <param name="self"></param>
    /// <param name="subItems"></param>
    /// <returns></returns>
    public static string Join(this string self, IEnumerable<string> subItems)
    {
        var sb = new StringBuilder();
        var first = true;

        foreach (var subItem in subItems)
        {
            if (first is false)
                sb.Append(self);
            first = false;
            sb.Append(subItem);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 尝试解析日期时间委托
    /// </summary>
    /// <typeparam name="T">日期时间</typeparam>
    /// <param name="s">字符串</param>
    /// <param name="format">格式</param>
    /// <param name="ci">样式供应器</param>
    /// <param name="dts">日期时间风格</param>
    /// <param name="dt">日期时间</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public delegate bool TryDateParseDelegate<T>(
        string s,
        string format,
        IFormatProvider ci,
        DateTimeStyles dts,
        out T dt
    );

    /// <summary>
    /// 尝试解析日期时间
    /// </summary>
    /// <typeparam name="T">日期时间</typeparam>
    /// <param name="s">字符串</param>
    /// <param name="formats">格式</param>
    /// <param name="styles">日期时间格式</param>
    /// <param name="parser">尝试解析委托</param>
    /// <param name="dateTime">日期时间</param>
    /// <param name="parsedFormat">解析格式</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public static bool TryParseDateTime<T>(
        string s,
        string[] formats,
        DateTimeStyles styles,
        TryDateParseDelegate<T> parser,
        out T dateTime,
        out int parsedFormat
    )
    {
        parsedFormat = 0;
        dateTime = default!;
        for (var i = 0; i < formats.Length; i++)
        {
            var format = formats[i];
            if (parser(s, format, CultureInfo.InvariantCulture, styles, out dateTime) is false)
                continue;
            parsedFormat = i;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 转换为注释
    /// </summary>
    /// <param name="self">文本</param>
    /// <param name="tw">文本写入器</param>
    public static void AsComment(this string self, TextWriter tw)
    {
        var array = self.Split(TomlSyntax.NEWLINE_CHARACTER);
        for (int i = 0; i < array.Length; i++)
        {
            string? line = array[i];
            tw.WriteLine($"{TomlSyntax.COMMENT_SYMBOL} {line.Trim()}");
        }
    }

    /// <summary>
    /// 异步转换为注释
    /// </summary>
    /// <param name="self">文本</param>
    /// <param name="tw">文本写入器</param>
    public static async Task AsCommentAsync(this string self, TextWriter tw)
    {
        var array = self.Split(TomlSyntax.NEWLINE_CHARACTER);
        for (int i = 0; i < array.Length; i++)
        {
            string? line = array[i];
            await tw.WriteLineAsync($"{TomlSyntax.COMMENT_SYMBOL} {line.Trim()}");
        }
    }

    /// <summary>
    /// 删除所有指定字符
    /// </summary>
    /// <param name="txt">文本</param>
    /// <param name="toRemove">指定字符</param>
    /// <returns>完成的字符串</returns>
    public static string RemoveAll(this string txt, char toRemove)
    {
        var sb = new StringBuilder(txt.Length);
        foreach (var c in txt.Where(c => c != toRemove))
            sb.Append(c);
        return sb.ToString();
    }

    /// <summary>
    /// 编码
    /// </summary>
    /// <param name="txt">文本</param>
    /// <param name="escapeNewlines">编码至新行</param>
    /// <returns>编码完成的字符串</returns>
    public static string Escape(this string txt, bool escapeNewlines = true)
    {
        var stringBuilder = new StringBuilder(txt.Length + 2);
        for (var i = 0; i < txt.Length; i++)
        {
            var c = txt[i];

            static string CodePoint(string txt, ref int i, char c) =>
                char.IsSurrogatePair(txt, i)
                    ? $"\\U{char.ConvertToUtf32(txt, i++):X8}"
                    : $"\\u{(ushort)c:X4}";

            stringBuilder.Append(
                c switch
                {
                    '\b' => @"\b",
                    '\t' => @"\t",
                    '\n' when escapeNewlines => @"\n",
                    '\f' => @"\f",
                    '\r' when escapeNewlines => @"\r",
                    '\\' => @"\\",
                    '\"' => @"\""",
                    var _
                        when TomlSyntax.MustBeEscaped(c, escapeNewlines is false)
                            || TOML.ForceASCII && c > sbyte.MaxValue
                        => CodePoint(txt, ref i, c),
                    var _ => c
                }
            );
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 尝试解码
    /// </summary>
    /// <param name="txt">文本</param>
    /// <param name="unescaped">解码字符串</param>
    /// <param name="exception">异常</param>
    /// <returns>成功为 <see langword="true"/> 失败为 <see langword="false"/></returns>
    public static bool TryUnescape(this string txt, out string unescaped, out Exception exception)
    {
        try
        {
            exception = null!;
            unescaped = txt.Unescape();
            return true;
        }
        catch (Exception ex)
        {
            exception = ex;
            unescaped = null!;
            return false;
        }
    }

    /// <summary>
    /// 字符串解码
    /// </summary>
    /// <param name="txt">文本</param>
    /// <returns>解码后的字符串</returns>
    /// <exception cref="Exception">没码</exception>
    public static string Unescape(this string txt)
    {
        if (string.IsNullOrWhiteSpace(txt))
            return txt;
        var stringBuilder = new StringBuilder(txt.Length);
        for (var i = 0; i < txt.Length; )
        {
            var num = txt.IndexOf('\\', i);
            var next = num + 1;
            if (num < 0 || num == txt.Length - 1)
                num = txt.Length;
            stringBuilder.Append(txt, i, num - i);
            if (num >= txt.Length)
                break;
            var c = txt[next];

            static string CodePoint(int next, string txt, ref int num, int size)
            {
                if (next + size >= txt.Length)
                    throw new TomlException("Undefined escape sequence!");
                num += size;
                return char.ConvertFromUtf32(Convert.ToInt32(txt.Substring(next + 1, size), 16));
            }

            stringBuilder.Append(
                c switch
                {
                    'b' => "\b",
                    't' => "\t",
                    'n' => "\n",
                    'f' => "\f",
                    'r' => "\r",
                    '\'' => "\'",
                    '\"' => "\"",
                    '\\' => "\\",
                    'u' => CodePoint(next, txt, ref num, 4),
                    'U' => CodePoint(next, txt, ref num, 8),
                    _ => throw new TomlException("Undefined escape sequence!")
                }
            );
            i = num + 2;
        }

        return stringBuilder.ToString();
    }
}
