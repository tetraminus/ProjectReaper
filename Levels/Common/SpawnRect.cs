using Godot;
using System;
using ProjectReaper.Globals;

[Tool]
[GlobalClass]
public partial class SpawnRect : Node2D
{

	private bool scaling = false;
	[Export] public RectangleShape2D RectShape;

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			if (RectShape != null)
			{
				QueueRedraw();

				// Gizmos for scaling

				if (Input.IsMouseButtonPressed(MouseButton.Left) &&
				    GetGlobalMousePosition().DistanceTo(GlobalPosition + RectShape.Size) < 10)
				{

					scaling = true;
				}

				if (!Input.IsMouseButtonPressed(MouseButton.Left) && scaling)
				{
					scaling = false;
				}

				if (scaling)
				{
					RectShape.Size = GetGlobalMousePosition() - GlobalPosition;
				}




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
				DrawCircle(RectShape.Size, 10, Colors.Red);
			}
		}
	}



	public bool PlayerTooClose(float distance)
	{
		var playerPos = GameManager.Player.Position;
		return playerPos.DistanceTo(Position + RectShape.Size / 2) < distance;
	}


	public Vector2 GetRandomPosition()
	{
		return Position + new Vector2(GD.Randf() * RectShape.Size.X, GD.Randf() * RectShape.Size.Y);
	}

	public Vector2 ClosestPoint(Vector2 point, float margin = 0.0f)
	{
		var x = Mathf.Clamp(point.X, Position.X + margin, Position.X + RectShape.Size.X - margin);
		var y = Mathf.Clamp(point.Y, Position.Y + margin, Position.Y + RectShape.Size.Y - margin);
		return new Vector2(x, y);
	}
}
