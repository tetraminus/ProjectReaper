using Godot;
using System;
using ProjectReaper.Abilities.Projectiles;

public partial class SwirlfireBullet : AbstractDamageArea
{
	public override float Speed { get; set; } = 1000f;
	public override float Damage { get; set; } = 10f;
	public override float Duration { get; set; } = 1f;

	private float rotationSpeed = 6f;
	
	private Transform2D heading;


	public override void PostInit()
	{
		heading = Transform;
	}

	public override void _Process(double delta){
		
		// Rotate the bullet
		Rotation += rotationSpeed * (float)delta;
		
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		Translate(heading.X * Speed * (float)delta);
	}
}
