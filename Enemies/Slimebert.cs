using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Util;
using System;

using ProjectReaper.Enemies;
using System.Collections.Specialized;
using System.Diagnostics;



public partial class Slimebert : AbstractCreature
{

    public AnimatedSprite2D sprite;

	private SlimeBertShoot  Shooting = new SlimeBertShoot();



    private enum EnemyState
    {
        Moving,
        Shooting
    }

    private EnemyState currentState = EnemyState.Moving;
    private Timer stateTimer;

	private Timer ShootTimer;


    private Random random = new Random();
    private Vector2 randomDirection = Vector2.Zero; // Store the random direction
    ulong lastTimeMs = 0;

    float movementDist = 25.0f;




    

    public override void _Ready()
    {
        Stats.Init();
        Stats.Health = 20;

        // Create and add a timer for state switching
        stateTimer = new Timer();
        AddChild(stateTimer);
        stateTimer.WaitTime = 2.0f; // Adjust the time as needed
        stateTimer.OneShot = true;
        stateTimer.Timeout += _OnStateTimerTimeout;
		stateTimer.Start();


        randomDirection.X = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * movementDist;
        randomDirection.Y = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * movementDist;
        randomDirection.X += GlobalPosition.X;
        randomDirection.Y += GlobalPosition.Y;
        


		        // Create and add a timer for state switching
        ShootTimer = new Timer();
        AddChild(ShootTimer);
        ShootTimer.WaitTime = 0.5f; // Adjust the time as needed
        ShootTimer.OneShot = true;
        ShootTimer.Timeout += _OnStateTimerTimeout;
		ShootTimer.Start();

		this.AddChild(Shooting);

            //Animated sprite 


        

        sprite = (FindChild("Slimeboy") as AnimatedSprite2D);
		sprite.Play();
	
		
    }
     public override void _Process(double delta)
    {
		if(Position.X > ProjectReaper.Globals.GameManager.Player.Position.X) {
			sprite.FlipH = false;
		} else {
			sprite.FlipH = true;
		}

     
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        switch (currentState)
        {
            case EnemyState.Moving:
                MoveRandomly(randomDirection * Stats.Speed);
                break;
            case EnemyState.Shooting:
                ShootState();
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        // Get the player's position
        Node player = ProjectReaper.Globals.GameManager.Player;
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
            Velocity = direction * Stats.Speed;
            MoveAndSlide();
        }
    }
    
    
    private void MoveRandomly(Vector2 vector2)
    {
        // Generate a random direction
        //Vector2 randomDirection = new Vector2((float)GD.Randf() * 2 - 1, (float)GD.Randf() * 2 - 1).Normalized();
        ulong currentTimeMs = Time.GetTicksMsec();       

        //if ((currentTimeMs - lastTimeMs) > 20.0) {
        

        if (GlobalPosition.DistanceSquaredTo(randomDirection) < 5.0f) {
            randomDirection.X = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * movementDist;
            randomDirection.Y = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * movementDist;
            if ((float)GD.RandRange(0.0f, 1.0f) > 0.5f) {
                randomDirection.X *= -1.0f;
            }
            if ((float)GD.RandRange(0.0f, 1.0f) > 0.5f) {
                randomDirection.Y *= -1.0f;
            }
            randomDirection.X += GlobalPosition.X;
            randomDirection.Y += GlobalPosition.Y;
        }

        GlobalPosition = GlobalPosition.Lerp(randomDirection, 0.05f);
        

        // Move the enemy in the random direction
        //Velocity = randomDirection * stats.Speed;
        //MoveAndSlide();
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

