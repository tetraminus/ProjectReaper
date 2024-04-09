using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using Key = ProjectReaper.Objects.Key.Key;

namespace ProjectReaper;
public partial class Level : Node2D
{
	private PackedScene KeyScn = GD.Load<PackedScene>("res://Objects/Key/Key.tscn");
	private PackedScene BurrowerScn = GD.Load<PackedScene>("res://Enemies/SnowPlant/Snowpeabert.tscn");
	private PackedScene GooberScn = GD.Load<PackedScene>("res://Enemies/Goober.tscn");
	private PackedScene _slimeScn = GD.Load<PackedScene>("res://Enemies/Slime/Slimebert.tscn");
	private float _totalSpawnArea;
	private const float _minSpawnDistance = 500;
	private Godot.Collections.Dictionary<SpawnRect, float> _spawnRectWeights = new Godot.Collections.Dictionary<SpawnRect, float>();
	[Export] public bool DisableSpawning { get; set; }
	[Export] public bool DropKeys = true;
	[Export] public float KeyDropChance = 0.1f;
	[Export] public Array<string> FightStems = new Array<string>();
	
	[Export(PropertyHint.Range, "0,1,or_greater")] public int NumberOfChests { get; set; }
	[Export] public Node SpawnRects { get; set; }
	[Export] public Node LootPoints { get; set; }
	
	[Export] public Node2D CameraBounds { get; set; }

	private Timer _renavTimer;
   

	public override void _Ready()
	{
		GameManager.Level = this;

		var spawnset = new Spawnset();

		spawnset.AddEnemy(new EnemySpawnCard(GooberScn, "Goober", 10));
		spawnset.AddEnemy(new EnemySpawnCard(_slimeScn, "Slimebert", 50));
		//spawnset.AddEnemy(new EnemySpawnCard(BurrowerScn, "Snowpeabert", 10));

		// PhantomCamera.Set("limit/left", BoundsLeft);
		// PhantomCamera.Set("limit/right", BoundsRight);
		// PhantomCamera.Set("limit/top", BoundsTop);
		// PhantomCamera.Set("limit/bottom", BoundsBottom);

		SpawnDirector.Instance.Init(spawnset);
		if (!DisableSpawning) SpawnDirector.Instance.StartSpawning();

		foreach (var spawnRect in SpawnRects.GetChildren()) {
			_totalSpawnArea += ((SpawnRect)spawnRect).GetArea();
		}
		
		foreach (var spawnRect in SpawnRects.GetChildren()) {
			_spawnRectWeights.Add((SpawnRect)spawnRect, ((SpawnRect)spawnRect).GetArea() / _totalSpawnArea);
		}
		
		Callbacks.Instance.CreatureDied += OnEnemyDeath;
		
		_renavTimer = new Timer();
		_renavTimer.WaitTime = 2;
		_renavTimer.Timeout += OnRenavTimerTimeout;
		AddChild(_renavTimer);
		_renavTimer.Start();
		
	}
	
	public void FadeInFightMusic(float time = 1)
	{
		if (FightStems.Count == 0)
		{
			return;
		}

		foreach (var stem in FightStems)
		{
			AudioManager.Instance.PlayStem(stem, time);
		}
		
	}
	
	public void FadeOutFightMusic(float time = 1)
	{
		if (FightStems.Count == 0)
		{
			return;
		}

		foreach (var stem in FightStems)
		{
			AudioManager.Instance.StopStem(stem, time);
		}
		
	}
	
	private async void OnRenavTimerTimeout()
	{

		for (int i = 0; i < SpawnDirector.MaxNavGroups; i++)
		{
		   // GD.Print("Renaving " + i);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			Callbacks.Instance.EmitSignal(Callbacks.SignalName.EnemyRenav,GameManager.Player.GlobalPosition, i);

		}
	}

	public override void _ExitTree()
	{
		Callbacks.Instance.CreatureDied -= OnEnemyDeath;
		base._ExitTree();
	}

	public void OnEnemyDeath(AbstractCreature creature)
	{
		if (GameManager.RollBool(KeyDropChance,1, GameManager.LootRng) && DropKeys)
		{
			var key = KeyScn.Instantiate<Key>();
			key.GlobalPosition = creature.GlobalPosition;
			CallDeferred(Node.MethodName.AddChild, key);
		}
		
	}
	

	/// <summary>
	/// Returns a random spawn position based on the spawnRects
	/// </summary>
	/// <returns></returns>
	public Vector2 GetSpawnPosition(bool isplayer = false)
	{
		
		var random = GD.Randf();
		foreach (var spawnRect in _spawnRectWeights)
		{
			if (spawnRect.Key.PlayerTooClose(_minSpawnDistance) && !isplayer)
			{
				continue;
			}
			

			random -= spawnRect.Value;
			if (random <= 0)
			{
				return spawnRect.Key.GetRandomPosition();
			}
		}
		return Vector2.Zero;
		
	}

	

	public void AddPlayer(Node playerroot)
	{
		var player = playerroot.GetNode<Player.Player>("%Player");
		AddChild(playerroot);
		player.GlobalPosition = GetSpawnPosition(true);
		if (CameraBounds != null)
		{
			playerroot.GetNode("%PlayerPCam").Call("set_limit_node", CameraBounds);
			
		}
		playerroot.GetNode<Node2D>("%PlayerPCam").GlobalPosition = player.GlobalPosition;


		player.Show();
		
	}
	
	public List<Node2D> GetInteractables()
	{
		var interactables = GetNode("Chests");
		if (interactables == null)
		{
			return new List<Node2D>();
		}
		
		return interactables.GetChildren().Cast<Node2D>().ToList();
	}
	
	

	public void Generate()
	{
		LootDirector.Instance.PlaceInteractables(NumberOfChests, this);
		
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.LevelLoaded);
	}
}
