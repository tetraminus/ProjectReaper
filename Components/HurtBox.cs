using Godot;
using ProjectReaper.Objects;

namespace ProjectReaper.Enemies;
/// <summary>
/// generic hitbox for enemies and other projectile blockers
/// </summary>
public partial class HurtBox : Area2D
{
    
    [Export] private bool overrideLayer = false;
    public IProjectileBlocker GetParentBlocker()
    {
        return GetParent() as IProjectileBlocker;
    }
    
    public void Disable()
    {
        SetDeferred("monitoring", false);
    }
    
    public void Enable()
    {
        SetDeferred("monitoring", true);
    }
    

    public override void _Ready()
    {
        if (GetParentBlocker() == null)
        {
            GD.PrintErr("HurtBox parent must implement IProjectileBlocker");
        }
        
        if (!overrideLayer)
        {
            CollisionLayer = 0;
            CollisionMask = 0;
            SetCollisionLayerValue(2, true);
            SetCollisionMaskValue(2, true);
        }
    }
}