using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper;

public partial class Level : Node2D
{
    private PackedScene BurrowerScn = GD.Load<PackedScene>("res://Enemies/Snowpeabert.tscn");
    private PackedScene GooberScn = GD.Load<PackedScene>("res://Enemies/Goober.tscn");
    private PackedScene SlimeScn = GD.Load<PackedScene>("res://Enemies/Slimebert.tscn");
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

    public override void _Ready()
    {
        GameManager.Level = this;

        var spawnset = new Spawnset();

        spawnset.AddEnemy(new EnemySpawnCard(GooberScn, "Goober", 10));
        //spawnset.AddEnemy(new EnemySpawnCard(SlimeScn, "Slimebert", 100));
        // spawnset.AddEnemy(new EnemySpawnCard(BurrowerScn, "Snowpeabert", 200));
        
        GameManager.Player.Camera.LimitLeft = BoundsLeft;
        GameManager.Player.Camera.LimitRight = BoundsRight;
        GameManager.Player.Camera.LimitTop = BoundsTop;
        GameManager.Player.Camera.LimitBottom = BoundsBottom;
       
        
        LootDirector.Instance.PlaceInteractables(NumberOfChests, this);
        

        SpawnDirector.Instance.Init(spawnset);
        if (!DisableSpawning) SpawnDirector.Instance.StartSpawning();

        if (SpawnRects == null) return;

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
    public Vector2 GetSpawnPosition()
    {
        
        var random = GD.Randf();
        foreach (var spawnRect in _spawnRectWeights)
        {
            if (spawnRect.Key.PlayerTooClose(_minSpawnDistance))
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
}