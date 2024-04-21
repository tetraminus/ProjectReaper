using System;
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
	
	private PackedScene _leechScn = GD.Load<PackedScene>("res://Enemies/Bosses/Leech/leechbert.tscn");
	
	private PackedScene _goopScn = GD.Load<PackedScene>("res://Objects/Goop/goop.tscn");
	
	private float _totalSpawnArea;
	
	[Export]
	public double CollapseTimeMax { get; set; } = 1;//60 * 4;
	public double CollapseTime;
	public bool Collapsing { get; set; }
	private const float _minSpawnDistance = 500;
	private Godot.Collections.Dictionary<SpawnRect, float> _spawnRectWeights = new Godot.Collections.Dictionary<SpawnRect, float>();
	[Export] public bool DisableSpawning { get; set; }
	[Export] public bool DropKeys = true;
	[Export] public float KeysPerMiniboss = 5;
	
	[Export] public Array<string> FightStems = new Array<string>();
	
	[Export(PropertyHint.Range, "0,1,or_greater")] public int NumberOfChests { get; set; }
	[Export] public Node SpawnRects { get; set; }
	[Export] public Node LootPoints { get; set; }
	
	
	
	[Export] public Node2D DreamCollapseEndLocation { get; set; }
	[Export] public Vector2 LevelSize { get; set; }
	


	private Timer _renavTimer;
   

	public override void _Ready()
	{
		GameManager.Level = this;

		var spawnset = new Spawnset();

		spawnset.AddEnemy(new EnemySpawnCard(GooberScn, "Goober", 10));
		spawnset.AddEnemy(new EnemySpawnCard(_slimeScn, "Slimebert", 50));
		spawnset.AddEnemy(new EnemySpawnCard(BurrowerScn, "Snowpeabert", 175));

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
		
		CollapseTime = CollapseTimeMax;
		
	}

	public override void _Process(double delta)
	{
		CollapseTime -= delta;
		
		if (CollapseTime <= 0 && !Collapsing)
		{
			Collapsing = true;
			TriggerCollapse();
		}
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
		if (creature.IsInGroup("miniboss"))
		{
			
			CollapseTime -= 45;
			
			if (DropKeys)
			{
				for (int i = 0; i < KeysPerMiniboss; i++)
				{
					var key = (Key)KeyScn.Instantiate<Key>();
					
					var dropPosition = creature.GlobalPosition;
					
					Vector2 direction;
					do
					{
						direction = Vector2.FromAngle((float)(GD.Randf() * Math.Tau));
						direction *= 25f;
					} while (!key.checkDropPosition(dropPosition, direction));
					
					key.GlobalPosition = dropPosition + direction;
					CallDeferred(Node.MethodName.AddChild, key);
				}
			}
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
		SpawnDirector.Instance.PlaceMiniBosses(4, this);
		
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.LevelLoaded);
		
		
		
		
	}

	public AbstractCreature GetRandomMiniBoss()
	{
		return _leechScn.Instantiate<AbstractCreature>();
	}
	
	

	public Vector2 GetMinibossSpawnPosition()
	{
		var random = GD.Randf();
		foreach (var spawnRect in _spawnRectWeights)
		{
			random -= spawnRect.Value;
			if (random <= 0)
			{
				return spawnRect.Key.GetRandomPosition();
			}
		}
		return Vector2.Zero;
		
	}
	public bool PointSeesPlayer(Vector2 point)
	{
		var parameters = new PhysicsRayQueryParameters2D();
		parameters.From = point;
		parameters.To = GameManager.Player.GlobalPosition;
		// bit 1 is terrain
		parameters.CollisionMask = 1;
        
		var ray = GameManager.Level.GetWorld2D().DirectSpaceState.IntersectRay(parameters);

		return ray.Count == 0;
        
	}

	public List<SpawnRect> GetSpawnRects()
	{
		
		return _spawnRectWeights.Keys.ToList();
		
	}

	public void TriggerCollapse()
	{
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.CollapseStart);
		
		var goop = _goopScn.Instantiate<Goop>();
		CallDeferred(Node.MethodName.AddChild, goop);
		goop.SetTime(60);
		goop.GlobalPosition = DreamCollapseEndLocation.GlobalPosition;
		
		
		
		var farthestcorner = new Vector2(LevelSize.X/2f * -Math.Sign(DreamCollapseEndLocation.GlobalPosition.X), LevelSize.Y/2f * -Math.Sign(DreamCollapseEndLocation.GlobalPosition.Y));
		var radius = DreamCollapseEndLocation.GlobalPosition.DistanceTo(farthestcorner*2);
		
			
		GD.Print("Radius " + radius);
		goop.SetStartRadius(radius);
		goop.SetStopRadius(400);
		
		AudioManager.Instance.PlayMusic("Music", "DreamCollapseStart", 1);
		AudioManager.Instance.MusicManagerLooped += OnMusicManagerLooped;
		
	}
	
	private void OnMusicManagerLooped(string bank, string sound)
	{
		AudioManager.Instance.MusicManagerLooped -= OnMusicManagerLooped;
		if (sound == "DreamCollapseStart")
		{
			AudioManager.Instance.PlayMusic("Music", "DreamCollapse", 1.2f);
		}
		
	}
	
}
