using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Util;
using System;
using ProjectReaper.Enemies;

public partial class Slimebert : AbstractCreature
{
	private EnemyShoot  Shooting = new EnemyShoot();

    public Stats stats = new Stats();

    private enum EnemyState
    {
        Moving,
        Shooting
    }

    private EnemyState currentState = EnemyState.Moving;
    private Timer stateTimer;

	private Timer ShootTimer;

    public override void _Ready()
    {
        stats.Init();

        // Create and add a timer for state switching
        stateTimer = new Timer();
        AddChild(stateTimer);
        stateTimer.WaitTime = 2.0f; // Adjust the time as needed
        stateTimer.OneShot = true;
        stateTimer.Timeout += _OnStateTimerTimeout;
		stateTimer.Start();



		        // Create and add a timer for state switching
        ShootTimer = new Timer();
        AddChild(ShootTimer);
        ShootTimer.WaitTime = 0.5f; // Adjust the time as needed
        ShootTimer.OneShot = true;
        ShootTimer.Timeout += _OnStateTimerTimeout;
		ShootTimer.Start();

		this.AddChild(Shooting);
    }

    public override void OnHit() {
        
    }

    public override void OnDeath() {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        switch (currentState)
        {
            case EnemyState.Moving:
                MoveTowardsPlayer();
                break;
            case EnemyState.Shooting:
                ShootState();
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        // Get the player's position
        Node player = GameManager.Player;
        if (player == null)
        {
            return;
        }
        Vector2 playerPosition = ((Node2D)player).GlobalPosition;

        // Calculate the direction to the player
        Vector2 direction = (playerPosition - GlobalPosition).Normalized();

        // Move the enemy towards the player
        if (Math.Abs((playerPosition - GlobalPosition).Length()) > 100)
        {
            Velocity = direction * stats.Speed;
            MoveAndSlide();
        }
    }

    private void ShootState()
    {
		if (ShootTimer.TimeLeft > 0) return;
        Shooting.Use();
		ShootTimer.Start();
	
    }

    private void _OnStateTimerTimeout()
    {
        // Switch between Moving and Shooting states when the timer expires
        currentState = (currentState == EnemyState.Moving) ? EnemyState.Shooting : EnemyState.Moving;

        // Reset the timer for the next state switch
        stateTimer.Start();
    }
}

