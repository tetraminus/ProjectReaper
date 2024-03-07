using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Player;
using ProjectReaper.Util;

namespace ProjectReaper.Globals;

public partial class GameManager : Node
{
    public static PackedScene ExplosionScene = ResourceLoader.Load<PackedScene>("res://Abilities/Projectiles/Explosion.tscn");
    public static PackedScene PlayerHudScene = ResourceLoader.Load<PackedScene>("res://Player/PlayerHud.tscn");
    public static PackedScene PauseMenuScene = ResourceLoader.Load<PackedScene>("res://Menu/PauseMenu.tscn");
    public static PackedScene PlayerScene = ResourceLoader.Load<PackedScene>("res://Player/player.tscn");
    public static PackedScene MainMenuScene = ResourceLoader.Load<PackedScene>("res://Menu/MainMenu/MainMenu.tscn");
    public static PackedScene ScreenFaderScene = ResourceLoader.Load<PackedScene>("res://Util/ScreenFader.tscn");
    // Called when the node enters the scene tree for the first time.
    public static Player.Player Player { get; set; }
    public static PlayerHud PlayerHud { get; set; }
    public static Level Level { get; set; }
    public static bool Paused { get; set; }
    public static bool InRun { get; set; }
    public static PauseMenu PauseMenu { get; set; }
    public static RunInfo CurrentRun { get; set; }
    public static Node2D MainNode { get; set; }
    public static MainMenu MainMenu { get; set; }
    public static ScreenFader ScreenFader { get; set; }


    public static RandomNumberGenerator LootRng = new RandomNumberGenerator();
    public static RandomNumberGenerator LevelRng = new RandomNumberGenerator();
    public static RandomNumberGenerator BossRng = new RandomNumberGenerator();
    

    public override void _Ready()
    {
        var playerhudScene = PlayerHudScene.Instantiate<CanvasLayer>();
        var pauseMenuScene = PauseMenuScene.Instantiate<CanvasLayer>();
        AddChild(playerhudScene);
        AddChild(pauseMenuScene);
        PlayerHud.Hide();
        PauseMenu.Hide();
        
        SetupMenu();
        
        var screenFaderlayer = ScreenFaderScene.Instantiate();
        AddChild(screenFaderlayer);
        
        ScreenFader = screenFaderlayer.GetNode("ScreenFader") as ScreenFader;
        
    }

    private void SetupMenu()
    {
        ItemLibrary.Instance.LoadItems();
        
        var menu = MainMenuScene.Instantiate<CanvasLayer>();
        AddChild(menu);
        MainMenu = menu.GetNode("%MainMenu") as MainMenu;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    

    

    public static void PauseGame()
    {
        if (!InRun) return;
        
        Paused = true;
        Level.GetTree().Paused = true;
        PauseMenu.Show();
        
    }
    
    public static void UnpauseGame()
    {
        if (!InRun) return;
        
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
        CurrentRun = null;
        
    }

    public static void StartRun()
    {
        StartRun(GD.Randi());
    }
    
    public async static void StartRun(uint seed)
    {
        
        ScreenFader.FadeOut(1);
        await MainNode.ToSignal(ScreenFader, ScreenFader.SignalName.FadeOutComplete);
        
        InRun = true;
        LootRng.Seed = seed;
        LevelRng.Seed = seed;
        BossRng.Seed = seed;
        CurrentRun = new RunInfo();
        CurrentRun.Seed = seed;
        CurrentRun.CurrentLevel = 1;
        Level = LevelLoader.Instance.GetRandomLevelScene(CurrentRun.CurrentLevel).Instantiate<Level>();
        MainNode.AddChild(Level);
        
        Player = PlayerScene.Instantiate<Player.Player>();
        CurrentRun.Player = Player;
        Level.AddPlayer(Player);
        
        Level.Generate();
        
        PlayerHud.Show();
        PlayerHud.SetPlayer(Player);
        MainMenu.Hide();
        
        ScreenFader.FadeIn(1);
        
        
    }

    public static void GoToMainMenu()
    {
        if (MainMenu.GetParent() == null)
        {
            MainNode.AddChild(MainMenu);
            
        }
        UnpauseGame();
        
        SpawnDirector.Instance.Clear();
        InRun = false;
        PlayerHud.Hide();
        PauseMenu.Hide();
        Level?.QueueFree();
        Player?.QueueFree();
        Callable.From(ClearVariables).CallDeferred();
        
        MainMenu.Show();
        
    }
    
    public static void ClearVariables()
    {
        Player = null;
        Level = null;
        CurrentRun = null;
    }

    public static async void GoToNextLevel(bool playAnimation = true)
    {
        if (playAnimation)
        {

            ScreenFader.FadeOut(1);
            await ScreenFader.ToSignal(ScreenFader, ScreenFader.SignalName.FadeOutComplete);
        }

        Player.Reparent(MainNode);
        Level.QueueFree();
        await Level.ToSignal(Level, "tree_exited");

        CurrentRun.CurrentLevel++;
        Level = LevelLoader.Instance.GetRandomLevelScene(CurrentRun.CurrentLevel).Instantiate<Level>();
        MainNode.AddChild(Level);
        Level.AddPlayer(Player);
        Level.Generate();

        if (playAnimation)
        {
          ScreenFader.FadeIn(1);
        }
    }   
    
}