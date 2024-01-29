using Godot;
using System;
using ProjectReaper.Enemies;
using ProjectReaper.Util;

public partial class GameManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public static ProjectReaper.Player.Player Player { get; set; }
	public static PackedScene ExplosionScene = ResourceLoader.Load<PackedScene>("res://Abilities/Projectiles/Explosion.tscn");

	public static Node2D Level { get; set; }
	
	public static bool RandomBool(int luck)
	{
		for (int i = 0; i < luck + 1; i++)
		{
			if (GD.RandRange(0, 1) == 1 )
			{
				return true;
			}
			
		}
		return false;
	}
	
	public static float Randf(float min, float max, int luck)
	{
		float result = 0;
		for (int i = 0; i < luck + 1; i++) {
			var rand = GD.Randf();
			rand = rand * (max - min) + min;
			
			if (rand > result)
			{
				result = rand;
			}
		}
		return result;
	}
	
	
	
	
	
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public static void SpawnExplosion(Vector2 globalPosition, int damage, float scale = 1f, AbstractCreature creature = null) {
		
		var explosion = (Explosion) ExplosionScene.Instantiate();
		explosion.GlobalPosition = globalPosition;
		explosion.Scale = new Vector2(scale, scale);
		explosion.setDamage(damage);
		if (creature == null)
		{
			explosion.Source = Player;
		}
		Level.CallDeferred("add_child", explosion);
		
		
	}
}
