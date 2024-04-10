using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class VitaminB : AbstractItem
{
    public const float AttackSpeed = 0.1172f; // it NEEDs to be this, i promise
    public override string Id => "vitamin_b";
    public override ItemRarity Rarity => ItemRarity.Common;

    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
    }

    private float CalculateStat(float stat, string statname)
    {
        if (statname == "AttackSpeed")
        {
            return stat + (stat * AttackSpeed * Stacks);
        }
        return stat;
    }
    
    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
}