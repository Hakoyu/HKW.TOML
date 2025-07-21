using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using HKW.HKWTOML.Attributes;

namespace HKW.HKWTOML;

internal static class TomlUtils
{
    public static PropertyInfo[] GetPropertiesWithoutIgnore(
        this Type type,
        BindingFlags bindingAttr
    )
    {
        return type.GetProperties(bindingAttr)
            .Where(p =>
                (
                    Attribute.IsDefined(p, typeof(TomlIgnoreAttribute))
                    || Attribute.IsDefined(p, typeof(IgnoreDataMemberAttribute))
                    || Attribute.IsDefined(p, typeof(JsonIgnoreAttribute))
                )
                    is false
            )
            .ToArray();
    }

    /// <summary>
    /// 是静态
    /// </summary>
    /// <param name="propertyInfo">属性信息</param>
    /// <returns>是为 <see langword="true"/> 不是为 <see langword="false"/></returns>
    public static bool IsStatic(this PropertyInfo propertyInfo)
    {
        if (propertyInfo.GetMethod is MethodInfo getMethod)
            return getMethod.IsStatic;
        else if (propertyInfo.SetMethod is MethodInfo setMethod)
            return setMethod.IsStatic;
        else
            return false;
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
