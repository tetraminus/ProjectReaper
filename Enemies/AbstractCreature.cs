using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Util;

namespace ProjectReaper.Enemies;

public abstract partial class AbstractCreature : CharacterBody2D {

    public Stats Stats { get; set; } = new Stats();
    
    public Area2D Hurtbox { get; set; }

    public override void _Ready() {
        Stats.Init();
        Hurtbox = FindChild("Hurtbox") as Area2D;
        
    }

    public virtual void OnHit()
    {
	    
    }
	
	public virtual void OnDeath() {
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.CreatureDied, this);
		QueueFree();
	}


	public void Damage(DamageReport damageReport) {
		var damage = damageReport.Damage;
		var source = damageReport.Source;
		var target = damageReport.Target;
		var sourceStats = damageReport.SourceStats;
		var targetStats = damageReport.TargetStats;

		var finalDamage = Stats.CalculateDamage(damage, source, target, sourceStats, targetStats);
		
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.CreatureDamaged, this, finalDamage);
		
		Stats.Health -= finalDamage;
		if (Stats.Health <= 0) {
			OnDeath();
		}
		else {
			OnHit();
		}
		
	}
}
