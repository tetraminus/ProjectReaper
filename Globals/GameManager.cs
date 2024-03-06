using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Player;
using ProjectReaper.Util;

namespace ProjectReaper.Globals;

public partial class GameManager : Node
{
    public static PackedScene ExplosionScene = ResourceLoader.Load<PackedScene>("res://Abilities/Projectiles/Explosion.tscn");
    public static PackedScene PlayerHudScene = ResourceLoader.Load<PackedScene>("res://Player/PlayerHud.tscn");
    

    // Called when the node enters the scene tree for the first time.
    public static Player.Player Player { get; set; }
    public static PlayerHud PlayerHud { get; set; }
    public static Level Level { get; set; }
    public static bool Paused { get; set; }
    public static bool InRun { get; set; }
    public static PauseMenu PauseMenu { get; set; }
    public static RunInfo CurrentRun { get; set; }
    
    
    public static RandomNumberGenerator LootRng = new RandomNumberGenerator();
    public static RandomNumberGenerator LevelRng = new RandomNumberGenerator();
    public static RandomNumberGenerator BossRng = new RandomNumberGenerator();
    

    public override void _Ready()
    {
        var playerhudScene = PlayerHudScene.Instantiate<CanvasLayer>();
        AddChild(playerhudScene);
        PlayerHud.Hide();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    

    

    public static void PauseGame()
    {
        Paused = true;
        Level.GetTree().Paused = true;
        PauseMenu.Show();
        
    }
    
    public static void UnpauseGame()
    {
        Paused = false;
        Level.GetTree().Paused = false;
        PauseMenu.Hide();
    }

    public static void SpawnExplosion(Vector2 globalPosition, int damage, float scale = 1f,
        AbstractCreature creature = null)
    {
        var explosion = (Explosion)ExplosionScene.Instantiate();
        explosion.GlobalPosition = globalPosition;
        explosion.Scale = new Vector2(scale, scale);
        explosion.setDamage(damage);
        explosion.Source = creature ?? Player;
        Level.CallDeferred("add_child", explosion);
    }

    public static void GameOver()
    {
        PlayerHud.ShowDeathQuote();
        
    }

    public static void StartRun()
    {
        StartRun(GD.Randi());
    }
    
    public static void StartRun(uint seed)
    {
        InRun = true;
        LootRng.Seed = seed;
        LevelRng.Seed = seed;
        BossRng.Seed = seed;
        CurrentRun = new RunInfo();
        CurrentRun.Seed = seed;
        CurrentRun.CurrentLevel = 1;
        
        
    }
}