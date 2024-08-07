using System;
using System.Collections.Generic;
using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Player;

public partial class Player : AbstractCreature
{
	
	private AnimatedSprite2D _sprite;
	private const float Accelfac = 20.0f;
	[Export(PropertyHint.NodeType)] private AbilityManager _abilityManager;
	public Vector2 MoveDirection { get; set; }
	public Vector2 LastNavPos { get; private set; }
	public int CurrentNavGroup { get; set; } = 1;
	private Dictionary<string, int> _inventory = new();
	
	private bool _controllerMode = false;
	private float _lastAim = 0;


	public override void _Input(InputEvent @event)
	{
		if ((EventIsAim(@event)) && !_controllerMode )
		{
			_controllerMode = true;
			GD.Print("Controller mode");
		}
		else if ((@event is InputEventMouseButton || @event is InputEventMouseMotion) && _controllerMode)
		{
			_controllerMode = false;
			GD.Print("Mouse mode");
		}
		
		base._Input(@event);
	}
	
	public bool EventIsAim(InputEvent @event)
	{
		return @event.IsActionPressed("Aim_Left") || @event.IsActionPressed("Aim_Right") || @event.IsActionPressed("Aim_Up") || @event.IsActionPressed("Aim_Down");
	}
	
	


	public override void _Ready()
	{

		
		base._Ready();
		if (_abilityManager.GetParent() != this) _abilityManager.Reparent(this);
		GameManager.Player = this;
		InitStats();
		Team = Teams.Player;
		LastNavPos = GlobalPosition;
		Dead = false;
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
	}

	public override void _ExitTree()
	{
		if (IsQueuedForDeletion())
		{
			
		}
	}


	public override void OnHit()
	{
		if (HitState != HitBoxState.Normal) return;
		HitState = HitBoxState.Invincible;
		GetTree().CreateTimer(0.25f).Timeout += () => { HitState = HitBoxState.Normal; };
		
	}

	public override void OnDeath()
	{
		Dead = true;
		Velocity = Vector2.Zero;
		MoveDirection = Vector2.Zero;
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.CreatureDied, this);
		Callbacks.Instance.EmitSignal(Callbacks.SignalName.PlayerDeath);
		GameManager.GameOver();
	}
	
	public override float AimDirection() {
		if (_controllerMode)
		{
			return _lastAim;
		}
		else
		{
			return GlobalPosition.DirectionTo(GetGlobalMousePosition()).Angle();
		}
		
	}


	public void GetInput(float delta )
	{
		if (Dead)
		{
			MoveDirection = Vector2.Zero;
			return;
		}
		
		var inputDir = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
		
		Velocity += inputDir * Stats.Speed * delta * Accelfac;
		
		MoveDirection = inputDir;
	}
	
	


	public override void _Process(double delta)
	{
		
		if (Dead) return;

		var StopMove = false;
		var aimDir = AimDirection();
		var aimVec = Vector2.FromAngle(aimDir).Normalized();
		
		if (aimVec.Dot(Vector2.Up) > 0.5)
		{
			_sprite.Play("MoveUp");
		}
		else if (aimVec.Dot(Vector2.Down) > 0.5)
		{
			_sprite.Play("MoveDown");
		}
		else if (aimVec.Dot(Vector2.Right) > 0.5)
		{
			_sprite.Play("MoveRight");
		}
		else if (aimVec.Dot(Vector2.Left) > 0.5)
		{
			_sprite.Play("MoveLeft");
		}
		
		
		if (MoveDirection == Vector2.Zero)
		{
			StopMove = true;

			if ( aimVec.Dot(Vector2.Up) > 0.5)
			{
				_sprite.Play("IdleUp");
			}
			else if (aimVec.Dot(Vector2.Down) > 0.5)
			{
				_sprite.Play("IdleDown");
			}
			else if (aimVec.Dot(Vector2.Right) > 0.5)
			{
				_sprite.Play("IdleRight");
			}
			else if (aimVec.Dot(Vector2.Left) > 0.5)
			{
				_sprite.Play("IdleLeft");
			}
			{
				
			}
		}
		
		
		
		if (_controllerMode)
		{
			var inputDir = Input.GetVector("Aim_Left", "Aim_Right", "Aim_Up", "Aim_Down");
			if (inputDir.Length() > 0.1)
			{
				_lastAim = inputDir.Angle();
			}
		}
		
		
		
		if (Input.IsActionPressed("ability1")) _abilityManager.UseAbility(0);
		if (Input.IsActionPressed("ability2")) _abilityManager.UseAbility(1);
		if (Input.IsActionPressed("ability3")) _abilityManager.UseAbility(2);
		if (Input.IsActionPressed("ability4")) _abilityManager.UseAbility(3);
		
		if (false)//Input.IsActionJustPressed("cheat"))
		{
			GameManager.GoToLibrary(true);
		}
		
		if ((LastNavPos - GlobalPosition).Length() > 20)
		{
			LastNavPos = GlobalPosition;
			Callbacks.Instance.EmitSignal(Callbacks.SignalName.EnemyRenav,GlobalPosition, CurrentNavGroup);
			//GD.Print("Renaving " + NavGroup);
			CurrentNavGroup++;
			CurrentNavGroup %= SpawnDirector.MaxNavGroups;
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		if (!Dead)
		{
			GetInput((float)delta);
			// simulate friction whith delta
			if (Velocity.Length() > Stats.Speed || MoveDirection == Vector2.Zero)
			{
				Velocity = Velocity.Lerp(Vector2.Zero, 0.1f);
			}
		}

		MoveAndSlide();
		
	}

	private void InitStats()
	{
		Stats.Init();
		Stats.Speed = 100;
	}

	public override void AddItem(string id, int stacks = 1, bool _ = false)
	{
		base.AddItem(id, stacks, true);
	}
	
	public override void AddItem(AbstractItem item)
	{
		base.AddItem(item);
	}


   
	public AbstractAbility GetAbility(AbilityManager.AbilitySlot slot)
	{
		return _abilityManager.GetAbility(slot);
	}

	public void AddKey(string KeyId, int num = 1) {
		if (_inventory.ContainsKey(KeyId))
		{
			_inventory[KeyId] += num;
		}
		else
		{
			_inventory.Add(KeyId, num);
		}
		GameManager.PlayerHud.UpdateKeyInventory(_inventory);
		
		
	}
	
	public bool HasKey(string KeyId, int num = 1)
	{
		return _inventory.ContainsKey(KeyId) && _inventory[KeyId] >= num;
	}
	
	public bool UseKey(string KeyId, int num = 1)
	{
		if (_inventory.ContainsKey(KeyId))
		{
			if (_inventory[KeyId] >= num)
			{
				_inventory[KeyId] -= num;
				if (_inventory[KeyId] <= 0)
				{
					_inventory.Remove(KeyId);
				}
				GameManager.PlayerHud.UpdateKeyInventory(_inventory);
				return true;
			}
		}
		
		return false;
	}
	
	public int GetKeyCount(string KeyId)
	{
		return _inventory.ContainsKey(KeyId) ? _inventory[KeyId] : 0;
	}


	public List<AbstractItem> GetItems()
	{
		var items = new List<AbstractItem>();
		foreach (var item in Items.GetChildren())
		{
			if (item is AbstractItem abstractItem)
			{
				items.Add(abstractItem);
			}
		}

		return items;
		
	}
}
