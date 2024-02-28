using Godot;
using System;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Objects;

public partial class ForcefieldOrbital : Node2D, IProjectileBlocker
{
	
	[Export] public float Distance { get; set; } = 20;
	[Export] public float Speed { get; set; } = 1;
	public AbstractCreature.Teams Team { get; } = AbstractCreature.Teams.Player;


	private Area2D _shield;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shield = GetNode<Area2D>("shield");
		_shield.Position = new Vector2(Distance, 0);
		
		_shield.AreaEntered += OnAreaEntered;
		
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is not HurtBox hurtBox) return;
		if (hurtBox.GetParentBlocker() is not AbstractCreature creature) return;
		creature.Knockback(GlobalPosition.DirectionTo(creature.GlobalPosition) * 1000);
	}

	public void SetDistance(float distance)
	{
		Distance = distance;
		_shield.Position = new Vector2(Distance, 0);
	}
	
	public void SetSpeed(float speed)
	{
		Speed = speed;
	}
	
	public void SetRotation(float rotation)
	{
		Rotation = rotation;
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Rotate((float)delta * Speed);
		
	}

	public bool CanBlockProjectile(AbstractDamageArea projectile)
	{
		return _shield.GetOverlappingBodies().Contains(projectile);
	}

	public void OnProjectileBlocked(AbstractDamageArea projectile)
	{
		
	}
}
