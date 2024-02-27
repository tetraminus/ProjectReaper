using Godot;
using ProjectReaper.Enemies;
using System;

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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get reference to the player
        Node player = GameManager.Player;
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
                MoveTowardsPlayer();
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

    private void MoveTowardsPlayer()
    {
        // Move the boss in the charge direction
		GD.Print(chargeDirection.X * chargeSpeed * GetProcessDeltaTime());
		GD.Print(chargeDirection.Y * chargeSpeed * GetProcessDeltaTime());
        MoveAndSlide();
        //Vector2(chargeDirection.X * chargeSpeed * GetProcessDeltaTime(), chargeDirection.Y * chargeSpeed * GetProcessDeltaTime());
        MoveAndSlide();
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
