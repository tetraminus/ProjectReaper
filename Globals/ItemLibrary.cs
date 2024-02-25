using System;
using System.Linq;
using Godot;
using Godot.Collections;
using ProjectReaper.Items;

namespace ProjectReaper.Globals;

public partial class ItemLibrary : Node
{
    /// <summary>
    ///     Event for when the item library is loaded
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
    ///     Reflected load of all items in the assembly
    /// </summary>
    public void LoadBaseItems()
    {
        
        var types = GetType().Assembly.GetTypes();
        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(AbstractItem)) && !type.IsAbstract)
            {
                if (type.GetCustomAttributes(typeof(IgnoreAutoaddAttribute), true).Length > 0) continue;
                var item = (AbstractItem) Activator.CreateInstance(type);
                if (item == null || item.Id == null)
                {
                    GD.PrintErr($"Item with type {type} has no id, skipping");
                    continue;
                }
                if (Items.ContainsKey(item.Id))
                {
                    GD.PrintErr($"Item with id {item.Id} already exists, skipping");
                    continue;
                }
                Items.Add(item.Id, item);
                GD.Print($"Loaded item {item.Id}");
            }
        }
    }
    
    /// <summary>
    ///    Roll an item from the library
    /// </summary>
    /// <returns>A random item from the library</returns>
    public AbstractItem RollItem()
    {
        var random = new RandomNumberGenerator();
        random.Randomize();
        var index = random.RandiRange(0, Items.Count - 1);
        return CreateItem(Items.Keys.ToArray()[index]);
    }

    /// <summary>
    ///    Create an item from the library
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A new item with the given id</returns>
    public AbstractItem CreateItem(string id)
    {
        if (Items.ContainsKey(id)) return Items[id].MakeCopy();
        return null;
    }

    /// <summary>
    ///    Get an item from the library
    /// </summary>
    ///  <param name="id"></param>
    ///  <returns>The library copy of item with the given id, DON'T MESS WITH</returns>
    public AbstractItem GetItem(string id)
    {
        if (Items.TryGetValue(id, out var item)) return item;
        return null;
    }
    
    
    /// <summary>
    ///     Get the id of the item
    /// </summary>
    /// <typeparam name="T">The type of the item</typeparam>
    /// <returns>The id of the item</returns>
    public string GetId<T>() where T : AbstractItem
    {
        foreach (var item in Items)
            if (item.Value is T)
                return item.Key;
        
        return null;
    }
    
    public class IgnoreAutoaddAttribute : Attribute
    {
        
    }
}