namespace ProjectReaper.Enemies;

public partial class Goober : AbstractCreature
{
    public override void _Ready() {
        Stats.Init();
        Stats.Speed = 10;
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        var player = GameManager.Player;
        var dir = player.GlobalPosition - GlobalPosition;
        
        Velocity = dir.Normalized() * Stats.Speed;

        MoveAndCollide(Velocity * (float) delta);

    }

    public override void OnHit()
    {
        
    }

    public override void OnDeath()
    {
        
    }
}