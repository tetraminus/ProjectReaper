using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper;

public partial class Level : Node2D
{
	private PackedScene GooberScn = GD.Load<PackedScene>("res://Enemies/Goober.tscn");
	private PackedScene SlimeScn = GD.Load<PackedScene>("res://Enemies/Slimebert.tscn");
	private PackedScene BurrowerScn = GD.Load<PackedScene>("res://Enemies/Snowpeabert.tscn");
	

	public override void _Ready()
	{
		GameManager.Level = this;
		
		var spawnset = new Spawnset();
		
		spawnset.AddEnemy(new EnemySpawnCard(GooberScn, "Goober", 5));
		spawnset.AddEnemy(new EnemySpawnCard(SlimeScn, "Slimebert", 100));
		spawnset.AddEnemy(new EnemySpawnCard(BurrowerScn, "Snowpeabert", 200));
		
		
		
		SpawnDirector.Instance.Init(spawnset);
		SpawnDirector.Instance.StartSpawning();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	
	public override void _Process(double delta)
	{
	}
}