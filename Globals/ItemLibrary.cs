using Godot;
using Godot.Collections;
using ProjectReaper.Items;

namespace ProjectReaper.Globals;

public partial class ItemLibrary : Node
{
    /// <summary>
    ///     Event for when the item library is loaded, inject methods here to load items
    /// </summary>
    public delegate void LoadItemEventHandler(ItemLibrary itemLibrary);

    public LoadItemEventHandler LoadItemEvent;
    public static ItemLibrary Instance { get; private set; }
    public Dictionary<string, AbstractItem> Items { get; set; } = new();

    public override void _Ready()
    {
        Instance = this;
        LoadBaseItems();
        LoadItemEvent?.Invoke(this);
    }

    /// <summary>
    ///     Load the base items into the library
    /// </summary>
    public void LoadBaseItems()
    {
        AbstractItem item;

        item = new BoomStick();
        Items.Add(item.Id, item);

        item = new Items.Collectables.DamageDestroyer();
        Items.Add(item.Id, item);
    }

    public AbstractItem CreateItem(string id)
    {
        if (Items.ContainsKey(id)) return Items[id].MakeCopy();
        return null;
    }

    public AbstractItem GetItem(string id)
    {
        if (Items.TryGetValue(id, out var item)) return item;
        return null;
    }
    
    public string GetId<T>() where T : AbstractItem
    {
        foreach (var item in Items)
            if (item.Value is T)
                return item.Key;
        
        return null;
    }
}