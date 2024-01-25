using Godot;	
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

namespace ProjectReaper.Abilities.Projectiles;

public abstract partial class AbstractDamageArea : Area2D
{
	
	private Timer _timer;
	public bool IsEnemy = false;
	
	public AbstractCreature Source { get; set; }
	
	public override void _Ready()
	{
		Monitoring = true;
		
		AreaEntered += (area) => {
			GD.Print("Area entered");
			if (area is not HurtBox hurtBox) return;
			var enemy = hurtBox.GetParentCreature();
			enemy.Damage(new DamageReport(Damage, Source, enemy, Source.Stats, enemy.Stats));
			QueueFree();
			

		};
		
	}
	public void OnShoot() {}


	public void OnHit() {
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.BulletHit, this);
	}

	public abstract float Speed { get; set; }
	
	public abstract float Damage { get; set; }
	
	public abstract float Duration { get; set; }
	
}
