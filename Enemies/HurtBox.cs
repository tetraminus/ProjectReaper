using Godot;
using ProjectReaper.Objects;

namespace ProjectReaper.Enemies;
/// <summary>
/// generic hitbox for enemies and other projectile blockers
/// </summary>
public partial class HurtBox : Area2D
{
    public IProjectileBlocker GetParentBlocker()
    {
        return GetParent() as IProjectileBlocker;
    }
    

    public override void _Ready()
    {
        if (GetParentBlocker() == null)
        {
            GD.PrintErr("HurtBox parent must implement IProjectileBlocker");
        }
    }
}