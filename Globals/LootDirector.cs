using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ProjectReaper.Interactables;
using ProjectReaper.Items;

namespace ProjectReaper.Globals;

public partial class LootDirector : Node
{
    public static LootDirector Instance { get; private set; }
    private static PackedScene _chest = GD.Load<PackedScene>("res://Interactables/Chest.tscn");
    
    

    public override void _Ready()
    {
        Instance = this;
    }

    /// <summary>
    ///  Places interactable objects in the level
    /// </summary>
    /// <param name="numberOfChests"> The number of chests to place in the level</param>
    /// <param name="level"> The level to place the interactables in</param>
    public void PlaceInteractables(int numberOfChests, Level level)
    {
        var lootholder = level.LootPoints;
        var _lootPoints = new List<Node2D>();
        if (lootholder != null)
        {
            foreach (Node2D lootPoint in lootholder.GetChildren())
            {
                
                _lootPoints.Add(lootPoint);
            }
        }
        
        
        if (numberOfChests > 0 && level != null) 
        {
            if (numberOfChests > _lootPoints.Count)
            {
                numberOfChests = _lootPoints.Count;
            }
            
            
            
            
            for (var i = 0; i < numberOfChests; i++)
            {
                var lootPoint = _lootPoints[GameManager.LootRng.RandiRange(0, _lootPoints.Count - 1)];
                var chest = _chest.Instantiate<Chest>();
                level.AddChild(chest);
                chest.GlobalPosition = lootPoint.GlobalPosition;
                GD.Print("Chest placed at: " + lootPoint.GlobalPosition);
                _lootPoints.Remove(lootPoint);
            }
            
        }
    }
    
}
