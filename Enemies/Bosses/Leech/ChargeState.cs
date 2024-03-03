using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Components;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies.Bosses.Leech;

public partial class ChargeState : AbstractState
{
    private Vector2 _chargeDirection;
    private bool _charging;
    [Export] private float _chargeTime;
    private MeleeArea meleeArea;
    private Node2D meleePivot; 
    
    public override void _Ready()
    {
        meleePivot = GetNode<Node2D>("%MeleePivot");
        meleeArea = GetNode<MeleeArea>("%MeleeArea");
        meleeArea.Disable();
        
        InitMeleeArea();
        
        
    }

    private async void InitMeleeArea()
    {
        await ToSignal(GetTree(), "process_frame");
        meleeArea.Init(StateMachine.Creature, StateMachine.Creature.Team);
        meleeArea.KBType = MeleeArea.KnockbackType.Charge;
    }

    public override void OnEnter()
    {
        meleeArea.Enable();
        base.OnEnter();
        var leechbert = StateMachine.Creature as leechbert;
        GetTree().CreateTimer(_chargeTime).Timeout += LeaveCharge;
        _chargeDirection = (GameManager.Player.GlobalPosition - leechbert.GlobalPosition).Normalized();
        meleePivot.Rotation = _chargeDirection.Angle();
        _charging = true;
    }
    

    public override void PhysicsUpdate(double delta)
    {
        var leechbert = StateMachine.Creature as leechbert;
        if (_charging )
        {
            for (var i = 0; i < leechbert.GetSlideCollisionCount() ; i++)
            {
                var collision = leechbert.GetSlideCollision(i);
                if (collision.GetCollider() is TileMap || (collision.GetCollider() is PhysicsBody2D c && c.GetCollisionLayerValue(1)))
                {
                    leechbert.Velocity = Vector2.Zero;
                    LeaveCharge();
                }
                
            }
            
            leechbert.Velocity = _chargeDirection * leechbert.Stats.Speed * 5f;
        }
    }

    private void LeaveCharge()
    {
        meleeArea.Disable();
        _charging = false;
        StateMachine.ChangeState("ChaseState");
    }
}