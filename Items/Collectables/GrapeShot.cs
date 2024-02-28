using System;
using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Vfx;

namespace ProjectReaper.Items;

public partial class GrapeShot : AbstractItem
{
    
    private int _count = 0; 
    private const float Spread = 0.2f;
    public override string Id => "grapeshot";
    
    private PackedScene _bulletScene = GD.Load<PackedScene>("res://Items/Prefabs/GrapeBullet.tscn");
    public override void OnInitalPickup()
    {
        Callbacks.Instance.AbilityUsedEvent += OnAbilityUsed;
    }
    
    public void OnAbilityUsed(AbstractAbility ability, int slot)
    {
        _count++;
        if (slot == 1 && _count >= 3)
        {
            FireGrapeshot();
            _count = 0;
        }
    }

    private void FireGrapeshot()
    {
        // fire 3 bullets in a spread
        for (var i = 0; i < 3; i++)
        {
            var bullet = _bulletScene.Instantiate<BasicBullet>();
            bullet.Damage = 10;
            bullet.Speed = 500;
            var angle = GlobalRotation + (i - 1) * Spread;
            bullet.GlobalRotation = GameManager.Player.ShootDirection + angle;
            bullet.Position = GetHolder().GlobalPosition;
            bullet.Team = GetHolder().Team;
            GameManager.Level.AddChild(bullet);
        }
        
        
        
    }
}