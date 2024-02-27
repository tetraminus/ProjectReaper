using Godot;
using System;

public partial class SpawnRectHelper : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Draw()
	{
		if (Engine.IsEditorHint())
		{
			foreach (var child in GetChildren())
			{
				if (child is SpawnRect spawnRect)
				{
					//DrawRect(new Rect2(spawnRect.RectPosition, spawnRect.RectSize), new Color(1, 1, 1, 0.5f));
				}
			}
			
		}
	}
}
