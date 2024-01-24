using Godot;
using System;
using ProjectReaper.Util;

public partial class GameManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public static ProjectReaper.Player.Player Player { get; set; }
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
}
