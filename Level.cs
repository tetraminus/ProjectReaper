using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper;

public partial class Level : Node2D
{
    private PackedScene BurrowerScn = GD.Load<PackedScene>("res://Enemies/Snowpeabert.tscn");
    private PackedScene GooberScn = GD.Load<PackedScene>("res://Enemies/Goober.tscn");
    private PackedScene _slimeScn = GD.Load<PackedScene>("res://Enemies/Slime/Slimebert.tscn");
    private float _totalSpawnArea;
    private const float _minSpawnDistance = 500;
    private Dictionary<SpawnRect, float> _spawnRectWeights = new Dictionary<SpawnRect, float>();
    [Export] public bool DisableSpawning { get; set; }

    [Export] public int BoundsLeft { get; set; }
    [Export] public int BoundsRight { get; set; }
    [Export] public int BoundsTop { get; set; }
    [Export] public int BoundsBottom { get; set; }
    [Export(PropertyHint.Range, "0,1,or_greater")] public int NumberOfChests { get; set; }
    [Export] public Node SpawnRects { get; set; }
    [Export] public Node LootPoints { get; set; }
    [Export] public Node2D PhantomCamera { get; set; }
   

    public override void _Ready()
    {
        GameManager.Level = this;

        var spawnset = new Spawnset();

        spawnset.AddEnemy(new EnemySpawnCard(GooberScn, "Goober", 10));
        spawnset.AddEnemy(new EnemySpawnCard(_slimeScn, "Slimebert", 50));
        // spawnset.AddEnemy(new EnemySpawnCard(BurrowerScn, "Snowpeabert", 200));

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

    public void AddPlayer(Player.Player player)
    {
        AddChild(player);
        player.GlobalPosition = GetSpawnPosition(true);
        
        var pcamProperties = PhantomCamera.Get("Properties").AsGodotObject();
        
        PhantomCamera.Set("follow_target", player.GetPath());
        
        
        pcamProperties.Set("has_tweened", true);
        
        if (PhantomCamera.Get("tween_parameters").Obj != null)
        {
            PhantomCamera.Get("tween_parameters").AsGodotObject().Set("duration", 0);
        }
        
        
        
        PhantomCamera.Set("follow_parameters/damping", false);
        PhantomCamera.GlobalPosition = player.GlobalPosition;
        FixCameraTween();
       
        
        player.Show();
        
    }
    
    public async void FixCameraTween()
    {
        // Going to hell for this
       await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
       await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
       if (PhantomCamera.Get("tween_parameters").Obj != null)
       {
           PhantomCamera.Get("tween_parameters").AsGodotObject().Set("duration", 1);
       }
       
       PhantomCamera.Set("follow_parameters/damping", true);
    }

    public void Generate()
    {
        LootDirector.Instance.PlaceInteractables(NumberOfChests, this);
    }
}