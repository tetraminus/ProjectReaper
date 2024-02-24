using Godot;

namespace ProjectReaper.Enemies;

public partial class Goober : AbstractCreature
{
    AnimatedSprite2D sprite;

    public override void _Ready() {

        base._Ready();
        Stats.Speed = 50;
        Stats.Health = 10;
        Stats.MaxHealth = 10;

        sprite = GetNode<AnimatedSprite2D>("Sprite");
        sprite.Play();
    }

    public override void _PhysicsProcess(double delta)
    {
        var player = Globals.GameManager.Player;
        var dir = player.GlobalPosition - GlobalPosition;

        Velocity = dir.Normalized() * Stats.Speed;

        MoveAndSlide();

    }

}
