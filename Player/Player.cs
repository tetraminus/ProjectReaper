using System.Collections.Generic;
using Godot;
using Godot.Collections;
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
    public int NavGroup { get; set; } = 1;
    private List<string> _keys = new();
        
    


    public override void _Ready()
    {
        base._Ready();
        if (_abilityManager.GetParent() != this) _abilityManager.Reparent(this);
        GameManager.Player = this;
        InitStats();
        Team = Teams.Player;
        LastNavPos = GlobalPosition;
    }


    public override void OnHit()
    {
        
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Callbacks.Instance.PlayerDeathEvent?.Invoke();
        GameManager.GameOver();
    }
    
    public override float AimDirection() {
        return (GetGlobalMousePosition() - GlobalPosition).Angle();
    }


    public void GetInput(float delta )
    {
        var inputDir = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
        // add less speed when moving fast
        
        Velocity += inputDir * Stats.Speed * delta * Accelfac;
        
        MoveDirection = inputDir;
    }

    


    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ability1")) _abilityManager.UseAbility1();
        // if (Input.IsActionPressed("ability2")) _abilityManager.UseAbility2();
        if (Input.IsActionPressed("ability3")) _abilityManager.UseAbility3();
        // if (Input.IsActionPressed("ability4")) _abilityManager.UseAbility4();
        
        
        if ( (LastNavPos - GlobalPosition).Length() > 10)
        {
            LastNavPos = GlobalPosition;
            Callbacks.Instance.EnemyShouldRenavEvent?.Invoke(GlobalPosition, NavGroup);
            NavGroup++;
            NavGroup %= SpawnDirector.MaxNavGroups;
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        GetInput( (float)delta);
        // simulate friction whith delta
        if (Velocity.Length() > Stats.Speed || MoveDirection == Vector2.Zero)
        {
            Velocity = Velocity.Lerp(Vector2.Zero, 0.1f);
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


    public AbstractAbility GetAbility(int slot)
    {
        switch (slot)
        {
            case 1:
                return _abilityManager.Ability1;
            case 2:
                return _abilityManager.Ability2;
            case 3:
                return _abilityManager.Ability3;
            case 4:
                return _abilityManager.Ability4;
            default:
                return null;
        }
        
    }

    public void AddKey(string KeyId) {
        _keys.Add(KeyId);
    }

    public bool HasKey(string KeyId) {
        return _keys.Contains(KeyId);
    }

    public bool UseKey(string KeyId) {
        return _keys.Remove(KeyId);
    }
}
