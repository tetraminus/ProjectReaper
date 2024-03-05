using System.Linq;
using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;




namespace ProjectReaper.Objects.BossGate;

public partial class BossGate : StaticBody2D, IInteractable
{
    [Export] private Node2D _spawnPoint;
    [Export] private Node2D _bossSpawnPoint;
    [Export] private PackedScene _bossScene;
    [Export] private Node2D _phantomCamera;
    
    private bool _canInteract = true;
    
    public override void _Ready()
    {
        
        Callbacks.Instance.BossDiedEvent += BossDiedEvent;
    }

    private void BossDiedEvent()
    {
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
        _phantomCamera.Set("priority", 0);
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
        _phantomCamera.Call("append_follow_group_node", GameManager.Player);
        _phantomCamera.Call("append_follow_group_node", boss);
        boss.TreeExiting += () =>
        {
            _phantomCamera.Set("priority", 0);
        };
        
        
        // disable interaction
        _canInteract = false;
    }

    public bool CanInteract()
    {
        return _canInteract;
    }
}