using ProjectReaper.Globals;
using ProjectReaper.Interactables;

namespace ProjectReaper.Items.Collectables;

public partial class TreasureMap : AbstractItem
{
    
    public override string Id => "treasure_map";
    
    public override ItemRarity Rarity => ItemRarity.Legendary;

    public override void OnInitalPickup()
    {
        Callbacks.Instance.PreChestOpened += OnChestOpened;
    }
    
    private void OnChestOpened(Chest chest)
    {
        chest.AddItems(Stacks);
    }
    

    public override void Cleanup()
    {
        Callbacks.Instance.PreChestOpened -= OnChestOpened;
    }
}