using Godot;
using ProjectReaper.Abilities;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	private float speed = 100.0f;

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void  _PhysicsProcess(double delta)
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
		if (Math.Abs((playerPosition-GlobalPosition).Length()) > 100){
            Velocity = direction * speed;
		    MoveAndSlide();
        }
           
	}

	

}


