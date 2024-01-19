using Godot;
using System;
using ProjectReaper.Player;
using ProjectReaper.Util;

public partial class Player : CharacterBody2D
{
	[Export(PropertyHint.NodeType)] private AbilityManager _abilityManager;
	
	public Stats Stats { get; set; } = new Stats();

	public override void _Ready()
	{
		if (_abilityManager.GetParent() != this)
		{
			_abilityManager.Reparent(this);
		}
		
		GameManager.Player = this;

		InitStats();

	}

	public void GetInput()
	{
		Vector2 inputDir = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
		Velocity += inputDir * Stats.Speed * (float)GetProcessDeltaTime();
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

	private void InitStats() {
		Stats.Init();
	}
	
	
}
