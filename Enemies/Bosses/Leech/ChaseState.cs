using Godot;
using ProjectReaper.Components;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies.Bosses.Leech;

public partial class ChaseState : AbstractState
{

   
    private Timer _timer;
    private bool _canCharge = true;
    private float _interest = 1;
    [Export] public float EnterCooldown = 1.0f;
    
    
    

    public override void _Ready()
    {
        base._Ready();
        _timer = new Timer();
        _timer.WaitTime = EnterCooldown;
        _timer.OneShot = true;
        _timer.Timeout += () =>
        {
            _canCharge = true;
        };
        AddChild(_timer);
    }
    
    public override void OnEnter(params object[] args)
    {
        _canCharge = false;
        _timer.Start();
        
    }

    public override void Update(double delta)
    {
        if (GameManager.Player.Dead) return;
        
        var distance = (StateMachine.Creature.GlobalPosition - GameManager.Player.GlobalPosition).Length();
        
        if (distance < 1000 && _canCharge)
        {
            StateMachine.ChangeState("ChargeState");
             _timer.Start();
             _canCharge = false;
             
        }
        
    }

    public override void PhysicsUpdate(double delta)
    {
        var leechbert = StateMachine.Creature as Leechbert;
        // raycast to player
        var player = GameManager.Player;
        if (player.Dead) return;

        if (player.GlobalPosition.DistanceTo(leechbert.GlobalPosition) < 500){
            var parameters = new PhysicsRayQueryParameters2D();
            parameters.From = leechbert.GlobalPosition;
            parameters.To = player.GlobalPosition;
            // bit 1 is terrain
            parameters.CollisionMask = 1;

            var ray = leechbert.GetWorld2D().DirectSpaceState.IntersectRay(parameters);
            
            if (ray.Count > 0)
            {
                _interest -= (float) delta;
            }
            else
            {
                _interest = 1;
            }
            
            if (_interest < 0)
            {
                StateMachine.ChangeState("WanderState");
            }
            
            
        }
        

        // move towards player
        leechbert.MoveDirection = (player.GlobalPosition - leechbert.GlobalPosition).Normalized();
        leechbert.Velocity += leechbert.MoveDirection * leechbert.Stats.Speed * (float)delta * Leechbert.Accelfac;

        // simulate friction with delta
        if (leechbert.Velocity.Length() > leechbert.Stats.Speed || leechbert.MoveDirection == Vector2.Zero)
        {
            leechbert.Velocity = leechbert.Velocity.Lerp(Vector2.Zero, 0.1f);
        }
    }
}