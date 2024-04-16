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
        if (Duration > 0)
        {
            AddChild(_timer);
            _timer.Timeout += () => QueueFree();
            _timer.Start(Duration);
        }

        Knockback = 150f;
       

        base._Ready();
        
        
    }

    protected override Vector2 GetKnockbackDirection(AbstractCreature creature)
    {
        //knockback in the direction of the bullet travel
        return Transform.X.Normalized();
    }
    
    public void Resprite(Texture2D texture, Vector2 offset = default, float angle = 0f)
    {
        _sprite.Texture = texture;
        _sprite.Offset = offset;
        _sprite.Rotation = angle;
    }
    

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Translate(Transform.X * Speed * (float)delta);
    }
}