using Godot;
using GodotStateCharts;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Components;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies.Bosses.Wizard;

public partial class WizardBoss : AbstractCreature
{
    private StateChart _stateChart;
    private Texture2D _bulletTexture = GD.Load<Texture2D>("res://Assets/Enemies/Eye_Blood.png");
    private PackedScene BulletScn = GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");
    public override void _Ready()
    {
        base._Ready();
        _stateChart = StateChart.Of(GetNode("StateChart"));
        
        Stats.MaxHealth = 1200;
        Stats.Health = 1200;
        //availableAttacks.AddRange(Attacks);
    }
    
    private void FireBullet(float direction, float speed = 200)
    {
        var bullet = BulletScn.Instantiate<BasicBullet>();
        bullet.Init(this, Team, GlobalPosition, direction);
        bullet.Speed = speed;
        bullet.Duration = -1;
        GameManager.Level.AddChild(bullet);
        bullet.Resprite(_bulletTexture, Vector2.Zero, -Mathf.Pi/2f);
    }
    
    public override float AimDirection()
    {
        return GlobalPosition.DirectionTo(GameManager.Player.GlobalPosition).Angle();
    }
    public override void OnDeath()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.BossDied);
        base.OnDeath();
    }
    
    
    public void ChoosingMove()
    {
        var move = GD.Randf();
        if (move < 0.5)
        {
            _stateChart.SendEvent("PickedSwirl");
        }
        else
        {
            _stateChart.SendEvent("PickedSpread");
        }
    }
    
    
    // --- teleport ---
    private static float _teleportTime = 1.5f;
    private const float _stunTime = 0.5f;
    
    private float _teleportTimer = 0f;
    private bool _teleported = false;
    
    private const float minDistance = 150f;
    private const float maxDistance = 200f;
    
    private const float targetdistance = 200f;
    
    private void EnterTeleport()
    {
        
        _teleportTimer = 0f;
        _teleported = false;
    }
    
    private void UpdateTeleport(float delta)
    {
        _teleportTimer += delta;
        

        if (_teleportTimer >= _teleportTime + _stunTime)
        {
            _stateChart.SendEvent("PickMove");
            return;
        }
        
        if (_teleportTimer >= _teleportTime && !_teleported)
        {
            DoTeleport();
            _teleported = true;
            
        }
    }
    
    private void DoTeleport()
    {
        var target = GetRandomPosition();
        Position = target;
    }

    private Vector2 GetRandomPosition()
    {
        // pick random angle based on the player position and distance
        var angle = GlobalPosition.DirectionTo(GameManager.Player.GlobalPosition).Angle();
        angle += GD.Randf() * Mathf.Pi - Mathf.Pi / 2;
        //flip the angle if too close to the player 
        if (GlobalPosition.DistanceTo(GameManager.Player.GlobalPosition) < targetdistance)
        {
            angle += Mathf.Pi;
        }
        
        var distance = minDistance + GD.Randf() * (maxDistance - minDistance);
        
        
        // move back if hit the wall
        var target = Position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        var projected = Level.CastToWall(Position , target);
        
        if (projected != target)
        {
            target = Position + (projected - Position).Normalized() * (projected.DistanceTo(Position) - 32);
        }
        
        return target;
    }


    // Spreadfire
    
    private float _spreadfireTimer = 0f;
    private float _spreadfireDirection = 0f;
    private int _spreadfireBulletCount = 0;
        
    private const int SpreadfireCount = 10;
    private const float SpreadfireAngle = Mathf.Pi / 4;
    private const float SpreadfireInterval = 0.01f;
    
    private void EnterSpreadfire()
    {
        _spreadfireTimer = 0;
        _spreadfireDirection = AimDirection();
        _spreadfireBulletCount = 0;
    }
    
    private void UpdateSpreadfire(float delta)
    {
        _spreadfireTimer += delta;
        if (_spreadfireTimer > SpreadfireInterval)
        {
            _spreadfireTimer = 0;
            FireBullet(_spreadfireDirection);
            _spreadfireDirection += SpreadfireAngle / SpreadfireCount;
            if (_spreadfireDirection > Mathf.Pi * 2)
            {
                _spreadfireDirection -= Mathf.Pi * 2;
            }

            _spreadfireBulletCount++;
            if (_spreadfireBulletCount >= SpreadfireCount)
            {
                _spreadfireBulletCount = 0;
                _stateChart.SendEvent("MoveDone");
            }
        }
    }
    
    // Swirlfire
    private PackedScene _swirlfireBulletScn = GD.Load<PackedScene>("res://Enemies/Bosses/Wizard/SwirlfireBullet.tscn");
    private float _swirlfireTimer = 0f;
    private float _swirlfireDirection = 0f;
    private float _firedBullets = 0;
    
    private const int SwirlfireCount = 30;
    private const float SwirlfireInterval = 0.05f;
    
    
    
    private void EnterSwirlfire()
    {
        _swirlfireTimer = 0;
        _swirlfireDirection = AimDirection() - Mathf.Pi / 2;
        _firedBullets = 0;
    }
    
    private void UpdateSwirlfire(float delta)
    {
        _swirlfireTimer += delta;
        if (_swirlfireTimer > SwirlfireInterval)
        {
            _swirlfireTimer = 0;
            FireSwirlfireBullet(_swirlfireDirection);
            FireSwirlfireBullet(_swirlfireDirection + Mathf.Pi);
            _firedBullets += 1;
            _swirlfireDirection += Mathf.Pi / SwirlfireCount;
            if (_swirlfireDirection > Mathf.Pi * 2)
            {
                _swirlfireDirection -= Mathf.Pi * 2;
            }
        }
        
        if (_firedBullets >= SwirlfireCount)
        {
            _stateChart.SendEvent("MoveDone");
        }
    }
    
    private void FireSwirlfireBullet(float direction)
    {
        var bullet = _swirlfireBulletScn.Instantiate<BulletGroup>();
        bullet.Init(this, Team, GlobalPosition, direction);
        GameManager.Level.AddChild(bullet);
    }
    
    
    
    
    
}