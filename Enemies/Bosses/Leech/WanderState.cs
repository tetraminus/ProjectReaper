using Godot;
using ProjectReaper.Components;
using ProjectReaper.Globals;
using ProjectReaper.Player;
using ProjectReaper.Util;

namespace ProjectReaper.Enemies.Bosses.Leech;

public partial class WanderState : AbstractState
{
    private float _interest = 0;
    private Vector2 spawnPoint;
    private NavigationAgent2D _navigationAgent;
    private Vector2 _movementTarget;
    private TextureRect _alertTexture;

    public override void _Ready()
    {
        base._Ready();

        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();
        
    }
    
    private void OnHit(DamageReport damageReport)
    {
        if (StateMachine.GetCurrentStateName() != "WanderState") return;
        if (damageReport.Source == null) return;
        
        if (damageReport.Source.Team == AbstractCreature.Teams.Player && damageReport.finalDamage > 0)
        {
            StateMachine.ChangeState("ChaseState");
            _alertTexture.Visible = true;
            _alertTexture.Modulate = new Color(1, 0, 0, 1);
        }
        
    }

    public override void OnEnter(object[] args)
    {
        var leechbert = StateMachine.Creature as Leechbert;
        leechbert.MoveDirection = Vector2.Zero;
        leechbert.Velocity = Vector2.Zero;
        if (_alertTexture != null) _alertTexture.Visible = false;
        _interest = 0;
        
        _navigationAgent.TargetPosition = spawnPoint;
    }
    
    

    public override void PhysicsUpdate(double delta)
    {
        var leechbert = StateMachine.Creature as Leechbert;
        var player = GameManager.Player;
        
        if (player.GlobalPosition.DistanceTo(leechbert.GlobalPosition) < 150){
            var parameters = new PhysicsRayQueryParameters2D();
            parameters.From = leechbert.GlobalPosition;
            
            parameters.To = player.GlobalPosition;
            // bit 1 is terrain
            parameters.CollisionMask = 1;

            var ray = leechbert.GetWorld2D().DirectSpaceState.IntersectRay(parameters);
            
            if (ray.Count == 0)
            {
                _interest += (float) delta;
                _alertTexture.Visible = true;
                _alertTexture.Modulate = new Color(1, 1, 0, 1);
            }
            else
            {
                _interest = 0;
                _alertTexture.Visible = false;
                
            }
            
            if (_interest > 1)
            {
                StateMachine.ChangeState("ChaseState");
                _alertTexture.Visible = true;
                _alertTexture.Modulate = new Color(1, 0, 0, 1);
            }
            
            
        }
        
        if (leechbert.GlobalPosition.DistanceTo(spawnPoint) > 10)
        {
            FollowPath(delta);
        }
        else
        {
            leechbert.MoveDirection = Vector2.Zero;
        }
        
        // simulate friction with delta
        if (leechbert.Velocity.Length() > leechbert.Stats.Speed || leechbert.MoveDirection == Vector2.Zero)
        {
            leechbert.Velocity = leechbert.Velocity.Lerp(Vector2.Zero, (float)delta * 10f);
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

        
    }

    public override void OnExit()
    {
        GetTree().CreateTimer(0.5f).Timeout += () =>
        {
            _alertTexture.Visible = false;
        };
        
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
        
        var leechbert = StateMachine.Creature as Leechbert;
        leechbert.Hit += OnHit;
        _alertTexture = StateMachine.Creature.GetNode<TextureRect>("AlertTexture");
    }
}