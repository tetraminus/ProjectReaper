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
    
    public float CalculateStat(float stat, string statname, AbstractCreature creature)
    {
        if (creature != GetHolder())
        {
            return stat; 
        }
        
        switch (statname)
        {
            case "Damage":
                return stat + stat * Damage * Stacks;
            case "CritDamage":
                return stat + CritDamage * Stacks;
            case "CritChance":
                return stat + CritChance * Stacks;
            case "AttackSpeed":
                return stat + stat * AttackSpeed * Stacks;
        }

        if (statname == "AbilityCooldown" + AbilityManager.AbilitySlot.Utility && creature == GetHolder())
        {
            return stat * Mathf.Pow(1 - CooldownReduction, Stacks);
        }
        
        return stat;
        
    }

    public override void Cleanup()
    {
        Callbacks.Instance.CalculateStat -= CalculateStat;
    }
    
}