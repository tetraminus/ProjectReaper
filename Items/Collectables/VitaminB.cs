using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class VitaminB : AbstractItem
{
    public override string Id => "vitamin_b";
    public override ItemRarity Rarity => ItemRarity.Common;
    
    public const float AttackSpeed = 0.0761f; // it NEED to be this, i promise

    public override void OnInitalPickup()
    {
        GameManager.Player.Stats.AttackSpeed += AttackSpeed;
    }
    
    public override void OnStack(int newstacks)
    {
        GameManager.Player.Stats.AttackSpeed += AttackSpeed * newstacks;
    }

    public override void Cleanup()
    {
        
        
    }
}