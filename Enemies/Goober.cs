namespace ProjectReaper.Enemies;

public partial class Goober : AbstractCreature
{
    public override void _Ready() {

        base._Ready();
        Stats.Speed = 10;
        Stats.Health = 10;
        Stats.MaxHealth = 10;
        
        
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