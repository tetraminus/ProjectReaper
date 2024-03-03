using Godot;
using System;
using ProjectReaper.Components;
using ProjectReaper.Enemies.Bosses;
using ProjectReaper.Globals;
using ProjectReaper.Util;

public partial class ChaseState : AbstractState
{
    
    private Timer _chargeTimer;
    private bool _canCharge = true;

    public override void _Ready()
    {
        base._Ready();
        _chargeTimer = new Timer();
        _chargeTimer.WaitTime = 1;
        _chargeTimer.OneShot = true;
        _chargeTimer.Timeout += () =>
        {
            _canCharge = true;
        };
        AddChild(_chargeTimer);
    }

    public override void Update(double delta)
    {
        var distance = (StateMachine.Creature.GlobalPosition - GameManager.Player.GlobalPosition).Length();
        if (distance < 1000 && _canCharge)
        {
            StateMachine.ChangeState("ChargeState");
            _chargeTimer.Start();
            _canCharge = false;
        }
    }

    public override void PhysicsUpdate(double delta)
    {
        var leechbert = StateMachine.Creature as leechbert;
        // raycast to player
        var player = GameManager.Player;
        if (player.Dead) return;

        // move towards player
        leechbert.MoveDirection = (player.GlobalPosition - leechbert.GlobalPosition).Normalized();
        leechbert.Velocity += leechbert.MoveDirection * leechbert.Stats.Speed * (float)delta * leechbert.Accelfac;

        // simulate friction with delta
        if (leechbert.Velocity.Length() > leechbert.Stats.Speed || leechbert.MoveDirection == Vector2.Zero)
        {
            leechbert.Velocity = leechbert.Velocity.Lerp(Vector2.Zero, 0.1f);
        }
    }
}
