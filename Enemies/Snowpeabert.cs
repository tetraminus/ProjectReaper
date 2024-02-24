using System;
using Godot;
using ProjectReaper.Abilities;

namespace ProjectReaper.Enemies;

public partial class Snowpeabert : AbstractCreature {
    private float _burrowTime = 2.0f; // Time in seconds for how long the character will burrow
    private Timer _burrowTimer;
    private CollisionShape2D _collisionShape;
    private EnemyState _currentState = EnemyState.Burrowing;
    private ulong _lastTimeMs;
    private float _movementDist = 25.0f;
    private Random _random = new();
    private Vector2 _randomDirection = Vector2.Zero; // Store the random direction
    private SnowPeaShoot _shooting = new();
    private Timer _shootTimer;
    private GpuParticles2D _particles;
    public AnimatedSprite2D Sprite;
    private Timer _stateTimer;
    private float _shootangle;
    private bool _justStartedShooting = false;


    private void Burrow() {
        _currentState = EnemyState.Burrowing;
        Sprite.Visible = false; // Make the KinematicBody2D invisible
        HitState = HitBoxState.Spectral; // Make the enemy invincible
        _burrowTimer.Start(2); // Start the timer
        _particles.Emitting = true;
    }

    private void Unburrow() {
        _currentState = EnemyState.Shooting;
        _justStartedShooting = true;
        Sprite.Visible = true; // Make the KinematicBody2D visible again
        HitState = HitBoxState.Normal;
        _burrowTimer.Start(2);
        _particles.Emitting = false;
    }

    public override void _Ready() {
        Stats.Init();
        Stats.Health = 20;
        _particles = GetNode<GpuParticles2D>("BurrowParticles");
        

        // Create and add a timer for state switching


        _randomDirection.X = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * _movementDist;
        _randomDirection.Y = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * _movementDist;
        _randomDirection.X += GlobalPosition.X;
        _randomDirection.Y += GlobalPosition.Y;

        AddChild(_shooting);

        Sprite = FindChild("Snowboy") as AnimatedSprite2D;
        Sprite.Play();


        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D"); // Replace with your actual CollisionShape2D path
        
        _burrowTimer = new Timer();
        AddChild(_burrowTimer);
        _burrowTimer.Timeout += () => {
            GD.Print(_currentState);
            if (_currentState == EnemyState.Burrowing) {
                Unburrow();
            } else {
                Burrow();
            }
        };
        Unburrow();
    }

    public override void _Process(double delta) {
        if (Position.X > Globals.GameManager.Player.Position.X)
            Sprite.FlipH = true;
        else
            Sprite.FlipH = false;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta) {
        switch (_currentState) {
            case EnemyState.Shooting:
                ShootState(delta);
                break;
            case EnemyState.Burrowing:
                MoveRandomly(_randomDirection * Stats.Speed);
                break;
        }
    }

    private void MoveTowardsPlayer() {
        // Get the player's position
        Node player = Globals.GameManager.Player;
        if (player == null) return;
        var playerPosition = ((Node2D)player).GlobalPosition;

        // Calculate the direction to the player
        var direction = (playerPosition - GlobalPosition).Normalized();

        // Move the enemy towards the player
        if (Math.Abs((playerPosition - GlobalPosition).Length()) > 100) {
            Velocity = direction * Stats.Speed;
            MoveAndSlide();
        }
    }


    private void MoveRandomly(Vector2 vector2) {
        // Generate a random direction
        //Vector2 randomDirection = new Vector2((float)GD.Randf() * 2 - 1, (float)GD.Randf() * 2 - 1).Normalized();
        var currentTimeMs = Time.GetTicksMsec();

        if (GlobalPosition.DistanceSquaredTo(_randomDirection) < 5.0f) {
            _randomDirection.X = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * _movementDist;
            _randomDirection.Y = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * _movementDist;
            if ((float)GD.RandRange(0.0f, 1.0f) > 0.5f) _randomDirection.X *= -1.0f;
            if ((float)GD.RandRange(0.0f, 1.0f) > 0.5f) _randomDirection.Y *= -1.0f;
            _randomDirection.X += GlobalPosition.X;
            _randomDirection.Y += GlobalPosition.Y;
        }

        GlobalPosition = GlobalPosition.Lerp(_randomDirection, 0.05f);
    }


    private void ShootState(double delta) {
        if (_justStartedShooting) {
            _justStartedShooting = false;
            // set the target position to a spot in near the player
            var angletoPlayer = GlobalPosition.AngleToPoint(Globals.GameManager.Player.GlobalPosition);
            angletoPlayer += (GD.Randf() > 0.5) ? 0.7f : -0.7f;
            _shootangle = angletoPlayer;
        }
        _shooting.Use(_shootangle);
        // rotate the angle towards the angle to the player
        
        // account for the 0-2pi radian wrap
        var anglediff =_shootangle -GlobalPosition.AngleToPoint(Globals.GameManager.Player.GlobalPosition) ;
        if (anglediff > Math.PI) anglediff -= (float)(2 * Math.PI);
        if (anglediff < -Math.PI) anglediff += (float)(2 * Math.PI);
        if (Math.Abs(anglediff) < 0.03) return;
        _shootangle += (anglediff > 0) ? (float)(-1f * delta) : (float)(1f * delta);
        
    }
    


    private enum EnemyState {
        Shooting,
        Burrowing
    }
}