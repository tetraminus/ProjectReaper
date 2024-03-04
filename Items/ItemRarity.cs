using Godot;

namespace ProjectReaper.Items;

public partial class ItemRarity 
{
    public int Value;
    public float Weight;
    public string Name;
    public Color Color;
    public bool AvailableInChests;
    public const float BaseCommonWeight = .6f;
    public const float BaseUncommonWeight = .3f;
    public const float BaseRareWeight = .1f;
    public const float BaseLegendaryWeight = 11.01f;
        
    public ItemRarity(int value, float weight, string name, Color color, bool availableInChests = true)
    {
        Value = value;
        Weight = weight;
        Name = name;
        Color = color;
        AvailableInChests = availableInChests;
    }

    public static readonly ItemRarity Common = new ItemRarity(100, BaseCommonWeight, "r_common", new Color(1, 1, 1));
    public static readonly ItemRarity Uncommon = new ItemRarity(200, BaseUncommonWeight, "r_uncommon", new Color(0, 1, 0));
    public static readonly ItemRarity Rare = new ItemRarity(300, BaseRareWeight, "r_rare", new Color(1, 0, 0.7f));
    public static  ItemRarity Legendary = new ItemRarity(400, BaseLegendaryWeight, "r_legendary", new Color(1,0,0));
}