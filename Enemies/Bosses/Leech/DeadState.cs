using Godot;
using ProjectReaper.Components;

namespace ProjectReaper.Enemies.Bosses.Leech;

public partial class DeadState : AbstractState
{
    public override void OnEnter(object[] args)
    {
        var sprite = args[0] as AnimatedSprite2D;
        var leechbert = StateMachine.Creature as Leechbert;
        leechbert.Velocity = Vector2.Zero;
        leechbert.MoveDirection = Vector2.Zero;
        
        sprite.Play("Dead");
        sprite.AnimationFinished += () =>
        {
            leechbert.QuietDie();
        };
    }

    public override void Update(double delta)
    {
        var leechbert = StateMachine.Creature as Leechbert;
        leechbert.Velocity = Vector2.Zero;
        
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}