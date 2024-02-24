using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Enemies;
using ProjectReaper.Player;
using ProjectReaper.Util;
using System;

public partial class Enemy : AbstractCreature 
{ 
	public Stats stats = new Stats();
	public AnimatedSprite2D sprite;
	public override void _Ready()
	{
		sprite = (FindChild("ShadowGuy") as AnimatedSprite2D);
		sprite.Play();
		




		stats.Init();
	}

    public override void _Process(double delta)
    {
		if(Position.X > ProjectReaper.Globals.GameManager.Player.Position.X) {
			sprite.FlipH = true;
		} else {
			sprite.FlipH = false;
		}

     
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void  _PhysicsProcess(double delta)
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
		if (Math.Abs((playerPosition-GlobalPosition).Length()) > 100){
			Velocity = direction * stats.Speed ;
			MoveAndSlide();
		}
		
	

	}

	

}


