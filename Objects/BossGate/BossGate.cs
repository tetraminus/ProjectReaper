using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Objects.BossGate;

public partial class BossGate : StaticBody2D, IInteractable
{
    [Export] private Node2D _spawnPoint;
    [Export] private Node2D _bossSpawnPoint;
    [Export] private PackedScene _bossScene;
    
    private bool _canInteract = true;
    
    public override void _Ready()
    {
        
        Callbacks.Instance.BossDiedEvent += BossDiedEvent;
    }

    private void BossDiedEvent()
    {
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }


    public void Interact()
    {
        SpawnDirector.Instance.KillAllEnemies();
        SpawnDirector.Instance.StopSpawning();
        GameManager.Player.GlobalPosition = _spawnPoint.GlobalPosition;

        var boss = _bossScene.Instantiate<AbstractCreature>();
        boss.GlobalPosition = _bossSpawnPoint.GlobalPosition;
        
        GameManager.Level.AddChild(boss);
        _canInteract = false;
    }

    public bool CanInteract()
    {
        return _canInteract;
    }
}