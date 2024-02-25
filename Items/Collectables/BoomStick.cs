using System;
using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class BoomStick : AbstractItem
{
    public override string Id => "boom_stick";
    public override Texture2D Icon => GD.Load<Texture2D>("res://Items/Collectables/Icons/boom_stick.png");

    public override void OnInitalPickup()
    {
        Callbacks.Instance.CreatureDiedEvent += OnCreatureDied;
    }

    public override void Init()
    {
        
    }


    public void OnCreatureDied(AbstractCreature creature)
    {
        GameManager.SpawnExplosion(creature.GlobalPosition, 10 * Stacks, 1.5f);
    }
}

