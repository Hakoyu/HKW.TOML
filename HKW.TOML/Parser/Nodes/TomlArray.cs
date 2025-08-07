#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Diagnostics;
using System.Text;
using HKW.HKWUtils;

namespace HKW.HKWTOML;

/// <summary>
/// Toml数组
/// </summary>
[DebuggerDisplay("Count = {ChildrenCount}")]
[DebuggerTypeProxy(typeof(HKWUtils.DebugViews.ICollectionDebugView))]
public class TomlArray : TomlNode, IList<TomlNode>, IListWrapper<TomlNode, IList<TomlNode>>
{
    /// <inheritdoc/>
    public TomlArray()
    {
        RawArray = [];
    }

    /// <inheritdoc/>
    /// <param name="nodes">节点</param>
    public TomlArray(IEnumerable<TomlNode> nodes)
        : this()
    {
        AddRange(nodes);
    }

    /// <inheritdoc/>
    /// <param name="rawArray">原始数组</param>
    public TomlArray(IList<TomlNode> rawArray)
    {
        ArgumentNullException.ThrowIfNull(rawArray);
        RawArray = rawArray;
    }

    /// <inheritdoc/>
    public new IEnumerator<TomlNode> GetEnumerator() => RawArray.GetEnumerator();

    /// <summary>
    /// 原始值
    /// </summary>
    public IList<TomlNode> RawArray { get; }

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
    public IList<TomlNode> BaseList => RawArray;

    /// <inheritdoc/>
    public override void Add(TomlNode node) => RawArray.Add(node);

    /// <inheritdoc/>
    public override void AddRange(IEnumerable<TomlNode> nodes)
    {
        if (RawArray is List<TomlNode> list)
        {
            list.AddRange(nodes);
        }
        else
        {
            foreach (var tomlNode in nodes)
                Add(tomlNode);
        }
    }

    /// <inheritdoc/>
    public override void Delete(TomlNode node) => RawArray.Remove(node);

    /// <inheritdoc/>
    public override void Delete(int index) => RawArray.RemoveAt(index);

    /// <inheritdoc/>
    public override string ToString() => ToString(IsMultiline);

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
            if (multiline)
            {
                sb.AppendLine();
                var items = new List<string>();
                var maxLength = 1;
                for (var i = 0; i < RawArray.Count; i++)
                {
                    var toml = RawArray[i].ToInlineToml();
                    items.Add(toml);
                    maxLength = Math.Max(toml.Length, maxLength);
                }
                maxLength++;
                // 自适应注释对齐
                for (var i = 0; i < items.Count; i++)
                {
                    var node = RawArray[i];
                    if (string.IsNullOrWhiteSpace(node.Comment.PrecedingComment) is false)
                    {
                        var array = node.Comment.PrecedingComment.Split(
                            TomlSyntax.NEWLINE_CHARACTER
                        );
                        for (var j = 0; j < array.Length; j++)
                        {
                            var line = array[j];
                            sb.AppendLine($"  {TomlSyntax.COMMENT_SYMBOL} {line.Trim()}");
                        }
                    }
                    sb.Append("  ");
                    sb.Append(items[i]);
                    sb.Append(TomlSyntax.ITEM_SEPARATOR);
                    if (string.IsNullOrWhiteSpace(node.Comment.InlineComment) is false)
                    {
                        sb.Append(
                            $"{new string(' ', maxLength - items[i].Length)}{TomlSyntax.COMMENT_SYMBOL} {node.Comment.InlineComment.Trim()}"
                        );
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
            }
            else
            {
                var arrayStart = " ";
                var arraySeparator = $"{TomlSyntax.ITEM_SEPARATOR} ";
                var arrayEnd = " ";
                sb.Append(arrayStart)
                    .Append(arraySeparator.Join(RawArray.Select(n => n.ToInlineToml())))
                    .Append(arrayEnd);
            }
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

        if (string.IsNullOrWhiteSpace(Comment.PrecedingComment) is false)
        {
            tw.WriteLine();
            Comment.WritePrecedingComment(tw);
        }
        tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
        tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
        tw.Write(name);
        tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
        tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
        if (string.IsNullOrWhiteSpace(Comment.InlineComment) is false)
            Comment.WriteInlineComment(tw);
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

                if (string.IsNullOrWhiteSpace(Comment.PrecedingComment) is false)
                    Comment.WritePrecedingComment(tw);
                tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
                tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
                tw.Write(name);
                tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
                tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
                if (string.IsNullOrWhiteSpace(Comment.InlineComment) is false)
                    Comment.WriteInlineComment(tw);
                tw.WriteLine();
            }

            first = false;

            // Don't write section since it's already written here
            tbl.WriteTo(tw, name, false);
        }
    }

    /// <inheritdoc/>
    public override async Task WriteToAsync(TextWriter tw, string name = null!)
    {
        // If it's a normal array, write it as usual
        if (IsTableArray is false)
        {
            await tw.WriteLineAsync(ToString(IsMultiline));
            return;
        }

        if (string.IsNullOrWhiteSpace(Comment.PrecedingComment) is false)
        {
            await tw.WriteLineAsync();
            await Comment.WritePrecedingCommentAsync(tw);
        }
        await tw.WriteAsync(TomlSyntax.ARRAY_START_SYMBOL);
        await tw.WriteAsync(TomlSyntax.ARRAY_START_SYMBOL);
        await tw.WriteAsync(name);
        await tw.WriteAsync(TomlSyntax.ARRAY_END_SYMBOL);
        await tw.WriteAsync(TomlSyntax.ARRAY_END_SYMBOL);
        if (string.IsNullOrWhiteSpace(Comment.InlineComment) is false)
            await Comment.WriteInlineCommentAsync(tw);
        await tw.WriteLineAsync();

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
                await tw.WriteLineAsync();

                if (string.IsNullOrWhiteSpace(Comment.PrecedingComment) is false)
                    await Comment.WritePrecedingCommentAsync(tw);
                await tw.WriteAsync(TomlSyntax.ARRAY_START_SYMBOL);
                await tw.WriteAsync(TomlSyntax.ARRAY_START_SYMBOL);
                await tw.WriteAsync(name);
                await tw.WriteAsync(TomlSyntax.ARRAY_END_SYMBOL);
                await tw.WriteAsync(TomlSyntax.ARRAY_END_SYMBOL);
                if (string.IsNullOrWhiteSpace(Comment.InlineComment) is false)
                    await Comment.WriteInlineCommentAsync(tw);
                await tw.WriteLineAsync();
            }

            first = false;

            // Don't write section since it's already written here
            await tbl.WriteToAsync(tw, name, false);
        }
    }

    #region IList
    /// <inheritdoc/>
    public int Count => RawArray.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => RawArray.IsReadOnly;

    /// <inheritdoc/>
    public int IndexOf(TomlNode item)
    {
        return RawArray.IndexOf(item);
    }

    /// <inheritdoc/>
    public void Insert(int index, TomlNode item)
    {
        RawArray.Insert(index, item);
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        RawArray.RemoveAt(index);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        RawArray.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(TomlNode item)
    {
        return RawArray.Contains(item);
    }

    /// <inheritdoc/>
    public void CopyTo(TomlNode[] array, int arrayIndex)
    {
        RawArray.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public bool Remove(TomlNode item)
    {
        return RawArray.Remove(item);
    }

    #endregion
}
