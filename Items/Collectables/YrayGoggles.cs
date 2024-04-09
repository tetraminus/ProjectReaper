using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Interactables;

namespace ProjectReaper.Items.Collectables;

public partial class YrayGoggles : AbstractItem
{
    public override string Id => "xray_goggles";
    public override ItemRarity Rarity => ItemRarity.Rare;
    private PackedScene _rarityIndicator = GD.Load<PackedScene>("res://Items/Prefabs/RarityIndicator.tscn");

    public override void OnInitalPickup()
    {
        Callbacks.Instance.LevelLoaded += OnLevelLoaded;
    }
    
    private void OnLevelLoaded()
    {
        foreach (var chest in GameManager.Level.GetInteractables())
        {
            if (chest is Chest c)
            {
                c.AddChild(_rarityIndicator.Instantiate<RarityIndicator>());
            }
        }
       
    }

    public override void Cleanup()
    {
        
        
    }
    
}