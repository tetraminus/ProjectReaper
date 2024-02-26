using System;
using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies;

public partial class Slimebert : AbstractCreature
{
    private AnimatedSprite2D _sprite;
    private NavigationAgent2D _navigationAgent;
    private PackedScene _bullet = GD.Load<PackedScene>("res://Abilities/Projectiles/SlimeBullet.tscn");
    private Timer Timer => GetNode<Timer>("Timer");
    
    private Vector2 _movementTargetPosition = Vector2.Zero;
    private bool _directSight = false;
    private EnemyState _state;
    private bool _justEnteredShooting = false;
    
    
    
    
    public Vector2 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }

    public override void _Ready()
    {
        base._Ready();
        
        Stats.Speed = 100;
        Stats.Health = 10;
        Stats.MaxHealth = 10;
        Stats.Damage = 0;

        _sprite = GetNode<AnimatedSprite2D>("Sprite");
        _sprite.Play();
        
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        _navigationAgent.PathDesiredDistance = 4.0f;
        _navigationAgent.TargetDesiredDistance = 4.0f;

        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();

        Callbacks.Instance.EnemyShouldRenavEvent += Renav;
        
        _state = EnemyState.Moving;
        
        Timer.Timeout += StateCheck;
        Timer.Start();
        
    }

    private void StateCheck()
    {
        if (_directSight)
        {
            if (_state == EnemyState.Shooting)
            {
                _state = EnemyState.Moving;
                Timer.WaitTime = 5.0f;
            }
            else
            {
                _state = EnemyState.Shooting;
                Timer.WaitTime = 1.0f;
                _justEnteredShooting = true;
            }
        }
        else
        {
            _state = EnemyState.Moving;
            Timer.WaitTime = 5.0f;
        }
    }

    public override void _ExitTree()
    {
        Callbacks.Instance.EnemyShouldRenavEvent -= Renav;
        base._ExitTree();
    }

    private void Renav(Vector2 position, int group)
    {
        if (group != navGroup) return;
        MovementTarget = position;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        MoveAndSlide();
        
        if (_justEnteredShooting)
        {
            _justEnteredShooting = false;
            Shoot();
        }
        
    }

    private void Shoot()
    {
        var bullet = _bullet.Instantiate<AbstractDamageArea>();
        Callbacks.Instance.BulletCreatedEvent?.Invoke(bullet);
        bullet.Position = GlobalPosition;
        bullet.LookAt(GameManager.Player.GlobalPosition);
        GameManager.Level.AddChild(bullet);
        bullet.Source = this;
        bullet.Team = Teams.Enemy;
    }

    public override void _PhysicsProcess(double delta)
    {
        
        if (_state != EnemyState.Moving) return;    
        
        // raycast to player
        var player = GameManager.Player;
        if (player.Dead) return;
        
        if (player.GlobalPosition.DistanceTo(GlobalPosition) < 500){
            var parameters = new PhysicsRayQueryParameters2D();
            parameters.From = GlobalPosition;
            parameters.To = player.GlobalPosition;
            // bit 1 is terrain
            parameters.CollisionMask = 1;

            var ray = GetWorld2D().DirectSpaceState.IntersectRay(parameters);

            _directSight = ray.Count == 0;
        }
        else
        {
            _directSight = false;
        }

        // follow nav path if no direct sight
        if (!_directSight)
        {
            FollowPath(delta);
        }
        // else move towards player
        else
        {
            Velocity = (player.GlobalPosition - GlobalPosition).Normalized() * Stats.Speed * (float)delta * 20f;
        }
        

    }

    private void FollowPath(double delta)
    {
        var nextPos = _navigationAgent.GetNextPathPosition();
        if (nextPos != Vector2.Zero)
        {
            var dir = (nextPos - GlobalPosition).Normalized();
            Velocity = dir * Stats.Speed * (float)delta * 20f;
        }
        else
        {
            Velocity = Vector2.Zero;
        }
    }

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
        MovementTarget = GameManager.Player.GlobalPosition;
    }


    private enum EnemyState
    {
        Moving,
        Shooting
    }
}