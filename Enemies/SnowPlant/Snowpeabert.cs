using System;
using Godot;
using GodotStateCharts;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies.SnowPlant;

public partial class Snowpeabert : AbstractCreature
{
    private static PackedScene BulletScene { get; } =
        GD.Load<PackedScene>("res://Abilities/Projectiles/BasicBullet.tscn");
    private const float ShootSweepAngle = 45.0f; // The angle in degrees for the sweep of the shooting state
    private const int ShootSweepCount = 10; // The number of shots to be fired in the shooting state
    private const float Accelfac = 20.0f;

    private float _burrowTime = 2.0f; // Time in seconds for how long the character will burrow
    private GpuParticles2D _particles;
    private float _shootangle;
    private float _shootstartangle;
    private float CooldownTimer = 0;
    private AnimatedSprite2D Sprite;
    private StateChart _stateChart;
    
    private const float TargetChaseDistance = 50;

    public Vector2 MoveDirection { get; set; }
    private NavigationAgent2D _navigationAgent;
    private Vector2 _movementTargetPosition = Vector2.Zero;
    private bool _directSight = false;
    public Vector2 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }

    private void Burrow()
    {
        _stateChart = StateChart.Of(GetNode("StateChart"));
        _stateChart.SendEvent("Burrow");
        Sprite.Visible = false; // Make the KinematicBody2D invisible
        HitState = HitBoxState.Spectral; // Make the enemy invincible
        _particles.Emitting = true;
        
        // Debug line to check if HitState is being updated correctly
        GD.Print($"HitState after burrowing: {HitState}");


    }

    private void Unburrow()
    {
        _stateChart = StateChart.Of(GetNode("StateChart"));
        _stateChart.SendEvent("Unburrow");
        Sprite.Visible = true; // Make the KinematicBody2D visible again
        HitState = HitBoxState.Normal;
        _particles.Emitting = false;
        EnterShoot();
        
    }

    public override void _Ready()
    {
        _stateChart = StateChart.Of(GetNode("StateChart"));
        _stateChart.SendEvent("Burrow");

        base._Ready();
        Stats.MaxHealth = 60;
        Stats.Health = Stats.MaxHealth;
        Stats.Speed = 100;
        _particles = GetNode<GpuParticles2D>("BurrowParticles");

        Sprite = FindChild("Snowboy") as AnimatedSprite2D;
        Sprite.Play();

        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

        _navigationAgent.PathDesiredDistance = 4.0f;
        _navigationAgent.TargetDesiredDistance = 4.0f;

        Callable.From(ActorSetup).CallDeferred();

        Callbacks.Instance.EnemyRenav += Renav;
    }

    public override void _ExitTree()
    {
        Callbacks.Instance.EnemyRenav -= Renav;
        base._ExitTree();
        
    }


    private void Renav(Vector2 position, int group)
    {
        if (group != NavGroup) return;
        MovementTarget = position;
        GD.Print("working");
    }

    public void OnShootingStateEntered()
    {
        var angleToPlayer = GetAngleTo(GameManager.Player.GlobalPosition);
        _shootstartangle = angleToPlayer - ShootSweepAngle/2;
        _shootstartangle += (GD.Randf() > 0.5f ? ShootSweepAngle : -ShootSweepAngle) / 2;
        _shootangle = _shootstartangle;
    }
    public void ChasePlayer(double delta)
    {
        var player = GameManager.Player;
        if (player.Dead) return;

        float distanceToPlayer = player.GlobalPosition.DistanceTo(GlobalPosition);

        if (distanceToPlayer > TargetChaseDistance + 50)
        {
            MoveDirection = (player.GlobalPosition - GlobalPosition).Normalized();
        }
        else if (distanceToPlayer < TargetChaseDistance)
        {
            MoveDirection = (GlobalPosition - player.GlobalPosition).Normalized();
        }
        else
        {
            MoveDirection = Vector2.Zero;
        }

        Velocity += MoveDirection * Stats.Speed * (float)delta * Accelfac;
    }
    public void ShootingStateProcess(float delta)
    {
        // shoot bullets from one side to the other every 0.2 seconds
        if (CooldownTimer > 0.2f)
        {
            var bullet = (AbstractDamageArea)BulletScene.Instantiate();
            bullet.Init(this, Team, GlobalPosition, _shootangle);
            _shootangle += ShootSweepAngle/10;
            CooldownTimer = 0;
        }
        CooldownTimer += delta;


    }

    private void FollowPath(double delta)
    {
        var nextPos = _navigationAgent.GetNextPathPosition();
        if (nextPos != Vector2.Zero)
        {
            MoveDirection = (nextPos - GlobalPosition).Normalized();
            Velocity += MoveDirection * Stats.Speed * (float)delta * Accelfac;
        }
        else
        {
            MoveDirection = Vector2.Zero;
            Velocity = Vector2.Zero;

        }

    }
    private void Move() {
        // simulate friction with delta
        if (Velocity.Length() > Stats.Speed || MoveDirection == Vector2.Zero)
        {
            Velocity = Velocity.Lerp(Vector2.Zero, 0.1f);
        }

        MoveAndSlide();
    }

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
        MovementTarget = GameManager.Player.GlobalPosition;
    }   
    public override void _PhysicsProcess(double delta) {
        // raycast to player
        var player = GameManager.Player;
        if (player.Dead) return;

        if (player.GlobalPosition.DistanceTo(GlobalPosition) < 500) {
            var parameters = new PhysicsRayQueryParameters2D();
            parameters.From = GlobalPosition;
            parameters.To = player.GlobalPosition;
            // bit 1 is terrain
            parameters.CollisionMask = 1;

            var ray = GetWorld2D().DirectSpaceState.IntersectRay(parameters);

            _directSight = ray.Count == 0;
        }
        else {
            _directSight = false;
        }
        
        if (_directSight) {
            _stateChart.SendEvent("PlayerSeen");
            
        }
        else {
            _stateChart.SendEvent("PlayerLost");
        }
        Move(); 
    }

    public override void _Process(double delta)
    {
        Sprite.FlipH = MoveDirection.X > 0;
    }



    public void EnterShoot()
    {
        if (GameManager.Player.Dead) return;
        ShootBasicBullet(GetAngleTo(GameManager.Player.GlobalPosition));
        ShootBasicBullet(GetAngleTo(GameManager.Player.GlobalPosition)+0.4f);
        ShootBasicBullet(GetAngleTo(GameManager.Player.GlobalPosition)-0.4f);
        
        MoveDirection = Vector2.Zero;
        Velocity = Vector2.Zero;
        
        
    }

    private void ShootBasicBullet(float rotation = 0)
    {
        var bullet = BulletScene.Instantiate<BasicBullet>();
        bullet.Init(this, Team, GlobalPosition, rotation);
        bullet.Speed = 200;
        GameManager.Level.AddChild(bullet);
    }

}