using Godot;
using System;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

public partial class BoomStick : AbstractItem
{
	public override string ID => "boom_stick";

	public override void init() {
		Callbacks.Instance.CreatureDiedEvent += onCreatureDied;
	}
	
	
	
	public void onCreatureDied(AbstractCreature creature) {
		
		ProjectReaper.Globals.GameManager.SpawnExplosion(creature.GlobalPosition, 10 * Stacks, 1.5f);
		
	}

}
