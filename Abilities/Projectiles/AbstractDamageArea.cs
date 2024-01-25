using Godot;	
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

namespace ProjectReaper.Abilities.Projectiles;

public abstract partial class AbstractDamageArea : Area2D
{
	
	private Timer _timer;
	public bool IsEnemy = false;
	public override void _Ready()
	{
		_timer = new Timer();
		AddChild(_timer);
		_timer.Timeout += () => QueueFree();
		
		BodyEntered += (area) =>
		{
			if (area is AbstractCreature enemy && !IsEnemy)
			{
				enemy.Damage(new DamageReport(Damage, GameManager.Player, enemy));
				QueueFree();
			}
			else if (area is Player.Player player && IsEnemy)
			{
				player.Damage(new DamageReport(Damage, GameManager.Player, player));
				QueueFree();
			}
			
			
		};
		
	}
	public void OnShoot() {}


	public void OnHit()
	{
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.BulletHit, this);
	}
	
	

	
	public abstract float Speed { get; set; }
	
	public abstract float Damage { get; set; }
	
	public abstract float Duration { get; set; }
	
}
