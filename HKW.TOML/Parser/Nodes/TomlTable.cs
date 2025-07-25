#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using HKW.HKWUtils;

namespace HKW.HKWTOML;

/// <summary>
/// Toml表格
/// </summary>
[DebuggerDisplay("Count = {ChildrenCount}")]
[DebuggerTypeProxy(typeof(HKWUtils.DebugViews.ICollectionDebugView))]
public class TomlTable
    : TomlNode,
        IDictionary<string, TomlNode>,
        IDictionaryWrapper<string, TomlNode, IDictionary<string, TomlNode>>
{
    /// <inheritdoc/>
    public TomlTable()
    {
        RawTable = new Dictionary<string, TomlNode>();
    }

    /// <inheritdoc/>
    /// <param name="rawDictionary">原始字典</param>
    /// <exception cref="ArgumentNullException">dictionary is null</exception>
    public TomlTable(IDictionary<string, TomlNode> rawDictionary)
    {
        ArgumentNullException.ThrowIfNull(rawDictionary);
        RawTable = rawDictionary;
    }

    #region TomlNode
    /// <inheritdoc/>
    public new IEnumerator<KeyValuePair<string, TomlNode>> GetEnumerator() =>
        RawTable.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// 是隐式的
    /// </summary>
    internal bool isImplicit;

    /// <inheritdoc/>
    public override bool HasValue { get; } = false;

    /// <inheritdoc/>
    public override bool IsTomlTable { get; } = true;

    /// <summary>
    /// 在行内
    /// </summary>
    public bool IsInline { get; set; }

    /// <summary>
    /// 原始表格
    /// </summary>
    public IDictionary<string, TomlNode> RawTable { get; }

    /// <inheritdoc/>
    public IDictionary<string, TomlNode> BaseDictionary => RawTable;

    /// <inheritdoc/>
    public override TomlNode this[string key]
    {
        get
        {
            if (RawTable.TryGetValue(key, out var result))
                return result;
            var lazy = new TomlLazy(this);
            RawTable[key] = lazy;
            return lazy;
        }
        set => RawTable[key] = value;
    }

    /// <inheritdoc/>
    public override int ChildrenCount => RawTable.Count;

    /// <inheritdoc/>
    public override ICollection<string> Keys => RawTable.Keys;

    /// <inheritdoc/>
    public override ICollection<TomlNode> Children => RawTable.Values;

    /// <inheritdoc/>
    ICollection<string> IDictionary<string, TomlNode>.Keys => RawTable.Keys;

    /// <inheritdoc/>
    public ICollection<TomlNode> Values => RawTable.Values;

    /// <inheritdoc/>
    public int Count => RawTable.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => RawTable.IsReadOnly;

    /// <inheritdoc/>
    public override bool HasKey(string key) => RawTable.ContainsKey(key);

    /// <inheritdoc/>
    public override void Add(string key, TomlNode node) => RawTable.Add(key, node);

    /// <inheritdoc/>
    public override bool TryGetNode(string key, out TomlNode node) =>
        RawTable.TryGetValue(key, out node!);

    /// <inheritdoc/>
    public override void Delete(TomlNode node) =>
        RawTable.Remove(RawTable.First(kv => kv.Value == node).Key);

    /// <inheritdoc/>
    public override void Delete(string key) => RawTable.Remove(key);

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TomlSyntax.INLINE_TABLE_START_SYMBOL);

        if (ChildrenCount != 0)
        {
            var collapsed = CollectCollapsedItems(normalizeOrder: false);

            if (collapsed.Count != 0)
            {
                sb.Append(' ');
                sb.Append(
                    $"{TomlSyntax.ITEM_SEPARATOR} ".Join(
                        collapsed.Select(n =>
                            $"{n.Key} {TomlSyntax.KEY_VALUE_SEPARATOR} {n.Value.ToInlineToml()}"
                        )
                    )
                );
            }
            sb.Append(' ');
        }

        sb.Append(TomlSyntax.INLINE_TABLE_END_SYMBOL);
        return sb.ToString();
    }
    #endregion

    /// <summary>
    /// 添加多个键值对
    /// </summary>
    /// <param name="table">Toml表格</param>
    public void AddRange(TomlTable table) => AddRange(table.AsDictionary);

    /// <summary>
    /// 添加多个键值对
    /// </summary>
    /// <param name="dic">多个键值对</param>
    public void AddRange(IDictionary<string, TomlNode> dic)
    {
        foreach (var kv in dic)
            RawTable.Add(kv.Key, kv.Value);
    }

    /// <summary>
    /// 收集折叠的项目
    /// </summary>
    /// <param name="prefix">前缀</param>
    /// <param name="level">等级</param>
    /// <param name="normalizeOrder">正常顺序</param>
    /// <returns></returns>
    private LinkedList<KeyValuePair<string, TomlNode>> CollectCollapsedItems(
        string prefix = "",
        int level = 0,
        bool normalizeOrder = true
    )
    {
        var nodes = new LinkedList<KeyValuePair<string, TomlNode>>();
        var postNodes = normalizeOrder ? new LinkedList<KeyValuePair<string, TomlNode>>() : nodes;

        foreach (var kv in RawTable)
        {
            var node = kv.Value;
            var key = kv.Key.AsKey();

            if (node is TomlTable table)
            {
                var subnodes = table.CollectCollapsedItems(
                    $"{prefix}{key}.",
                    level + 1,
                    normalizeOrder
                );
                // Write main table first before writing collapsed items
                if (subnodes.Count == 0 && node.CollapseLevel == level)
                {
                    postNodes.AddLast(new KeyValuePair<string, TomlNode>($"{prefix}{key}", node));
                }
                foreach (var subKV in subnodes)
                    postNodes.AddLast(subKV);
            }
            else if (node.CollapseLevel == level)
                nodes.AddLast(new KeyValuePair<string, TomlNode>($"{prefix}{key}", node));
        }

        if (normalizeOrder)
            foreach (var kv in postNodes)
                nodes.AddLast(kv);

        return nodes;
    }

    #region Save
    /// <summary>
    /// 保存至
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    public void SaveToFile(string tomlFile)
    {
        tomlFile = TOML.AddTOMLExtension(tomlFile);
        using var sw = new StreamWriter(tomlFile);
        WriteTo(sw, null!, false);
    }

    /// <summary>
    /// 异步保存至
    /// </summary>
    /// <param name="tomlFile">Toml文件</param>
    public async Task SaveToFileAsync(string tomlFile)
    {
        tomlFile = TOML.AddTOMLExtension(tomlFile);
        using var sw = new StreamWriter(tomlFile);
        await WriteToAsync(sw, null!, false);
    }

    /// <summary>
    /// 转换为Toml格式文本
    /// </summary>
    /// <returns></returns>
    public string ToTomlString()
    {
        using var ms = new MemoryStream();
        using (var sw = new StreamWriter(ms, leaveOpen: true))
        {
            WriteTo(sw, null!, false);
        }
        ms.Position = 0;
        using var sr = new StreamReader(ms);
        return sr.ReadToEnd();
    }

    /// <summary>
    /// 异步转换为Toml格式文本
    /// </summary>
    /// <returns></returns>
    public async Task<string> ToTomlStringAsync()
    {
        using var ms = new MemoryStream();
        using (var sw = new StreamWriter(ms, leaveOpen: true))
        {
            await WriteToAsync(sw, null!, false);
        }
        ms.Position = 0;
        using var sr = new StreamReader(ms);
        return await sr.ReadToEndAsync();
    }
    #endregion

    /// <summary>
    /// 写入至
    /// </summary>
    /// <param name="tw">文本写入器</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="writeSectionName">写入章节名</param>
    public void WriteTo(TextWriter tw, string? tomlFile, bool writeSectionName = true)
    {
        // The table is inline table
        if (IsInline && tomlFile is not null)
        {
            tw.WriteLine(ToInlineToml());
            return;
        }

        var collapsedItems = CollectCollapsedItems();

        if (collapsedItems.Count is 0)
            return;

        var hasRealValues = !collapsedItems.All(n =>
            n.Value is TomlTable { IsInline: false } or TomlArray { IsTableArray: true }
        );

        if (string.IsNullOrWhiteSpace(Comment) is false)
            Comment.AsComment(tw);

        if (
            tomlFile is not null
            && (hasRealValues || string.IsNullOrWhiteSpace(Comment) is false)
            && writeSectionName
        )
        {
            tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
            tw.Write(tomlFile);
            tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
            tw.WriteLine();
        }
        else if (string.IsNullOrWhiteSpace(Comment) is false)
        {
            tw.WriteLine();
        }

        var namePrefix = tomlFile is null ? string.Empty : $"{tomlFile}.";
        var first = true;

        foreach (var collapsedItem in collapsedItems)
        {
            var key = collapsedItem.Key;
            if (
                collapsedItem.Value
                is TomlArray { IsTableArray: true }
                    or TomlTable { IsInline: false }
            )
            {
                if (first is false)
                    tw.WriteLine();
                first = false;
                collapsedItem.Value.WriteTo(tw, $"{namePrefix}{key}");
                continue;
            }
            first = false;
            if (string.IsNullOrWhiteSpace(collapsedItem.Value.Comment) is false)
                collapsedItem.Value.Comment.AsComment(tw);
            tw.Write(key);
            tw.Write(' ');
            tw.Write(TomlSyntax.KEY_VALUE_SEPARATOR);
            tw.Write(' ');

            collapsedItem.Value.WriteTo(tw, $"{namePrefix}{key}");
        }
    }

    /// <summary>
    /// 异步写入至
    /// </summary>
    /// <param name="tw">文本写入器</param>
    /// <param name="tomlFile">Toml文件</param>
    /// <param name="writeSectionName">写入章节名</param>
    public async Task WriteToAsync(TextWriter tw, string? tomlFile, bool writeSectionName = true)
    {
        if (IsInline && tomlFile is not null)
        {
            await tw.WriteLineAsync(ToInlineToml());
            return;
        }

        var collapsedItems = CollectCollapsedItems();

        if (collapsedItems.Count is 0)
            return;

        var hasRealValues = !collapsedItems.All(n =>
            n.Value is TomlTable { IsInline: false } or TomlArray { IsTableArray: true }
        );

        if (string.IsNullOrWhiteSpace(Comment) is false)
            await Comment.AsCommentAsync(tw);

        if (
            tomlFile is not null
            && (hasRealValues || string.IsNullOrWhiteSpace(Comment) is false)
            && writeSectionName
        )
        {
            await tw.WriteAsync(TomlSyntax.ARRAY_START_SYMBOL);
            await tw.WriteAsync(tomlFile);
            await tw.WriteAsync(TomlSyntax.ARRAY_END_SYMBOL);
            await tw.WriteLineAsync();
        }
        else if (string.IsNullOrWhiteSpace(Comment) is false)
        {
            await tw.WriteLineAsync();
        }

        var namePrefix = tomlFile is null ? string.Empty : $"{tomlFile}.";
        var first = true;

        foreach (var collapsedItem in collapsedItems)
        {
            var key = collapsedItem.Key;
            if (collapsedItem.Value is TomlArray { IsTableArray: true })
            {
                if (first is false)
                    await tw.WriteLineAsync();
                first = false;
                collapsedItem.Value.WriteTo(tw, $"{namePrefix}{key}");
                continue;
            }
            else if (collapsedItem.Value is TomlTable { IsInline: false } table)
            {
                if (first is false)
                    await tw.WriteLineAsync();
                first = false;
                await table.WriteToAsync(tw, $"{namePrefix}{key}", true);
                continue;
            }

            first = false;
            if (string.IsNullOrWhiteSpace(collapsedItem.Value.Comment) is false)
                await collapsedItem.Value.Comment.AsCommentAsync(tw);
            await tw.WriteAsync(key);
            await tw.WriteAsync(' ');
            await tw.WriteAsync(TomlSyntax.KEY_VALUE_SEPARATOR);
            await tw.WriteAsync(' ');

            collapsedItem.Value.WriteTo(tw, $"{namePrefix}{key}");
        }
    }

    #region IDictionary
    /// <inheritdoc/>
    public bool ContainsKey(string key) => RawTable.ContainsKey(key);

    /// <inheritdoc/>
    public bool Remove(string key) => RawTable.Remove(key);

    /// <inheritdoc/>
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out TomlNode value) =>
        RawTable.TryGetValue(key, out value);

    /// <inheritdoc/>
    public void Add(KeyValuePair<string, TomlNode> item) => RawTable.Add(item);

    /// <inheritdoc/>
    public void Clear() => RawTable.Clear();

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<string, TomlNode> item) => RawTable.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<string, TomlNode>[] array, int arrayIndex) =>
        RawTable.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<string, TomlNode> item) => RawTable.Remove(item);

    #endregion
}
