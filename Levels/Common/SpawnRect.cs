using Godot;
using System;
using ProjectReaper.Globals;

[Tool]
[GlobalClass]
public partial class SpawnRect : Node2D {


	[Export] public RectangleShape2D RectShape;
	
	



	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			if (RectShape != null)
			{
				QueueRedraw();
			}
		}
		
	}
	
	public float GetArea()
	{
		return RectShape.Size.X * RectShape.Size.X;
	}

	public override void _Draw()
	{
		if (Engine.IsEditorHint())
		{
			if (RectShape != null)
			{
				DrawRect(new Rect2(Vector2.Zero, RectShape.Size), Colors.Purple, false, 4);
			}
		}
	}
	
	public bool PlayerTooClose(float distance)
	{
		var playerPos = GameManager.Player.Position;
		return playerPos.DistanceTo(Position + RectShape.Size / 2) < distance;
	}
	

	public Vector2 GetRandomPosition() {
		return Position + new Vector2(GD.Randf() * RectShape.Size.X, GD.Randf() * RectShape.Size.Y);
	}
}
