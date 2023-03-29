using System.Diagnostics;

namespace HKW.Libs.TOML;
[DebuggerDisplay("Type = {TypeName},Name = {Name}")]
public class TomlClassValue
{
    public string TypeName { get; private set; }
    public string Name { get; private set; }
    public TomlClassValue(string name, string typeName)
    {
        Name = name;
        TypeName = typeName;
    }
    public TomlClassValue(string name, TomlNode node)
    {
        Name = name;
        TypeName = GetTypeName(node);
    }
    static string GetTypeName(TomlNode node)
    {
        return node switch
        {
            { IsTomlBoolean: true } => "bool",
            { IsTomlString: true } => "string",
            { IsTomlFloat: true } => "double",
            { IsTomlInteger: true } => "int",
            { IsTomlDateTimeLocal: true } => "DateTime",
            { IsTomlDateTimeOffset: true } => "DateTimeOffset",
            { IsTomlDateTime: true } => "DateTime",
            _ => throw new ArgumentOutOfRangeException(nameof(node), node, null)
        };
    }

    public override string ToString() => $"    public {TypeName} {Name} {{ get; set; }}";
}