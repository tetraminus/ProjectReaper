using Godot;
using System;
using ProjectReaper.Player;

public partial class Player : CharacterBody2D
{
	private int _speed = 100;
	[Export(PropertyHint.NodeType)] private AbilityManager _abilityManager;

	public override void _Ready()
	{
		if (_abilityManager.GetParent() != this)
		{
			_abilityManager.Reparent(this);
		}
		
		GameManager.Player = this;
		
	}

	public void GetInput()
	{
		Vector2 inputDir = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
		Velocity += inputDir * _speed;
	}

	

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ability1"))
		{
			_abilityManager.UseAbility1();
		}
		if (Input.IsActionPressed("ability2"))
		{
			_abilityManager.UseAbility2();
		}
		if (Input.IsActionPressed("ability3"))
		{
			_abilityManager.UseAbility3();
		}
		if (Input.IsActionPressed("ability4"))
		{
			_abilityManager.UseAbility4();
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		Velocity = Velocity.Lerp(Vector2.Zero, 0.2f);
		MoveAndSlide();
	}
}
