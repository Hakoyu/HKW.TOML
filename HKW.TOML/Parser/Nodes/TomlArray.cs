#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Diagnostics;
using System.Text;

namespace HKW.HKWTOML;

/// <summary>
/// Toml数组
/// </summary>
[DebuggerDisplay("Count = {ChildrenCount}")]
[DebuggerTypeProxy(typeof(HKWUtils.DebugViews.ICollectionDebugView))]
public class TomlArray : TomlNode, IEnumerable<TomlNode>
{
    /// <inheritdoc/>
    public new IEnumerator<TomlNode> GetEnumerator() => RawArray.GetEnumerator();

    /// <summary>
    /// 原始值
    /// </summary>
    public List<TomlNode> RawArray { get; private set; } = new();

    /// <inheritdoc/>
    public override bool HasValue { get; } = true;

    /// <inheritdoc/>
    public override bool IsTomlArray { get; } = true;

    /// <summary>
    /// 是多行
    /// </summary>
    public bool IsMultiline { get; set; }

    /// <summary>
    /// 是Toml表格数组
    /// </summary>
    public bool IsTableArray { get; set; }

    /// <inheritdoc/>
    public override TomlNode this[int index]
    {
        get
        {
            if (index < RawArray.Count)
                return RawArray[index];
            var lazy = new TomlLazy(this);
            this[index] = lazy;
            return lazy;
        }
        set
        {
            if (index == RawArray.Count)
                RawArray.Add(value);
            else
                RawArray[index] = value;
        }
    }

    /// <inheritdoc/>
    public override int ChildrenCount => RawArray.Count;

    /// <inheritdoc/>
    public override IEnumerable<TomlNode> Children => RawArray.AsEnumerable();

    /// <inheritdoc/>
    public override void Add(TomlNode node) => RawArray.Add(node);

    /// <inheritdoc/>
    public override void AddRange(IEnumerable<TomlNode> nodes) => RawArray.AddRange(nodes);

    /// <inheritdoc/>
    public override void Delete(TomlNode node) => RawArray.Remove(node);

    /// <inheritdoc/>
    public override void Delete(int index) => RawArray.RemoveAt(index);

    /// <inheritdoc/>
    public override string ToString() => ToString(false);

    /// <summary>
    /// 转化为多行字符串
    /// </summary>
    /// <param name="multiline">是多行</param>
    /// <returns>多行字符串</returns>
    public string ToString(bool multiline)
    {
        var sb = new StringBuilder();
        sb.Append(TomlSyntax.ARRAY_START_SYMBOL);
        if (ChildrenCount != 0)
        {
            var arrayStart = multiline ? $"{Environment.NewLine}  " : " ";
            var arraySeparator = multiline
                ? $"{TomlSyntax.ITEM_SEPARATOR}{Environment.NewLine}  "
                : $"{TomlSyntax.ITEM_SEPARATOR} ";
            var arrayEnd = multiline ? Environment.NewLine : " ";
            sb.Append(arrayStart)
                .Append(arraySeparator.Join(RawArray.Select(n => n.ToInlineToml())))
                .Append(arrayEnd);
        }
        sb.Append(TomlSyntax.ARRAY_END_SYMBOL);
        return sb.ToString();
    }

    /// <inheritdoc/>
    public override void WriteTo(TextWriter tw, string name = null!)
    {
        // If it's a normal array, write it as usual
        if (IsTableArray is false)
        {
            tw.WriteLine(ToString(IsMultiline));
            return;
        }

        if (string.IsNullOrWhiteSpace(Comment) is false)
        {
            tw.WriteLine();
            Comment.AsComment(tw);
        }
        tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
        tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
        tw.Write(name);
        tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
        tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
        tw.WriteLine();

        var first = true;

        foreach (var tomlNode in RawArray)
        {
            if (tomlNode is not TomlTable tbl)
                throw new TomlFormatException(
                    "The array is marked as array table but contains non-table nodes!"
                );

            // Ensure it's parsed as a section
            tbl.IsInline = false;

            if (first is false)
            {
                tw.WriteLine();

                if (string.IsNullOrWhiteSpace(Comment) is false)
                    Comment?.AsComment(tw);
                tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
                tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
                tw.Write(name);
                tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
                tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
                tw.WriteLine();
            }

            first = false;

            // Don't write section since it's already written here
            tbl.WriteTo(tw, name, false);
        }
    }

    /// <inheritdoc/>
    public TomlArray() { }

    /// <inheritdoc/>
    public TomlArray(IEnumerable<TomlNode> nodes)
    {
        RawArray.AddRange(nodes);
    }
}
