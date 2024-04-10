using Godot;
using ProjectReaper.Enemies;

namespace ProjectReaper.Abilities.Projectiles;

public partial class BasicBullet : AbstractDamageArea
{
    private Timer _timer = new();
    
    private Sprite2D _sprite => GetNode<Sprite2D>("Sprite2D");

    public override float Speed { get; set; } = 1000f;

    public override float Damage { get; set; } = 10f;

    public override float Duration { get; set; } = 1f;
    
    public override void _Ready()
    {
        AddChild(_timer);
        _timer.Timeout += () => QueueFree();
        _timer.Start(Duration);
        Knockback = 150f;
       

        base._Ready();
        
        
    }

    protected override Vector2 GetKnockbackDirection(AbstractCreature creature)
    {
        //knockback in the direction of the bullet travel
        return Transform.X.Normalized();
    }
    
    public void Resprite(Texture2D texture)
    {
        _sprite.Texture = texture;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Translate(Transform.X * Speed * (float)delta);
    }
}