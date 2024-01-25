using Godot;
using System;
using ProjectReaper.Enemies;
using ProjectReaper.Util;
using LevelGenerator = ProjectReaper.Globals.LevelGenerator;

public partial class Level : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public Timer _timer = new Timer();
	public PackedScene _enemyScene = GD.Load<PackedScene>("res://Enemies/Goober.tscn");
	public override void _Ready()
	{
		GameManager.Level = this;
		
		_timer.Timeout += () => {
			var enemy = (AbstractCreature)_enemyScene.Instantiate();
			//move enemy to random position 100 pixels away from player
			
			var playerPos = GameManager.Player.GlobalPosition;
			Vector2 randomdir = new Vector2();
			randomdir.X = (float)GD.Randf() * 2 - 1;
			randomdir.Y = (float)GD.Randf() * 2 - 1;
			randomdir = randomdir.Normalized();
			enemy.GlobalPosition = playerPos + randomdir * 1000;
			enemy.Position = enemy.GlobalPosition;
			GetTree().Root.AddChild(enemy);
			_timer.Start();
		};
		_timer.WaitTime = 1;
		AddChild(_timer);
		_timer.Start();
		
		
		
		//LevelGenerator.Instance.GenerateLevel(RoomSetLoader.LoadRoomSet("TestRooms"), 10, 10, 1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
