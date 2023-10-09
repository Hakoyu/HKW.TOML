using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HKW.HKWTOML;

namespace HKWTOML.Utils;

internal static class TOMLUtils
{
    public const string _propertyGetMethodStartsWith = "get_";
    public const string _propertySetMethodStartsWith = "set_";

    /// <summary>
    /// 获取类的所有方法(排除属性生成的get,set方法)
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>所有方法</returns>
    public static IEnumerable<MethodInfo> GetRuntimeMethodsNotContainProperty(Type type)
    {
        return type.GetRuntimeMethods()
            .Where(
                m =>
                    (
                        m.Name.StartsWith(_propertyGetMethodStartsWith)
                        || m.Name.StartsWith(_propertySetMethodStartsWith)
                    )
                        is false
            );
    }

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
