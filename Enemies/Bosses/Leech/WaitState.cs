using Godot;
using ProjectReaper.Components;
using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Enemies.Bosses.Leech;

public partial class WaitState : AbstractState
{
    [Export] public Node NextState { get; set; }

    public override void OnEnter(object[] args)
    {
        GameManager.PlayerHud.Connect(PlayerHud.SignalName.FightAnimFinished, Callable.From(OnFightAnimFinished),
            (uint)ConnectFlags.OneShot);
        var leechbert = StateMachine.Creature as Leechbert;
        leechbert.Velocity = Vector2.Zero;
        leechbert.MoveDirection = Vector2.Zero;
        leechbert.HitState = AbstractCreature.HitBoxState.Invincible;
    }

    public override void Update(double delta)
    {
        var leechbert = StateMachine.Creature as Leechbert;
        leechbert.Velocity = Vector2.Zero;
    }

    public void OnFightAnimFinished()
    {
        StateMachine.ChangeState(NextState.Name);
    }

    public override void OnExit()
    {
        StateMachine.Creature.HitState = AbstractCreature.HitBoxState.Normal;

        base.OnExit();
    }
}