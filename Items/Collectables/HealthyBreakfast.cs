using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class HealthyBreakfast : AbstractItem
{
    public override string Id => "healthy_breakfast";
    public override ItemRarity Rarity => ItemRarity.Common;
    
    private const float HP = 0.20f;
    private const float Damage = 0.05f;
    

    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.RecalculateStats);
        
    }
    
    public float CalculateStat(float stat, string statname, AbstractCreature creature)
    {
        if (statname == "MaxHealth" && creature == GetHolder())
        {
            return stat + stat * HP * Stacks;
        }
        

        return stat;
    }

    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
    
}