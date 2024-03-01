using Godot;
using ProjectReaper.Enemies;
using System;
using ProjectReaper.Globals;

public partial class leechbert : AbstractCreature
{
    private enum BossState
    {
        Idle,
        Charging,
        Stopped
    }

    private BossState currentState = BossState.Idle;
    private Vector2 chargeDirection = Vector2.Zero;
    private float chargeSpeed = 500.0f; // Adjust the charge speed as needed
    private float stopDistance = 100.0f; // Adjust the stopping distance as needed
    private Node2D player;
    
    private const float Accelfac = 20.0f;
    
    public Vector2 MoveDirection { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      
    }

    public override void _Process(double delta)
    {
        MoveAndSlide();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
		var player = GameManager.Player;
        
        switch (currentState)
        {
            case BossState.Idle:
                // Check if the boss should start charging
                if (IsPlayerInRange())
                {
                    StartCharging();
                }
                break;
            case BossState.Charging:
                // Move the boss towards the player
                MoveTowardsPlayer(delta);
                // Check if the boss should stop charging
                if (IsCloseEnoughToPlayer())
                {
                    StopCharging();
                }
                break;
            case BossState.Stopped:
                // Boss is stopped, perform any idle behavior or attacks
                break;
        }
        
        if (Velocity.Length() > Stats.Speed || MoveDirection == Vector2.Zero)
        {
            Velocity = Velocity.Lerp(Vector2.Zero, 0.1f);
        }
    }

    private bool IsPlayerInRange()
    {
        if (player != null)
        {
            return GlobalPosition.DistanceTo(player.GlobalPosition) < 500.0f; // Adjust the range as needed
        }
        return false;
    }

    private void StartCharging()
    {
        currentState = BossState.Charging;
        // Calculate charge direction towards the player
        if (player != null)
        {
            chargeDirection = (player.GlobalPosition - GlobalPosition).Normalized();
        }
    }

    private void MoveTowardsPlayer(double delta)
    {
        // Move the boss in the charge direction
        MoveDirection = (player.GlobalPosition - GlobalPosition).Normalized();
        Velocity += MoveDirection * Stats.Speed * (float)delta * Accelfac;
    }

    private bool IsCloseEnoughToPlayer()
    {
        if (player != null)
        {
            return GlobalPosition.DistanceTo(player.GlobalPosition) < stopDistance; // Adjust the distance as needed
        }
        return false;
    }

    private void StopCharging()
    {
        currentState = BossState.Stopped;
        // Stop the boss
        Velocity = Vector2.Zero;
    }
}
