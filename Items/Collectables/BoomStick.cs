using Godot;
using System;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

public partial class BoomStick : AbstractItem
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void init() {
		Callbacks.Instance.CreatureDied += onCreatureDied;
	}
	
	
	
	public void onCreatureDied(AbstractCreature creature) {
		
		GameManager.SpawnExplosion(creature.GlobalPosition, 10 * Stacks, 1f);
		
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
