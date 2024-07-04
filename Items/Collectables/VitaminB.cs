using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class VitaminB : AbstractItem
{
	public override string Id => "vitamin_b";
	public override ItemRarity Rarity => ItemRarity.Common;
	
	public const float AttackSpeed = 0.0872f; // it NEEDs to be this, i promise

    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
    }

    private void CalculateStat(ref float stat, string statname, AbstractCreature creature)
    {
        if (statname == "AttackSpeed" && creature == GetHolder())
        {
            stat += (stat * AttackSpeed * Stacks);
        }
      
    }
    
    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
}