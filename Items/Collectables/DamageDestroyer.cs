using Godot;
using System;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

public partial class DamageDestroyer : AbstractItem
{
	
	
	public override string ID => "damage_destroyer";

	public override void init() {
        Callbacks.Instance.FinalDamageEvent += onFinalDamage;
    }
    
	
	
	
    public float onFinalDamage(AbstractCreature creature, float damage) {
		return damage * 2;
    }

}