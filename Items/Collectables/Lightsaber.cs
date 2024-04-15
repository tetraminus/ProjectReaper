using Godot;
using ProjectReaper.Abilities;
using ProjectReaper.Abilities.Projectiles;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Items.Collectables;

public partial class Lightsaber : AbstractItem
{
    public override string Id => "lightsaber";
    public override ItemRarity Rarity => ItemRarity.Uncommon;
    private static readonly PackedScene _fireZoneScene =
        ResourceLoader.Load<PackedScene>("res://Items/Prefabs/FireZone.tscn");
    
    public override void OnInitalPickup()
    {
        Callbacks.Instance.AbilityTrigger += OnAbilityTrigger;
    }
    
    public void OnAbilityTrigger(AbstractAbility ability, AbstractCreature src, int slot, Vector2 position)
    {
        if (src != GetHolder())
        {
            return;
        }
        
        if ((AbilityManager.AbilitySlot) slot == AbilityManager.AbilitySlot.Secondary)
        {
            var fireZone = _fireZoneScene.Instantiate<FireZone>();
            fireZone.Team = GetHolder().Team;
            GameManager.Level.CallDeferred(Node.MethodName.AddChild, fireZone);
            fireZone.GlobalPosition = position;
            fireZone.Source = GetHolder();
            
           
        }
        
    }
    
    public override void Cleanup()
    {
        Callbacks.Instance.AbilityTrigger -= OnAbilityTrigger;
    }

    

    
    
}