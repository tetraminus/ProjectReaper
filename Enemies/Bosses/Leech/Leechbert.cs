using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies.Bosses;

public partial class Leechbert : AbstractCreature
{
    private AnimatedSprite2D _sprite;
    private StateMachineComponent _stateMachine;

    public const float Accelfac = 20.0f;
    public Vector2 MoveDirection { get; set; }


    public override void OnDeath()
    {
        Callbacks.Instance.EmitSignal(Callbacks.SignalName.CreatureDied, this);
        Dead = true;
        _stateMachine.ChangeState("DeadState", _sprite);
    }

    public override void _Ready()
    {
        base._Ready();
        Stats.Speed = 80;
        
        Stats.MaxHealth = 10;
        Stats.Health = Stats.MaxHealth;

        _sprite = GetNode<AnimatedSprite2D>("Sprite");
        _sprite.Play();
        
        _stateMachine = GetNode<StateMachineComponent>("StateMachineComponent");
        _stateMachine.Creature = this;
        
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        _stateMachine.Process(delta);
        MoveAndSlide();
        
        if (MoveDirection != Vector2.Zero)
        {
            _sprite.FlipH = MoveDirection.X > 0;
        }
    }
    
    

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _stateMachine.PhysicsProcess(delta);
    }
}