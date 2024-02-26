using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper;

public partial class Level : Node2D
{
    private PackedScene BurrowerScn = GD.Load<PackedScene>("res://Enemies/Snowpeabert.tscn");
    private PackedScene GooberScn = GD.Load<PackedScene>("res://Enemies/Goober.tscn");
    private PackedScene SlimeScn = GD.Load<PackedScene>("res://Enemies/Slimebert.tscn");
    [Export] public bool DisableSpawning { get; set; }

    [Export] public int BoundsLeft { get; set; }
    [Export] public int BoundsRight { get; set; }
    [Export] public int BoundsTop { get; set; }
    [Export] public int BoundsBottom { get; set; }
    [Export(PropertyHint.Range, "0,1,or_greater")] public int NumberOfChests { get; set; }

    public override void _Ready()
    {
        GameManager.Level = this;

        var spawnset = new Spawnset();

        spawnset.AddEnemy(new EnemySpawnCard(GooberScn, "Goober", 5));
        // spawnset.AddEnemy(new EnemySpawnCard(SlimeScn, "Slimebert", 100));
        // spawnset.AddEnemy(new EnemySpawnCard(BurrowerScn, "Snowpeabert", 200));
        
        GameManager.Player.Camera.LimitLeft = BoundsLeft;
        GameManager.Player.Camera.LimitRight = BoundsRight;
        GameManager.Player.Camera.LimitTop = BoundsTop;
        GameManager.Player.Camera.LimitBottom = BoundsBottom;
        
       
        
        LootDirector.Instance.PlaceInteractables(NumberOfChests, this);
        

        SpawnDirector.Instance.Init(spawnset);
        if (!DisableSpawning) SpawnDirector.Instance.StartSpawning();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.


    public override void _Process(double delta)
    {
    }
}