using Godot;

namespace ProjectReaper.Enemies;

public partial class HurtBox : Area2D
{
    public AbstractCreature GetParentCreature()
    {
        return GetParent() as AbstractCreature;
    }

    public override void _Ready()
    {
        if (GetParentCreature() == null) GD.PrintErr("Hurtbox parent is not a creature");
    }
}