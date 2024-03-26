using Godot;
using System;
using System.Collections.Generic;
using ProjectReaper.Globals;

public partial class VortexEffect : Node2D
{


	private List<Sprite2D> _sprites = new List<Sprite2D>();
	[Export] public int SpriteCount = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Create sprites
		for (int i = 0; i < SpriteCount; i++)
		{
			var sprite = new Sprite2D();
			var item = ItemLibrary.Instance.RollTrulyRandomItem();
			sprite.Texture = item.Icon;
			sprite.Position = new Vector2(0, 0);
			sprite.Scale = new Vector2(2f, 2f);
			sprite.Visible = false;
			AddChild(sprite);
			_sprites.Add(sprite);
			InitialSpawn(sprite);
		}
		
		
		
	}
	
	/// <summary>
	/// spawns a sprite at a random position in the viewport
	/// </summary>
	/// <param name="sprite"></param>
	public void InitialSpawn(Sprite2D sprite)
	{
		
		sprite.Position = new Vector2(GD.Randf() * GetViewportRect().Size.X, GD.Randf() * GetViewportRect().Size.Y);
		
		sprite.Rotation = GD.Randf() * Mathf.Pi * 2;
		sprite.Visible = true;
	}
	
	
	/// <summary>
	/// respawns a sprite 20 px outside the edge of the screen
	/// </summary>
	/// <param name="sprite"></param>
	public void RespawnSprite(Sprite2D sprite)
	{
		
		var center = GetViewportRect().Size / 2;
		var direction = new Vector2(GD.Randf() * 2 - 1, GD.Randf() * 2 - 1).Normalized();

		sprite.Position = center + direction * GetViewportRect().Size.Length() / 2;
		sprite.Scale = new Vector2(2, 2);
		sprite.Rotation = GD.Randf() * Mathf.Pi * 2;
		sprite.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Move sprites towards the center
		Vector2 center = GetViewportRect().Size / 2;
		foreach (var sprite in _sprites)
		{
			
			sprite.Rotation += 1 * (float)delta;
			// Move sprite towards center
			var direction = (center - sprite.Position).Normalized();
			sprite.Position += direction * 200 * (float)delta;
			// scale sprite down as it gets closer to the center, from 2 to 0
			
			if (sprite.Position.DistanceTo(center) < 100)
			{
				sprite.Scale = new Vector2(2 * (sprite.Position.DistanceTo(center) / 100), 2 * (sprite.Position.DistanceTo(center) / 100));
			}
			
			// Respawn sprite if it gets too close to the center
			if (sprite.Position.DistanceTo(center) < 10)
			{
				RespawnSprite(sprite);
			}
		}
		
		
		
	}
}
