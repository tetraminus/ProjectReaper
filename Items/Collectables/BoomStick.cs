using Godot;
using System;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

public partial class BoomStick : AbstractItem
{
	public override void init() {
		Callbacks.Instance.CreatureDiedEvent += onCreatureDied;
	}
	
	
	
	public void onCreatureDied(AbstractCreature creature) {
		
		GameManager.SpawnExplosion(creature.GlobalPosition, 10 * Stacks, 1.5f);
		
	}

}
