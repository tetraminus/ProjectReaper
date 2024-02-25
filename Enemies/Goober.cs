using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Enemies;

public partial class Goober : AbstractCreature
{
    private AnimatedSprite2D sprite;

    public override void _Ready()
    {
        base._Ready();
        Stats.Speed = 1000;
        Stats.Health = 10;
        Stats.MaxHealth = 10;

        sprite = GetNode<AnimatedSprite2D>("Sprite");
        sprite.Play();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        MoveAndSlide();
    }

    public override void _PhysicsProcess(double delta)
    {
        var player = GameManager.Player;
        if (player.Dead) return;
        var dir = player.GlobalPosition - GlobalPosition;

        Velocity += dir.Normalized() * Stats.Speed * (float)delta;
        
        Velocity = Velocity.Lerp(Vector2.Zero, .2f);
       
    }
}