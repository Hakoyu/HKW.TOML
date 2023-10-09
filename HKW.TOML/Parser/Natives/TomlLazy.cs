#region TOML Official Site

// https://toml.io
// Original project
// https://github.com/dezhidki/Tommy

#endregion

namespace HKW.HKWTOML;

internal class TomlLazy : TomlNode
{
    private readonly TomlNode parent;
    private TomlNode replacement = null!;

    public TomlLazy(TomlNode parent) => this.parent = parent;

    /// <inheritdoc/>
    public override TomlNode this[int index]
    {
        get => Set<TomlArray>()[index];
        set => Set<TomlArray>()[index] = value;
    }

    /// <inheritdoc/>
    public override TomlNode this[string key]
    {
        get => Set<TomlTable>()[key];
        set => Set<TomlTable>()[key] = value;
    }

    /// <inheritdoc/>
    public override void Add(TomlNode node) => Set<TomlArray>().Add(node);

    /// <inheritdoc/>
    public override void Add(string key, TomlNode node) => Set<TomlTable>().Add(key, node);

    /// <inheritdoc/>
    public override void AddRange(IEnumerable<TomlNode> nodes) => Set<TomlArray>().AddRange(nodes);

    /// <summary>
    /// 设置Toml节点
    /// </summary>
    /// <typeparam name="T">TomlNode</typeparam>
    /// <returns>Toml节点</returns>
    private TomlNode Set<T>()
        where T : TomlNode, new()
    {
        if (replacement is not null)
            return replacement;

        var newNode = new T { Comment = Comment };

        if (parent.IsTomlTable)
        {
            var key = parent.Keys.FirstOrDefault(
                s => parent.TryGetNode(s, out var node) && node.Equals(this)
            );
            if (key == null)
                return default(T)!;

            parent[key] = newNode;
        }
        else if (parent.IsTomlArray)
        {
            var index = parent.Children.TakeWhile(child => child != this).Count();
            if (index == parent.ChildrenCount)
                return default(T)!;
            parent[index] = newNode;
        }
        else
        {
            return default(T)!;
        }

        replacement = newNode;
        return newNode;
    }
}
