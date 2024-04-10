using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class LuckGlasses : AbstractItem
{
    
    public override string Id => "new_years_glasses";
    public override ItemRarity Rarity => ItemRarity.Legendary;
    
    public override void OnInitalPickup()
    {
        
        GameManager.Player.Stats.Luck += 1;
        
    }
    
    public override void OnStack(int newstacks)
    {
        GameManager.Player.Stats.Luck += newstacks;
    }

    public override void Cleanup()
    {
        GameManager.Player.Stats.Luck -= Stacks;
    }
}