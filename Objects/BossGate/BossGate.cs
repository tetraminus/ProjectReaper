using System.Linq;
using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;




namespace ProjectReaper.Objects.BossGate;

public partial class BossGate : StaticBody2D, IInteractable
{
    
    private PackedScene _portalScene = GD.Load<PackedScene>("res://Objects/Portal/Portal.tscn");
    private PackedScene _bossChestScene = GD.Load<PackedScene>("res://Objects/Interactables/BossChest/BossChest.tscn");
    [Export] private Node2D _spawnPoint;
    [Export] private Node2D _bossSpawnPoint;
    [Export] private Node2D _portalSpawnPoint;
    [Export] private PackedScene _bossScene;
    [Export] private Node2D _phantomCamera;
    
    private bool _canInteract = true;
    
    public override void _Ready()
    {
        
        Callbacks.Instance.BossDied += BossDiedEvent;
    }
    
    public override void _ExitTree()
    {
        Callbacks.Instance.BossDied -= BossDiedEvent;
    }

    private void BossDiedEvent()
    {
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
        _phantomCamera.Set("priority", 0);
        var portal = _portalScene.Instantiate<Portal>();
        portal.GlobalPosition = _portalSpawnPoint.GlobalPosition;
        portal.PortalType = Portal.PortalTypes.Shop;
        
        var bossChest = _bossChestScene.Instantiate<BossChest>();
        bossChest.GlobalPosition = _bossSpawnPoint.GlobalPosition;
        
        GameManager.Level.CallDeferred(Node.MethodName.AddChild, portal);
        GameManager.Level.CallDeferred(Node.MethodName.AddChild, bossChest);

        GameManager.Level.FadeOutFightMusic();
    }


    public void Interact()
    {
        // kill all enemies
        SpawnDirector.Instance.KillAllEnemies();
        SpawnDirector.Instance.StopSpawning();
        
        // spawn player
        GameManager.Player.GlobalPosition = _spawnPoint.GlobalPosition;
        GameManager.Player.Velocity = Vector2.Zero;
        
        // spawn boss
        var boss = _bossScene.Instantiate<AbstractCreature>();
        boss.GlobalPosition = _bossSpawnPoint.GlobalPosition;
        GameManager.Level.AddChild(boss);
        
        // set camera to follow player and boss
        _phantomCamera.Set("priority", 15);
        _phantomCamera.Call("append_follow_targets", GameManager.Player);
        _phantomCamera.Call("append_follow_targets", boss);
        boss.TreeExiting += () =>
        {
            _phantomCamera.Call("erase_follow_targets", boss);
            _phantomCamera.Call("erase_follow_targets", GameManager.Player);
            _phantomCamera.Set("priority", 0);
        };
        GameManager.PlayerHud.PlayFightAnim();
        
        
        // disable interaction
        _canInteract = false;
        
        GameManager.Level.FadeInFightMusic();
    }

    public bool CanInteract()
    {
        return _canInteract;
    }
    
    public string GetPrompt(bool Interactable = false)
    {
        return Tr("ui_boss_gate");
    }
}