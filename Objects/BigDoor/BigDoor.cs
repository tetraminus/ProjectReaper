using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.NativeInterop;

namespace ProjectReaper.Objects.BigDoor;
[Tool]
public partial class BigDoor : AnimatableBody2D
{
	
	private CollisionPolygon2D CollisionPolygon2D => GetNode<CollisionPolygon2D>("CollisionPolygon2D");
	
	private Line2D BGdisplay => GetNode<Line2D>("BGdisplay");
	private Line2D FGdisplay => GetNode<Line2D>("FGdisplay");
	private float _percentage = 1;
	private bool _opening = false;
	private bool _closing = false;
	[Export(PropertyHint.NodeType, "Path2D")] public Path2D Curve { get; set; }
	[Export(PropertyHint.NodeType, "NavigationRegion2D")] public NavigationRegion2D NavigationRegion2D { get; set; }
	[Export] public float DoorThickness { get; set; } = 10;
	[Export] public Color DoorColor { get; set; } = new Color(1, 1, 1, 1);
	
	[Export(PropertyHint.Range, "1,100")] 
	private int Resolution = 25;
	
	// ready 
	public override void _Ready()
	{
		if (!Engine.IsEditorHint()){
			ConstructCollision();
			ConstructVisuals();
			_percentage = 1;
		}
		FGdisplay.Modulate = DoorColor;
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
		NavigationRegion2D.Enabled = false;
		
	}
	public bool IsOpen()
	{
		return _percentage <= 0.5;
	}
	public override void _Process(double delta)
	{
		
		if (Engine.IsEditorHint() )
		{
			if (Curve != null)
			{
				ConstructVisuals();
				FGdisplay.Modulate = DoorColor;
			}
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
					NavigationRegion2D.Enabled = true;
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
	    
	    BGdisplay.Points = newPoints.ToArray();
	    BGdisplay.Width = DoorThickness * 2;
	    FGdisplay.Points = newPoints.ToArray();
	    FGdisplay.Width = DoorThickness * 2;
	}

	
}