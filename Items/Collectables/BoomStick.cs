using System;
using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class BoomStick : AbstractItem
{
    public override string Id => "boom_stick";
    

    public override void OnInitalPickup()
    {
        Callbacks.Instance.CreatureDiedEvent += OnCreatureDied;
    }


    public void OnCreatureDied(AbstractCreature creature)
    {
        GameManager.SpawnExplosion(creature.GlobalPosition, 10 * Stacks, 1f + (0.2f * Stacks));
    }
}

