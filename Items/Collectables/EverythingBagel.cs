using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Items.Collectables;

public partial class EverythingBagel : AbstractItem
{
    public override string Id => "everything_bagel";
    public override ItemRarity Rarity => ItemRarity.Common;
    
    
    
    private const float Damage = 0.03f;
    private const float CritChance = 0.03f;
    private const float CritDamage = 0.03f;
    private const float AttackSpeed = 0.03f;
    private const float CooldownReduction = 0.03f;
    


    public override void OnInitalPickup()
    {
        Callbacks.Instance.CalculateStat += CalculateStat;
        
    }
    
    public void CalculateStat(ref float stat, string statname, AbstractCreature creature)
    {
        if (creature != GetHolder())
        {
            return; 
        }
        
        switch (statname)
        {
            case "Damage":
                stat += stat * Damage * Stacks;
                break;
            case "CritDamage":
                stat += CritDamage * Stacks;
                break;
            case "CritChance":
                stat += CritChance * Stacks;
                break;
            case "AttackSpeed":
                stat += stat * AttackSpeed * Stacks;
                break;
        }

        if (statname == "AbilityCooldown" + AbilityManager.AbilitySlot.Utility && creature == GetHolder())
        {
            stat *= Mathf.Pow(1 - CooldownReduction, Stacks);
        }
        
        
        
    }

    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
    
}