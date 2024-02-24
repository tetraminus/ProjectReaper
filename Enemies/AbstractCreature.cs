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
	    foreach (var item in Items.GetChildren()) {
		    if (item is T) {
			    stacks += (item as AbstractItem).Stacks;
		    }
		    
	    }
	    return stacks;
	}
    
    public int GetItemStacks(string id) {
	    var stacks = 0;
	    foreach (var item in Items.GetChildren()) {
		    if (item is AbstractItem abstractItem && abstractItem.ID == id) {
			    stacks += abstractItem.Stacks;
		    }
	    }
	    return stacks;
    }

    public bool HasItem<T>() where T : AbstractItem {
	    foreach (var item in Items.GetChildren()) {
		    if (item is T) {
			    return true;
		    }
	    }

	    return false;
    }
    
    public bool HasItem(string id) {
	    foreach (var item in Items.GetChildren()) {
		    if (item is AbstractItem abstractItem && abstractItem.ID == id) {
			    return true;
		    }
	    }
	    return false;
	}

    private AbstractItem GetItem(string id) {
	    foreach (var item in Items.GetChildren()) {
		    if (item is AbstractItem abstractItem && abstractItem.ID == id) {
			    return abstractItem;
		    }
	    }

	    return null;
    }


    public void AddItem(string id, int stacks = 1) {
	    var item = GetItem(id);
	    if (item != null) {
		    item.Stacks += stacks;
	    }
	    else {
		    var newItem = ItemLibrary.Instance.CreateItem(id);
		    newItem.Stacks = stacks;
		    Items.AddChild(newItem);
	    }
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
