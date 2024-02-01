using Godot;

using ProjectReaper.Util;
using System;

using ProjectReaper.Abilities;
using System.Collections.Specialized;
using System.Diagnostics;
namespace ProjectReaper.Enemies;


public partial class Snowpeabert : AbstractCreature
{

    public AnimatedSprite2D sprite;

    private SnowPeaShoot Shooting = new SnowPeaShoot();



    private enum EnemyState
    {

        Shooting,
        Burrowing
    }

    private EnemyState currentState = EnemyState.Burrowing;
    private Timer stateTimer;

    private Timer ShootTimer;


    private Random random = new Random();
    private Vector2 randomDirection = Vector2.Zero; // Store the random direction
    ulong lastTimeMs = 0;

    float movementDist = 25.0f;



    private float burrowTime = 2.0f; // Time in seconds for how long the character will burrow

    private CollisionShape2D collisionShape;
    private Timer burrowTimer;



    private void Burrow()
    {
        GD.Print("Burrow");
        currentState = EnemyState.Burrowing;
        Visible = false; // Make the KinematicBody2D invisible
        collisionShape.Disabled = true; // Disable the collisions
        burrowTimer.Start(2); // Start the timer
    }

    private void Unburrow()
    {
        GD.Print("Unburrow");

        currentState = EnemyState.Shooting;
        Visible = true; // Make the KinematicBody2D visible again
        collisionShape.Disabled = false; // Enable the collisions
        burrowTimer.Start(2);

    }

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

        this.AddChild(Shooting);

        sprite = (FindChild("Snowboy") as AnimatedSprite2D);
        sprite.Play();



        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D"); // Replace with your actual CollisionShape2D path


        burrowTimer = new Timer();
        AddChild(burrowTimer);
        burrowTimer.Timeout += _OnStateTimerTimeout;
        burrowTimer.Timeout += () =>
        {
            GD.Print(currentState);
            if (currentState == EnemyState.Burrowing)
            {
                Unburrow();
                burrowTimer.Start(10);
            }
            else
            {
                Burrow();
                burrowTimer.Start(10);


            }

        };







    }
    public override void _Process(double delta)
    {
        if (Position.X > GameManager.Player.Position.X)
        {
            sprite.FlipH = true;
        }
        else
        {
            sprite.FlipH = false;
        }


    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        switch (currentState)
        {
            case EnemyState.Shooting:
                ShootState();
                break;
            case EnemyState.Burrowing:
                MoveRandomly(randomDirection * Stats.Speed);
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


        if (GlobalPosition.DistanceSquaredTo(randomDirection) < 5.0f)
        {
            randomDirection.X = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * movementDist;
            randomDirection.Y = (2.0f - (float)GD.RandRange(0.0f, 1.0f)) * movementDist;
            if ((float)GD.RandRange(0.0f, 1.0f) > 0.5f)
            {
                randomDirection.X *= -1.0f;
            }
            if ((float)GD.RandRange(0.0f, 1.0f) > 0.5f)
            {
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
        Shooting.Use();


    }

    private void _OnStateTimerTimeout()
    {
        // Switch between Moving and Shooting states when the timer expires
        currentState = (currentState == EnemyState.Burrowing) ? EnemyState.Shooting : EnemyState.Burrowing;

        // Reset the timer for the next state switch
        stateTimer.Start();
    }







}