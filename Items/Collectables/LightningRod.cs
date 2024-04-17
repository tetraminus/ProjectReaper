using System.Collections.Generic;
using Godot;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Util;

namespace ProjectReaper.Items.Collectables;

public partial class LightningRod : AbstractItem
{
    public override string Id => "lightning_rod";
    public override ItemRarity Rarity => ItemRarity.Common;
    
    private const float Chance = 0.25f;
    private static Texture2D lightningTexture = GD.Load<Texture2D>("res://Assets/Abilities/zapbullet.png");
    
    public override void OnInitalPickup()
    {
        Callbacks.Instance.BulletCreated += BulletCreated;
        Callbacks.Instance.ProjectileHit += ProjectileHit;
        
    }
    
    public override void Cleanup()
    {
        Callbacks.Instance.BulletCreated -= BulletCreated;
        Callbacks.Instance.ProjectileHit -= ProjectileHit;
    }
    
    public void BulletCreated(AbstractDamageArea bullet)
    {
        if (bullet is BasicBullet basicBullet && GameManager.RollBool(Chance, GameManager.Player.Stats.Luck) && bullet.Source == GetHolder())
        {
            basicBullet.Resprite(lightningTexture);
            var chainInfo = new Dictionary<string, Variant>
            {
                {"Chain", new Variant()}
            };
            basicBullet.Flags.Add("Chain", chainInfo);
            basicBullet.DestroyOnHit = false;
        }
    }
    
    public void ProjectileHit(AbstractDamageArea bullet, AbstractCreature target)
    {
        if (bullet is BasicBullet basicBullet && bullet.Flags.ContainsKey("Chain"))
        {
            
            if (bullet.Flags.ContainsKey("Chain"))
            {
                
                bullet.Damage /= 2;
                bullet.ProcCoef *= 0.8f;
                var nextEnemy = GameManager.GetClosestEnemy(target.GlobalPosition, 500,new List<AbstractCreature> {target}, GetHolder().Team);
                if (nextEnemy != null)
                {
                    var newAngle = (nextEnemy.GlobalPosition - target.GlobalPosition).Normalized();
                    bullet.Rotation = newAngle.Angle();
                    bullet.ExclusionList.Add(target);
                }
                
                if (bullet.Damage <= 1)
                {
                    bullet.DestroyOnHit = true;
                    bullet.Flags.Remove("Chain");
                }
                
            }
            

        }
    }

    
}