using System.Collections.Generic;
using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Player;

public partial class Player : AbstractCreature
{
    
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
        if ((@event is InputEventJoypadMotion  || @event is InputEventJoypadButton) && !_controllerMode )
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


    public override void _Ready()
    {
        base._Ready();
        if (_abilityManager.GetParent() != this) _abilityManager.Reparent(this);
        GameManager.Player = this;
        InitStats();
        Team = Teams.Player;
        LastNavPos = GlobalPosition;
        Dead = false;
    }

    public override void _ExitTree()
    {
        if (IsQueuedForDeletion())
        {
            
        }
    }


    public override void OnHit()
    {
        
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
        // add less speed when moving fast
        
        Velocity += inputDir * Stats.Speed * delta * Accelfac;
        
        MoveDirection = inputDir;
    }

    


    public override void _Process(double delta)
    {
        
        if (Dead) return;
        
        if (_controllerMode)
        {
            var inputDir = Input.GetVector("Aim_Left", "Aim_Right", "Aim_Up", "Aim_Down");
            if (inputDir.Length() > 0.1)
            {
                _lastAim = inputDir.Angle();
            }
        }
        
        
        
        if (Input.IsActionPressed("ability1")) _abilityManager.UseAbility(0);
        // if (Input.IsActionPressed("ability2")) _abilityManager.UseAbility(1);
        if (Input.IsActionPressed("ability3")) _abilityManager.UseAbility(2);
        // if (Input.IsActionPressed("ability4")) _abilityManager.UseAbility(3);
        
        
        if ( (LastNavPos - GlobalPosition).Length() > 5)
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

   
    
}
