using Godot;
using Godot.Collections;
using ProjectReaper.Interactables;
using ProjectReaper.Items;

namespace ProjectReaper.Globals;

public partial class LootDirector : Node
{
    public static LootDirector Instance { get; private set; }
    private static PackedScene _chest = GD.Load<PackedScene>("res://Interactables/Chest.tscn");
    
    
    Array<LootPoint> _lootPoints = new();
    

    public override void _Ready()
    {
        Instance = this;
    }
    
    /// <summary>
    ///   Adds a loot point to the level, which will be used to spawn a chest or other interactable
    /// </summary>
    /// <param name="lootPoint"></param>
    public void AddLootPoint(Node2D lootPoint)
    {
        if (lootPoint is LootPoint)
        {
            _lootPoints.Add(lootPoint as LootPoint);
        }
    }

    /// <summary>
    ///  Places interactable objects in the level
    /// </summary>
    /// <param name="numberOfChests"> The number of chests to place in the level</param>
    /// <param name="level"> The level to place the interactables in</param>
    public void PlaceInteractables(int numberOfChests, Node2D level)
    {
        if (numberOfChests > 0 && level != null) 
        {
            if (numberOfChests > _lootPoints.Count)
            {
                numberOfChests = _lootPoints.Count;
            }
            
            var random = new RandomNumberGenerator();
            random.Randomize();
            
            for (var i = 0; i < numberOfChests; i++)
            {
                var lootPoint = _lootPoints[random.RandiRange(0, _lootPoints.Count - 1)];
                var chest = _chest.Instantiate<Chest>();
                level.AddChild(chest);
                chest.GlobalPosition = lootPoint.GlobalPosition;
                _lootPoints.Remove(lootPoint);
            }
            
        }
    }
}
