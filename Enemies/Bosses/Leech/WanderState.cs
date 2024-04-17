using Godot;
using ProjectReaper.Components;
using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Enemies.Bosses.Leech;

public partial class WanderState : AbstractState
{
    private float _interest = 0;
    private Vector2 spawnPoint;
    private NavigationAgent2D _navigationAgent;
    private Vector2 _movementTarget;

    public override void _Ready()
    {
        base._Ready();

        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();
    }

    public override void OnEnter(object[] args)
    {
        var leechbert = StateMachine.Creature as Leechbert;
        leechbert.MoveDirection = Vector2.Zero;
        leechbert.Velocity = Vector2.Zero;
        
        _navigationAgent.TargetPosition = spawnPoint;
    }
    
    

    public override void PhysicsUpdate(double delta)
    {
        var leechbert = StateMachine.Creature as Leechbert;
        var player = GameManager.Player;
        
        if (player.GlobalPosition.DistanceTo(leechbert.GlobalPosition) < 500){
            var parameters = new PhysicsRayQueryParameters2D();
            parameters.From = leechbert.GlobalPosition;
            parameters.To = player.GlobalPosition;
            // bit 1 is terrain
            parameters.CollisionMask = 1;

            var ray = leechbert.GetWorld2D().DirectSpaceState.IntersectRay(parameters);
            
            if (ray.Count == 0)
            {
                _interest += (float) delta;
            }
            else
            {
                _interest = 0;
            }
            
            if (_interest > 1)
            {
                StateMachine.ChangeState("ChaseState");
            }
            
            
        }
        
        if (leechbert.GlobalPosition.DistanceTo(spawnPoint) > 10)
        {
            FollowPath(delta);
        }
        
        if (leechbert.Velocity.Length() > leechbert.Stats.Speed || leechbert.MoveDirection == Vector2.Zero)
        {
            leechbert.Velocity = leechbert.Velocity.Lerp(Vector2.Zero, (float)delta * 2f);
        }
        
    }
    
    private void FollowPath(double delta)
    {
        var leechbert = StateMachine.Creature as Leechbert;
        var nextPos = _navigationAgent.GetNextPathPosition();
        if (nextPos != Vector2.Zero)
        {
            leechbert.MoveDirection = (nextPos - leechbert.GlobalPosition).Normalized();
            leechbert.Velocity += leechbert.MoveDirection * leechbert.Stats.Speed * (float)delta * Leechbert.Accelfac;
        }
        else
        {
            leechbert.MoveDirection = Vector2.Zero;
        }

        // simulate friction with delta
        
    }
    
    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        
        spawnPoint = StateMachine.Creature.GlobalPosition;
        
        _navigationAgent = StateMachine.Creature.GetNode<NavigationAgent2D>("NavigationAgent2D");

        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        _navigationAgent.PathDesiredDistance = 4.0f;
        _navigationAgent.TargetDesiredDistance = 4.0f;

        // Now that the navigation map is no longer empty, set the movement target.
        _movementTarget = spawnPoint;
    }
}