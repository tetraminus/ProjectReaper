using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectReaper.Components;
using ProjectReaper.Enemies;
using ProjectReaper.Util;

public partial class Goop : Node2D
{
	private Tween _tween;
	private Area2D _area;
	private float time = 1;
	private float _stopRadius = 0;
	private List<AbstractCreature> _creatures = new List<AbstractCreature>();
		
	public void SetTime(float time)
	{
		this.time = time;
	}
	
	public void SetStartRadius(float radius)
	{
		Scale = new Vector2(radius/2.5f, radius/2.5f);
	}
	
	public void SetStopRadius(float radius)
	{
		_stopRadius = radius / 2.5f;
	}

	public override void _Ready()
	{
		if (_tween != null)
		{
			_tween.Kill();
		}
		
		_tween = GetTree().CreateTween();
		
		_tween.TweenProperty(this, "scale", new Vector2(_stopRadius, _stopRadius), time);
		
		_tween.Play();
		
		_area = GetNode<Area2D>("Area2D");
		_area.BodyEntered += body =>
		{
			//GD.Print("Body entered " + body.Name);
			if (body is AbstractCreature creature) _creatures.Remove(creature);
		};
		_area.BodyExited += body =>
		{
			//GD.Print("Body exited " + body.Name);
			if (body is AbstractCreature creature) _creatures.Add(creature);
		};
		
	}
	
	public void DamageTick()
	{
		foreach (var creature in _creatures)
		{
			creature.Damage(new DamageReport(10, creature));
		}
	}
	
	


}