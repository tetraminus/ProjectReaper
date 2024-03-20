using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class RevengeBomber : AbstractItem
{
    public override string Id => "revenge_bomber";
    public override ItemRarity Rarity => ItemRarity.Common;
    
    public override void OnInitalPickup()
    {
        Callbacks.Instance.CreatureDamaged += OnCreatureDamaged;
    } 
    public override void Cleanup()
    {
        Callbacks.Instance.CreatureDamaged -= OnCreatureDamaged;
    }
    
    public void OnCreatureDamaged(AbstractCreature creature, float damage)
    {
        if (creature.IsPlayer() && damage > 0)
        {
            GameManager.SpawnExplosion(creature.GlobalPosition, 10 * Stacks, 1.5f, creature);
        }
    }
}