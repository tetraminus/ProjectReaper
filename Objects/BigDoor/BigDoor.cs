using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.NativeInterop;

namespace ProjectReaper.Objects.BigDoor;

public partial class BigDoor : AnimatableBody2D
{
	private const int Resolution = 25;
	private CollisionPolygon2D CollisionPolygon2D => GetNode<CollisionPolygon2D>("CollisionPolygon2D");
	private Line2D Line2D => GetNode<Line2D>("Line2D");
	private float _percentage = 1;
	private bool _opening = false;
	private bool _closing = false;
	[Export(PropertyHint.NodeType, "Path2D")] public Path2D Curve { get; set; }
	[Export] public float DoorThickness { get; set; } = 10;
	
	// ready 
	public override void _Ready()
	{
		if (!Engine.IsEditorHint()){
			ConstructCollision();
			ConstructVisuals();
			_percentage = 1;
		}
	}
	
	public void Open()
	{
		_opening = true;
		_closing = false;
	}
	public void Close()
	{
		_closing = true;
		_opening = false;
	}
	public bool IsOpen()
	{
		return _percentage <= 0.5;
	}
	public override void _Process(double delta)
	{
		
		if (Engine.IsEditorHint())
		{
			ConstructVisuals();
		}
		else
		{
			if (Input.IsActionJustPressed("interact"))
			{
				if (IsOpen())
				{
					Close();
				}
				else
				{
					Open();
				}
			}

			if (_opening)
			{
				_percentage -= (float)delta;
				if (_percentage < 0)
				{
					_percentage = 0;
					_opening = false;
				}

				ConstructCollision(_percentage);
				ConstructVisuals(_percentage);
			}

			if (_closing)
			{
				_percentage += (float)delta;
				if (_percentage > 1)
				{
					_percentage = 1;
					_closing = false;
				}

				ConstructCollision(_percentage);
				ConstructVisuals(_percentage);
			}
		}
	}

	public void ConstructCollision(float percentage = 1)
	{
	    var starttime = Time.GetTicksMsec();
	    var newPolygon = new List<Vector2>();

	    var totalSteps = (int)(Resolution * percentage);
	    var pixelsperstep = Curve.Curve.GetBakedLength() / Resolution; // Use the original Resolution for the step size
	    var stepsToConstruct = totalSteps; // Calculate the number of steps to construct based on the percentage

	    for (int i = 0; i <= stepsToConstruct; i++)
	    {
	        var transform = Curve.Curve.SampleBakedWithRotation(i * pixelsperstep);
	        var normal = transform.Y.Normalized();

	        newPolygon.Insert(i, transform.Origin + normal.Rotated(Mathf.Pi / 2) * DoorThickness);
	        newPolygon.Insert(newPolygon.Count - i, transform.Origin + normal.Rotated(-Mathf.Pi / 2) * DoorThickness);
	    }
	    
	    CollisionPolygon2D.Polygon = newPolygon.ToArray();
	    
	}
	public void ConstructVisuals(float percentage = 1)
	{
			    
	    var newPoints = new List<Vector2>();

	    var totalSteps = (int)(Resolution * percentage);
	    var pixelsperstep = Curve.Curve.GetBakedLength() / Resolution; // Use the original Resolution for the step size
	    var stepsToConstruct = totalSteps; // Calculate the number of steps to construct based on the percentage

	    for (int i = 0; i <= stepsToConstruct; i++)
	    {
	        var transform = Curve.Curve.SampleBaked(i * pixelsperstep);
	        newPoints.Add(transform);
	        
	    }
	    
	    Line2D.Points = newPoints.ToArray();
	    Line2D.Width = DoorThickness * 2;
	}
}