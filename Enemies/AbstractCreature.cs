using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Util;

namespace ProjectReaper.Enemies;

public abstract partial class AbstractCreature : CharacterBody2D {

    public Stats Stats { get; set; } = new Stats();
    
    public Area2D Hurtbox { get; set; }
    public Node Items { get; set; }
    public bool dead = false;
    
    public HitBoxState HitState { get; set; } = HitBoxState.Normal;
    

    public override void _Ready() {
        Stats.Init();
        Hurtbox = FindChild("Hurtbox") as Area2D;
        Items = new Node() {
			Name = "Items"
		};
        AddChild(Items);

    }

    public virtual void OnHit()
    {
	    
    }
    
    public int GetItemStacks<T>() where T : AbstractItem {
	    var stacks = 0;
	    foreach (var item in GetChildren()) {
		    if (item is T) {
			    stacks += (item as T).GetStacks();
		    }
	    }

	    return stacks;
	}

    public bool HasItem<T>() where T : AbstractItem {
	    foreach (var item in GetChildren()) {
		    if (item is T) {
			    return true;
		    }
	    }

	    return false;
    }

    public void AddItem(AbstractItem item) { 
	    
	}
	
	public virtual void OnDeath() {
		Callbacks.Instance.CreatureDiedEvent?.Invoke(this);
		dead = true;
		QueueFree();
	}


	public void Damage(DamageReport damageReport) {
		if (dead || HitState == HitBoxState.Invincible) {
			return;
		}
		
		var damage = damageReport.Damage;
		var source = damageReport.Source;
		var target = damageReport.Target;
		var sourceStats = damageReport.SourceStats;
		var targetStats = damageReport.TargetStats;

		var finalDamage = Stats.CalculateDamage(damage, source, target, sourceStats, targetStats);
		
		finalDamage = Callbacks.Instance.FinalDamageEvent?.Invoke(this, finalDamage) ?? finalDamage;
		
		Stats.Health -= finalDamage;
		if (Stats.Health <= 0) {
			OnDeath();
		}
		else {
			OnHit();
		}
		
	}
	
	public enum HitBoxState {
		Normal,
		Invincible,
		Spectral
	}
}
