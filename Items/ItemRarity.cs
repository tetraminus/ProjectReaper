using Godot;

namespace ProjectReaper.Items;

public partial class ItemRarity : GodotObject
{
    public int Value;
    public float Weight;
    public string NameKey;
    public Color Color;
    public bool AvailableInChests;
    public bool Hidden;
    public const float BaseCommonWeight = 58.9f;
    public const float BaseUncommonWeight = 30f;
    public const float BaseRareWeight = 10f;
    public const float BaseLegendaryWeight = 1f;
    public const float BaseShitWeight = 0.01f;
        
    public ItemRarity(int value, float weight, string nameKey, Color color, bool availableInChests = true, bool hidden = false)
    {
        Value = value;
        Weight = weight;
        NameKey = nameKey;
        Color = color;
        AvailableInChests = availableInChests;
        Hidden = hidden;
    }

    public static readonly ItemRarity Common = new ItemRarity(100, BaseCommonWeight, "r_common", new Color(1, 1, 1));
    public static readonly ItemRarity Uncommon = new ItemRarity(200, BaseUncommonWeight, "r_uncommon", new Color(0, 1, 0));
    public static readonly ItemRarity Rare = new ItemRarity(300, BaseRareWeight, "r_rare", new Color(1, 0, 0.7f));
    public static  ItemRarity Legendary = new ItemRarity(400, BaseLegendaryWeight, "r_legendary", new Color(1,0,0));
    public static ItemRarity Shit = new ItemRarity(0, BaseShitWeight, "r_garbage", Colors.BlanchedAlmond, true, true);
}