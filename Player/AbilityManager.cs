using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Player;

public partial class AbilityManager : Node
{
    [Export(PropertyHint.NodeType)] public AbstractAbility[] Abilities { get; set; } = new AbstractAbility[4];
    [Export] public Node2D Creature { get; set; }

    public override void _Ready()
    {
        foreach (var ability in Abilities)
        {
            ability?.SetCreature(Creature as AbstractCreature);
        }
    }

    public void UseAbility(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= Abilities.Length) return;

        var ability = Abilities[abilityIndex];
        if (!ability.CheckCooldown())
        {
            Callbacks.Instance.EmitSignal(Callbacks.SignalName.AbilityUsed, ability, abilityIndex + 1);
            ability.Use();
            ability.StartCooldown();
        }
    }
    
    

    
    
    public AbstractAbility GetAbility(AbilitySlot slot)
    {
        return Abilities[(int) slot];
    }
    
    
    
    public enum AbilitySlot
    {
        Main = 0,
        Secondary = 1,
        Utility = 2,
        Special = 3
    }
   
}