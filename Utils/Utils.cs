using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HKW.TOML;

namespace HKWToml.Utils;

internal static class Utils
{
    public const string c_propertyGetMethodStartsWith = "get_";
    public const string c_propertySetMethodStartsWith = "set_";

    /// <summary>
    /// 获取类的所有方法(排除属性生成的get,set方法)
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>所有方法</returns>
    public static IEnumerable<MethodInfo> GetMethodsWithOutProperty(Type type)
    {
        return type.GetRuntimeMethods()
            .Where(
                m =>
                    (
                        m.Name.StartsWith(c_propertyGetMethodStartsWith)
                        || m.Name.StartsWith(c_propertySetMethodStartsWith)
                    )
                        is false
            );
    }

    /// <summary>
    /// 将字符串转换为帕斯卡格式
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="keyWordSeparators">单词间隔符</param>
    /// <param name="removeKeyWordSeparator">删除单词间隔符</param>
    /// <returns>帕斯卡格式字符串</returns>
    public static string ToPascal(string str, char[] keyWordSeparators, bool removeKeyWordSeparator)
    {
        if (string.IsNullOrWhiteSpace(str) || removeKeyWordSeparator is false)
            return str;
        // 使用分隔符拆分单词
        var strs = str.Split(keyWordSeparators, StringSplitOptions.RemoveEmptyEntries);
        // 将单词首字母大写
        var newStrs = strs.Select(s => FirstLetterToUpper(s));
        return string.Join("", newStrs);
    }

    /// <summary>
    /// 将字符串首字母大写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>第一个为大写的字符串</returns>
    public static string FirstLetterToUpper(string str) => $"{char.ToUpper(str[0])}{str[1..]}";

    /// <summary>
    /// csharp关键字集合
    /// </summary>
    public static IReadOnlySet<string> CsharpKeywords =>
        new HashSet<string>()
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while"
        };
}
