using Godot;
using System;
[Tool]
[GlobalClass]
public partial class SpawnRect : Node2D
{

	private RectangleShape2D _rectShape;
	[Export]
	public RectangleShape2D RectShape
	{
		get => _rectShape;
		set
		{
			if (_rectShape != null && Engine.IsEditorHint())
			{
				_rectShape.Changed -= RectShapeOnChanged;
			}
			
			_rectShape = value;
			
			if (Engine.IsEditorHint())
			{
				_rectShape.Changed += RectShapeOnChanged;
				QueueRedraw();
			}
		}
	}
	
	

	public override void _Ready()
	{
		if (Engine.IsEditorHint() && RectShape != null)
		{
			RectShape.Changed += RectShapeOnChanged;
		}
		
	}

	public override void _ExitTree()
	{
		if (Engine.IsEditorHint() && RectShape != null)
		{
			RectShape.Changed -= RectShapeOnChanged;
		}
		
	}

	private void RectShapeOnChanged()
	{
		QueueRedraw();
	}


	public override void _Process(double delta)
	{
	}

	public override void _Draw()
	{
		if (Engine.IsEditorHint())
		{
			if (RectShape != null)
			{
				DrawRect(new Rect2(Vector2.Zero, RectShape.Size), new Color(1, 1, 1, 1f));
			}
		}
	}
}
