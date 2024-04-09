using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Items.Collectables;

public partial class RocketLauncher : AbstractItem
{
    public override string Id => "rocket_launcher";
    public override ItemRarity Rarity => ItemRarity.Shit;
    
    
    public override void OnInitalPickup()
    {
        Callbacks.Instance.AbilityUsed += PlayerShot;
    }

    public void PlayerShot(AbstractAbility Ability, int slotidx)
    {
        var slot = (AbilityManager.AbilitySlot)slotidx;
        GD.Print(slot);
        if (slot == AbilityManager.AbilitySlot.Secondary)
        {
            GameManager.Player.OnDeath();
        }
        
    }

    public override void Cleanup()
    {
        Callbacks.Instance.AbilityUsed -= PlayerShot;
    }


    public override void OnStack(int newstacks)
    {
        
    }
    
}