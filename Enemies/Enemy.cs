using System;
using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

public partial class Enemy : AbstractCreature
{
    public AnimatedSprite2D sprite;
    public Stats stats = new();

    public override void _Ready()
    {
        sprite = FindChild("ShadowGuy") as AnimatedSprite2D;
        sprite.Play();


        stats.Init();
    }

    public override void _Process(double delta)
    {
        if (Position.X > GameManager.Player.Position.X)
            sprite.FlipH = true;
        else
            sprite.FlipH = false;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        // Get the player's position 
        Node player = GameManager.Player;
        if (player == null) return;
        var playerPosition = ((Node2D)player).GlobalPosition;

        // Calculate the direction to the player
        var direction = (playerPosition - GlobalPosition).Normalized();

        // Move the enemy towards the player
        if (Math.Abs((playerPosition - GlobalPosition).Length()) > 100)
        {
            Velocity = direction * stats.Speed;
            MoveAndSlide();
        }
    }
}