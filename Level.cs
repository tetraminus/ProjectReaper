using Godot;
using System;
using ProjectReaper.Util;
using LevelGenerator = ProjectReaper.Globals.LevelGenerator;

public partial class Level : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Level = this;
		GD.Randomize();
		LevelGenerator.Instance.GenerateLevel(RoomSetLoader.LoadRoomSet("TestRooms"), 10, 10, GD.Randi());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
