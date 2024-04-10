using System.Collections.Generic;
using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Interactables;

namespace ProjectReaper.Items.Collectables;

public partial class YrayGoggles : AbstractItem
{
    public override string Id => "xray_goggles";
    public override ItemRarity Rarity => ItemRarity.Rare;
    private PackedScene _rarityIndicator = GD.Load<PackedScene>("res://Items/Prefabs/RarityIndicator.tscn");
    private Dictionary<Chest, RarityIndicator> _indicators = new Dictionary<Chest, RarityIndicator>();
    public override void OnInitalPickup()
    {
        Callbacks.Instance.LevelLoaded += OnLevelLoaded;
        OnLevelLoaded();
    }
    
    private void OnLevelLoaded()
    {
        _indicators.Clear();
        foreach (var chest in GameManager.Level.GetInteractables())
        {
            if (chest is Chest c)
            {
                var rarityIndicator = _rarityIndicator.Instantiate<RarityIndicator>();
                c.AddChild(rarityIndicator);
                _indicators.Add(c, rarityIndicator);
            }
        }
        SetIndicators();
       
    }

    public override void OnStack(int newstacks)
    {
        SetIndicators();
    }

    public void SetIndicators()
    {
        foreach (var indicator in _indicators)
        {
            var roll = GameManager.RollBool(0.1f * Stacks);
            if (roll)
            {
                GD.Print("Real rarity");
                indicator.Value.SetRarity(indicator.Key.GetRarity());
            }
            else
            {
                GD.Print("Fake rarity");
                var rarity = ItemLibrary.Instance.RollRarity(true, GameManager.RTRng);
                indicator.Value.SetRarity(rarity);
            }
            
        }
    }
    

    public override void Cleanup()
    {
        Callbacks.Instance.LevelLoaded -= OnLevelLoaded;
        foreach (var indicator in _indicators)
        {
            indicator.Value.QueueFree();
        }
        _indicators.Clear();
        
        
    }
    
}