using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Globals;

namespace ProjectReaper.Player;

public partial class AbilityManager : Node
{
    
    [Export(PropertyHint.NodeType)] public AbstractAbility Ability1 { get; set; }
    [Export(PropertyHint.NodeType)] public AbstractAbility Ability2 { get; set; }
    [Export(PropertyHint.NodeType)] public AbstractAbility Ability3 { get; set; }
    [Export(PropertyHint.NodeType)] public AbstractAbility Ability4 { get; set; }
    
    public void UseAbility1()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.AbilityUsed, Ability1, 1);
        Ability1.Use();
        
    }
    
    public void UseAbility2()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.AbilityUsed, Ability2, 2);
        Ability2.Use();
        
    }
    
    public void UseAbility3()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.AbilityUsed, Ability3, 3);
        Ability3.Use();
        
    }
    
    public void UseAbility4()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.AbilityUsed, Ability4, 4);
        Ability4.Use();
        
    }
    
}