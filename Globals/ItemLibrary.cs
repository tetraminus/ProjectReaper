using System;
using System.Linq;
using System.Collections.Generic;
using Godot;

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
    public Dictionary<string, AbstractItem> AllItems { get; set; } = new();
    public Dictionary<ItemRarity, List<AbstractItem>> ItemsByRarity { get; set; } = new();
    public RandomNumberGenerator ItemRNG = new();
    public static PackedScene ItemDisplayScene = GD.Load<PackedScene>("res://Player/ItemDisplay.tscn");

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _Process(double delta)
    {
        // hue shift the legendary color for fun
        ItemRarity.Legendary.Color.ToHsv(out var hue, out var s, out var v);
        ItemRarity.Legendary.Color = Color.FromHsv((float)(hue + delta/4) , s, v);
    }

    /// <summary>
    ///     Reflected load of all items in the assembly
    /// </summary>
    public void LoadItems()
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
                if (AllItems.ContainsKey(item.Id))
                {
                    GD.PrintErr($"Item with id {item.Id} already exists, skipping");
                    continue;
                }
                AllItems.Add(item.Id, item);
                if (!ItemsByRarity.ContainsKey(item.Rarity))
                {
                    ItemsByRarity.Add(item.Rarity, new List<AbstractItem>());
                }
                ItemsByRarity[item.Rarity].Add(item);
                GD.Print($"Loaded item {item.Id}");
            }
        }
        
        // print out chances of each rarity, calculated by the weight of each rarity
        var total = ItemsByRarity.Keys.Sum(rarity => rarity.Weight);
        foreach (var rarity in ItemsByRarity.Keys)
        {
            var chance = (float) rarity.Weight / total * 100;
            GD.Print($"{rarity.NameKey} has a {chance}% chance of being rolled");
        }
        
    }
    
    /// <summary>
    ///    Roll an item from the library
    /// </summary>
    /// <returns>A random item from the library</returns>
    public AbstractItem RollItem()
    {
        
        var rarity = RollRarity();
        var items = ItemsByRarity[rarity];
        
        var roll = ItemRNG.RandiRange(0, items.Count - 1);
        return items[roll].MakeCopy();
       
    }
    /// <summary>
    ///  Roll an item from the library with a specific rarity
    /// </summary>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public AbstractItem RollItem(ItemRarity rarity)
    {
        
        var items = ItemsByRarity[rarity];
        
        var roll = ItemRNG.RandiRange(0, items.Count - 1);
        return items[roll].MakeCopy();
       
    }
    
    
    /// <summary>
    ///  Roll a rarity from the library
    /// </summary>
    /// <returns></returns>
    public ItemRarity RollRarity(bool usePlayerLuck = true, RandomNumberGenerator rng = null)
    {
        if (rng == null)
        {
            rng = ItemRNG;
        }
        var rerolls = usePlayerLuck ? GameManager.Player.Stats.Luck + 1 : 1;
        var rarities = ItemsByRarity.Keys
            .Where(rarity => rarity.AvailableInChests && ItemsByRarity[rarity].Count > 0)
            .OrderBy(rarity => rarity.Value)
            .ToList();
        var total = rarities.Sum(rarity => rarity.Weight);
        var roll = rng.RandfRange(0, total);
        ItemRarity bestRarity = null;
        for (int i = 0; i < rerolls; i++)
        {
            var current = 0f;
            foreach (var rarity in rarities)
            {
                current += rarity.Weight;
                if (roll < current)
                {
                    if (bestRarity == null || rarity.Value > bestRarity.Value)
                    {
                        bestRarity = rarity;
                    }
                    break;
                }
            }
        }

        return bestRarity;
    }
    
    

    /// <summary>
    ///    Create an item from the library
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A new item with the given id</returns>
    public AbstractItem CreateItem(string id)
    {
        if (AllItems.ContainsKey(id))
        {
            return AllItems[id].MakeCopy();
        }
        return null;
    }

    /// <summary>
    ///    Get an item from the library
    /// </summary>
    ///  <param name="id"></param>
    ///  <returns>The library copy of item with the given id, DON'T MESS WITH</returns>
    public AbstractItem GetItem(string id)
    {
        if (AllItems.TryGetValue(id, out var item)) return item;
        return null;
    }
    
    
    /// <summary>
    ///     Get the id of the item
    /// </summary>
    /// <typeparam name="T">The type of the item</typeparam>
    /// <returns>The id of the item</returns>
    public string GetId<T>() where T : AbstractItem
    {
        foreach (var item in AllItems)
            if (item.Value is T)
                return item.Key;
        
        return null;
    }
    
    public class IgnoreAutoaddAttribute : Attribute
    {
        
    }

    /// <summary>
    /// create a truly random item, currently only used for the vortex effect
    /// </summary>
    /// <returns></returns>
    public AbstractItem RollTrulyRandomItem()
    {
        var items = AllItems.Values.ToList();
        var roll = (int)(GD.Randi() % items.Count);
        return items[roll];
    }
}