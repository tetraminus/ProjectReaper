using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Player;

public partial class AbilityManager : Node
{
    [Export(PropertyHint.NodeType)] public AbstractAbility Ability1 { get; set; }
    [Export(PropertyHint.NodeType)] public AbstractAbility Ability2 { get; set; }
    [Export(PropertyHint.NodeType)] public AbstractAbility Ability3 { get; set; }
    [Export(PropertyHint.NodeType)] public AbstractAbility Ability4 { get; set; }
    [Export] public Node2D Creature { get; set; }

    public override void _Ready()
    {
        
        Ability1?.SetCreature(Creature as AbstractCreature);
        Ability2?.SetCreature(Creature as AbstractCreature);
        Ability3?.SetCreature(Creature as AbstractCreature);
        Ability4?.SetCreature(Creature as AbstractCreature);
        
    }

    public void UseAbility1()
    {
        if (!Ability1.CheckCooldown())
        {
            Callbacks.Instance.AbilityUsedEvent?.Invoke(Ability1, 1);
            Ability1.Use();
            Ability1.StartCooldown();
        }
    }

    public void UseAbility2()
    {
        if (!Ability2.CheckCooldown())
        {
            Callbacks.Instance.AbilityUsedEvent?.Invoke(Ability2, 2);
            Ability2.Use();
            Ability2.StartCooldown();
        }
    }

    public void UseAbility3()
    {
        if (!Ability3.CheckCooldown())
        {
            Callbacks.Instance.AbilityUsedEvent?.Invoke(Ability3, 3);
            Ability3.Use();
            Ability3.StartCooldown();
        }
    }

    public void UseAbility4()
    {
        if (!Ability4.CheckCooldown())
        {
            Callbacks.Instance.AbilityUsedEvent?.Invoke(Ability4, 4);
            Ability4.Use();
            Ability4.StartCooldown();
        }
    }
}