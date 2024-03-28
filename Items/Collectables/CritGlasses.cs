namespace ProjectReaper.Items.Collectables;

public partial class CritGlasses : AbstractItem
{
    
    public override string Id => "critglasses";
    public override ItemRarity Rarity => ItemRarity.Common;
    public const float CritChance = 0.15f;
    
    public override void OnInitalPickup()
    {
        GetHolder().Stats.CritChance += CritChance;
        
    }

    public override void OnStack(int newstacks)
    {
        GetHolder().Stats.CritChance += CritChance * newstacks;
        base.OnStack(newstacks);
    }
}