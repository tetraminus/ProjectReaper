using Godot;
using ProjectReaper.Enemies;

namespace ProjectReaper;

public partial class Level : Node2D
{
	// Called when the node enters the scene tree for the first time.
	Timer EnemySpawnTimer = new Timer();
	PackedScene EnemyScene = ResourceLoader.Load<PackedScene>("res://enemies/Goober.tscn");
	public override void _Ready()
	{
		GameManager.Level = this;
		
		AddChild(EnemySpawnTimer);
		EnemySpawnTimer.WaitTime = 1;
		EnemySpawnTimer.Timeout += _OnEnemySpawnTimerTimeout;
		EnemySpawnTimer.Start();
		
		
		
		
		//LevelGenerator.Instance.GenerateLevel(RoomSetLoader.LoadRoomSet("TestRooms"), 10, 10, 1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	public void _OnEnemySpawnTimerTimeout()
	{
		var enemy = (AbstractCreature)EnemyScene.Instantiate();
		AddChild(enemy);
		
		Vector2 randomDirection = new Vector2();
		randomDirection.X = (2.0f - (float)GD.RandRange(0.0f, 1.0f));
		randomDirection.Y = (2.0f - (float)GD.RandRange(0.0f, 1.0f));
		randomDirection = randomDirection.Normalized();
		randomDirection *= 1000;
		enemy.GlobalPosition = randomDirection + GameManager.Player.GlobalPosition;
		EnemySpawnTimer.Start();
		
		
	}
	
	public override void _Process(double delta)
	{
	}
}