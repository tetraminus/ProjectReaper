using Godot;
using Godot.Collections;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Player;

public partial class Player : AbstractCreature
{
    [Export(PropertyHint.NodeType)] private AbilityManager _abilityManager;
    public Vector2 MoveDirection { get; set; }
    public Camera2D Camera  => GetNode<Camera2D>("Camera2D");
    
    public Vector2 LastNavPos { get; private set; }
    public int NavGroup { get; set; } = 1;
    

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


    public void GetInput(float delta )
    {
        var inputDir = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
        // add less speed when moving fast
        
       
        
        
        Velocity += inputDir * Stats.Speed * delta;
        MoveDirection = inputDir;
    }


    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ability1")) _abilityManager.UseAbility1();
        // if (Input.IsActionPressed("ability2")) _abilityManager.UseAbility2();
        if (Input.IsActionPressed("ability3")) _abilityManager.UseAbility3();
        // if (Input.IsActionPressed("ability4")) _abilityManager.UseAbility4();
        MoveAndSlide();
        
        if ( (LastNavPos - GlobalPosition).Length() > 25)
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
        Velocity = Velocity.Lerp(Vector2.Zero, 8f * (float)delta);
    }

    private void InitStats()
    {
        Stats.Init();
        Stats.Speed = 2000;
    }

    public override void AddItem(string id, int stacks = 1, bool _ = false)
    {
        base.AddItem(id, stacks, true);
    }
    
    public override void AddItem(AbstractItem item)
    {
        base.AddItem(item);
    }
    
    
}
