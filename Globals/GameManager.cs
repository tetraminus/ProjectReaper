using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Menu.ItemLibraryScreen;
using ProjectReaper.Player;
using ProjectReaper.Util;

namespace ProjectReaper.Globals;

public partial class GameManager : Node
{
    public static PackedScene ExplosionScene = ResourceLoader.Load<PackedScene>("res://Abilities/Projectiles/Explosion.tscn");
    public static PackedScene DamageNumberScene = ResourceLoader.Load<PackedScene>("res://Util/DamageNumber.tscn");
    
    public static PackedScene PlayerHudScene = ResourceLoader.Load<PackedScene>("res://Player/PlayerHud.tscn");
    public static PackedScene PauseMenuScene = ResourceLoader.Load<PackedScene>("res://Menu/PauseMenu.tscn");
    public static PackedScene PlayerScene = ResourceLoader.Load<PackedScene>("res://Player/player.tscn");
    public static PackedScene MainMenuScene = ResourceLoader.Load<PackedScene>("res://Menu/MainMenu/MainMenu.tscn");
    public static PackedScene ScreenFaderScene = ResourceLoader.Load<PackedScene>("res://Util/ScreenFader.tscn");
    public static PackedScene ItemLibraryScene = ResourceLoader.Load<PackedScene>("res://Menu/ItemLibraryScreen/ItemLibraryScreen.tscn");
    // Called when the node enters the scene tree for the first time.
    public static Player.Player Player { get; set; }
    public static Node PlayerRoot { get; set; }
    public static PlayerHud PlayerHud { get; set; }
    public static Level Level { get; set; }
    public static bool Paused { get; set; }
    public static bool InRun { get; set; }
    public static PauseMenu PauseMenu { get; set; }
    public static RunInfo CurrentRun { get; set; }
    public static Node2D MainNode { get; set; }
    public static MainMenu MainMenu { get; set; }
    public static ScreenFader ScreenFader { get; set; }
    public static ItemLibraryScreen ItemLibraryScreen { get; set; }
    
    public static Control CurrentScreen { get; set; }
    public static Control LastScreen { get; set; }
    public static bool fadingOut = false;
    


    public static RandomNumberGenerator LootRng = new RandomNumberGenerator();
    public static RandomNumberGenerator LevelRng = new RandomNumberGenerator();
    public static RandomNumberGenerator BossRng = new RandomNumberGenerator();
    
    public static bool RollBool(float chance, int luck = 1, RandomNumberGenerator rng = null)
    {
        if (rng == null)
        {
            
            var roll = RollFloat(luck) > 1 - chance;
            return roll;
        }
        else
        {
            return RollFloat(luck, rng) > 1 - chance;
        }
    }
    
    public static bool RollProc(float chance, AbstractDamageArea damageArea , int luck = 1)
    {
        return RollBool(chance * damageArea.ProcCoef, luck);
    }
    
    public static float RollFloat( int luck = 1,  RandomNumberGenerator rng = null)
    {
        float highest = 0;
        if (rng == null)
        {
            
            for (int i = 0; i < luck; i++)
            {
                var roll = GD.Randf();
                if (roll > highest)
                {
                    highest = roll;
                }
            }
        }
        else
        {
            for (int i = 0; i < luck; i++)
            {
                var roll = rng.Randf();
                if (roll > highest)
                {
                    highest = roll;
                }
            }
        }
        
        return highest;
    }
    
    public override void _Ready()
    {
        var playerhudScene = PlayerHudScene.Instantiate<CanvasLayer>();
        var pauseMenuScene = PauseMenuScene.Instantiate<CanvasLayer>();
        var library = ItemLibraryScene.Instantiate<Control>();
        AddChild(playerhudScene);
        AddChild(pauseMenuScene);
        pauseMenuScene.AddChild(library);
        ItemLibraryScreen.CloseRequested += ItemLibraryScreenOnCloseRequested;
        ItemLibraryScreen.Hide();
        PlayerHud.Hide();
        PauseMenu.Hide();
        
        SetupMenu();
        
        var screenFaderlayer = ScreenFaderScene.Instantiate();
        AddChild(screenFaderlayer);
        
        ScreenFader = screenFaderlayer.GetNode("ScreenFader") as ScreenFader;
        
        
    }

    public override void _Process(double delta)
    {
        if (InRun && !Paused && CurrentRun != null)
        {
            CurrentRun.Time += delta;
        }
        
        
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
        PauseMenu.GrabFocus();
        CurrentScreen = PauseMenu;
        
    }
    
    public static void UnpauseGame()
    {
        if (!InRun) return;
        
        Paused = false;
        Level.GetTree().Paused = false;
        PauseMenu.Hide();
        CurrentScreen = null;
    }

    public static void SpawnExplosion(Vector2 globalPosition, float damage, float scale = 1f,
        AbstractCreature creature = null)
    {
        var explosion = (Explosion)ExplosionScene.Instantiate();
        explosion.GlobalPosition = globalPosition;
        explosion.Scale = new Vector2(scale, scale);
        explosion.setDamage(damage);
        explosion.Source = creature ?? Player;
        Level.CallDeferred("add_child", explosion);
    }
    
    public static void SpawnDamageNumber(Vector2 globalPosition, float damage)
    {
        var damageNumber = DamageNumberScene.Instantiate<DamageNumber>();
        damageNumber.GlobalPosition = globalPosition;
        Level.AddChild(damageNumber);
        damageNumber.SetNumber(damage);
        
    }

    public static void GameOver()
    {
        PlayerHud.ShowDeathQuote();
        CurrentRun = null;
        SpawnDirector.Instance.Clear();
        
    }

    public static void StartRun()
    {
        StartRun(GD.Randi());
    }
    
    public async static void StartRun(uint seed)
    {
        
        fadingOut = true;
        ScreenFader.FadeOut(1);
        await MainNode.ToSignal(ScreenFader, ScreenFader.SignalName.FadeOutComplete);
        
        InRun = true;
        LootRng.Seed = seed;
        LevelRng.Seed = seed;
        BossRng.Seed = seed;
        CurrentRun = new RunInfo();
        CurrentRun.Seed = seed;
        CurrentRun.CurrentLevel = 1;
        CurrentRun.Time = 0;
        Level = LevelLoader.Instance.GetRandomLevelScene(CurrentRun.CurrentLevel).Instantiate<Level>();
        MainNode.AddChild(Level);
        
        PlayerRoot = PlayerScene.Instantiate<Node>();
        Player = PlayerRoot.GetNode<Player.Player>("%Player");
        
        CurrentRun.Player = Player;
        Level.AddPlayer(PlayerRoot);
        
        Level.Generate();
        
        PlayerHud.Show();
        PlayerHud.SetPlayer(Player);
        MainMenu.Hide();
        CurrentScreen = null;
        
        ScreenFader.FadeIn(1);
        fadingOut = false;
        
        
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
        PlayerHud.Reset();
        Level?.QueueFree();
        Player?.QueueFree();
        
        Callable.From(ClearVariables).CallDeferred();
        
        MainMenu.Show();
        MainMenu.GrabFocus();
        
        CurrentScreen = MainMenu;

    }
    
    public static float GetRunDifficulty()
    {
        if (CurrentRun == null) return -1;
        return (float)(1 + (CurrentRun.Time) / 60f);
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

        Level.RemoveChild(PlayerRoot);
        Level.QueueFree();
        await Level.ToSignal(Level, Node.SignalName.TreeExited);

        CurrentRun.CurrentLevel++;
        Level = LevelLoader.Instance.GetRandomLevelScene(CurrentRun.CurrentLevel).Instantiate<Level>();
        MainNode.AddChild(Level);
        Level.AddPlayer(PlayerRoot);
        Level.Generate();

        if (playAnimation)
        {
          ScreenFader.FadeIn(1);
        }
    }

    public static void GoToLibrary()
    {
        GD.Print("Going to library");
        
        
        CurrentScreen?.Hide();
        LastScreen = CurrentScreen;
        CurrentScreen = ItemLibraryScreen;
        
        ItemLibraryScreen.LoadItems();
        ItemLibraryScreen.Focus();
        ItemLibraryScreen.Show();
        
    }

    private static void ItemLibraryScreenOnCloseRequested()
    {
        GD.Print("Closing library");
        ItemLibraryScreen.Hide();
        LastScreen.Show();
        CurrentScreen = LastScreen;
        
        
    }
}