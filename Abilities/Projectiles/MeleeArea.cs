using System;
using Godot;
using ProjectReaper.Enemies;

namespace ProjectReaper.Abilities.Projectiles;

public partial class MeleeArea : AbstractDamageArea
{
	public override float Speed { get; set; } = 0;
	public override float Damage { get; set; } = 10;
	public override float Duration { get; set; } = -1;
	
	public KnockbackType KBType { get; set; } = KnockbackType.Normal;
	public enum KnockbackType
	{
		Normal,
		Charge
	}
	
	
	

	public override void _Ready()
	{
	
		base._Ready();
		Knockback = 700;	
		DestroyOnHit = false;
		DestroyOnWall = false;
	}

	protected override Vector2 GetKnockbackDirection(AbstractCreature creature)
	{
		if (KBType == KnockbackType.Normal)
		{
			return base.GetKnockbackDirection(creature);
		}
		else
		{
			var dir = Vector2.FromAngle(GlobalRotation);
			
			dir = dir.Rotated(Mathf.Pi / 2);
			
			if (GD.Randf() > 0.5) dir = -dir;
			
			return dir;
		}
	}
}