using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Items.Collectables;

public partial class RogueNanotech : AbstractItem
{
    
    public override string Id => "rogue_nanotech";
    public override ItemRarity Rarity => ItemRarity.Uncommon;

    private Dictionary<AbstractItem, int> Boosts = new();
    private Timer _timer;

    public override void OnInitalPickup()
    {
        _timer = new Timer
        {
            WaitTime = 10,
            Autostart = true,
            OneShot = false
        };
        AddChild(_timer);
        _timer.Timeout += ReboostItems;
        ReboostItems();
    }

    public override void OnStack(int newstacks)
    {
        ReboostItems();
    }

    private void ReboostItems()
    {
        ClearBoosts();
        var playerItems = GameManager.Player.GetItems();
        if (playerItems.Where(item => item.Id != Id).ToList().Count == 0)
        {
            return;
        }
        for (var i = 0; i < Stacks; i++)
        {
            AbstractItem item;
            do 
            {
                var roll = GD.RandRange(0, playerItems.Count - 1);
                item = playerItems[roll];
            } while (item.Id == Id);
            
            if (Boosts.ContainsKey(item))
            {
                Boosts[item]++;
            }
            else
            {
                Boosts.Add(item, 1);
            }
        }
        GD.Print("Boosting items" + Boosts);
        foreach (var boost in Boosts)
        {
            var item = boost.Key;
            var stacks = boost.Value;
            item.MimicStacks += stacks;
        }
    }
    
    
    private void ClearBoosts()
    {
        foreach (var boost in Boosts)
        {
            var item = boost.Key;
            var stacks = boost.Value;
            item.MimicStacks -= stacks;
        }
        Boosts.Clear();
    }

    public override void Cleanup()
    {
        ClearBoosts();
        _timer.Timeout -= ReboostItems;
        
        base.Cleanup();
    }
}