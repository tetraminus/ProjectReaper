using ProjectReaper.Globals;
using ProjectReaper.Interactables;

namespace ProjectReaper.Items.Collectables;

public partial class TreasureMap : AbstractItem
{
    
    public override string Id => "treasure_map";
    
    public override ItemRarity Rarity => ItemRarity.Legendary;

    public override void OnInitalPickup()
    {
        Callbacks.Instance.LevelLoaded += OnLevelLoaded;
        
        var interactables = GameManager.Level.GetInteractables();
        foreach (var interactable in interactables)
        {
            if (interactable is Chest chest)
            {
                chest.AddItems(Stacks);
            }
        }
    }
    
    private void OnLevelLoaded()
    {
        var interactables = GameManager.Level.GetInteractables();
        foreach (var interactable in interactables)
        {
            if (interactable is Chest chest)
            {
                chest.AddItems(Stacks);
            }
        }
    }
    
    public override void OnStack(int stacks)
    {
        var interactables = GameManager.Level.GetInteractables();
        foreach (var interactable in interactables)
        {
            if (interactable is Chest chest)
            {
                chest.AddItems(stacks);
            }
        }
    }

    public override void Cleanup()
    {
        Callbacks.Instance.LevelLoaded -= OnLevelLoaded;
    }
}