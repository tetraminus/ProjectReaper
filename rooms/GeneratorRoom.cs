using System.Linq;
using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public class GeneratorRoom : GodotObject
{
   
    public bool IsCollapsed { get; set; } = false;
    public string[] PossibleRooms { get; set; }
    public Vector2I Position { get; set; }
    
    public GeneratorRoom(string[] possibleRooms, Vector2I position)  {
        PossibleRooms = possibleRooms;
        Position = position;
    }
    
    public int GetEntropy() {
        return PossibleRooms.Length - 1;
    }
    
    

    public object Collapse(RandomNumberGenerator random, GeneratorTile[,] states, RoomSet roomSet)
    {
        // collapse tile to one of the possible Rooms based on weights, and this tile's neighbors
        var neighborRooms = GetNeighborTileIds(states);
        
        var tileWeights = new Dictionary<string, double>();
        // foreach (var tileId in PossibleRooms) {
        //     double weight = 0;
        //     int neighborCount = 0;
        //     foreach (var neighborTile in neighborRooms) {
        //         var neighborWeight = weights.GetWeights(neighborTile);
        //         weight += neighborWeight.GetWeight(tileId);
        //         neighborCount++;
        //     }
        //     if (neighborCount > 0) {
        //         tileWeights[tileId] = weight / neighborCount;
        //     }
        //     else
        //     {
        //         // no neighbors, use self weight
        //         tileWeights[tileId] = weights.GetWeights(tileId).GetSelfWeight();
        //     }
        // }
        
        var chosentileId = GetRandomWeightedTile(random, tileWeights);
        
        // collapse tile
        
        IsCollapsed = true;
        PossibleRooms = new[] {chosentileId};
        
        
        return chosentileId;
    }
    
    private string GetRandomWeightedTile(RandomNumberGenerator random, Dictionary<string, double> tileWeights) {
        var totalWeight = tileWeights.Values.Sum();
        var randomWeight = random.RandfRange(0, (float) totalWeight);
        var currentWeight = 0f;
        foreach (var tileWeight in tileWeights) {
            currentWeight += (float) tileWeight.Value;
            if (currentWeight >= randomWeight) {
                return tileWeight.Key;
            }
        }
        
        return null;
    }
    
    public Array<Vector2> GetNeighborRooms(GeneratorTile[,] states) {
        var neighborRooms = new Array<Vector2>();
        var x = (int)Position.X;
        var y = (int)Position.Y;
        if (x > 0) {
            neighborRooms.Add(new Vector2(x - 1, y));
        }
        
        if (x < states.GetLength(0) - 1) {
            neighborRooms.Add(new Vector2(x + 1, y));
        }
        
        if (y > 0) {
            neighborRooms.Add(new Vector2(x, y - 1));
        }
        
        if (y < states.GetLength(1) - 1) {
            neighborRooms.Add(new Vector2(x, y + 1));
        }
        
        return neighborRooms;
    }

    private Array<string> GetNeighborTileIds(GeneratorTile[,] states)
    {
        var neighborRooms = new Array<string>();
        var x = (int)Position.X;
        var y = (int)Position.Y;
        // if (x > 0)
        // {
        //     neighborRooms.Add(states[x - 1, y].GetCollapsedTile());
        // }
        //
        // if (x < states.GetLength(0) - 1)
        // {
        //     neighborRooms.Add(states[x + 1, y].GetCollapsedTile());
        // }
        //
        // if (y > 0)
        // {
        //     neighborRooms.Add(states[x, y - 1].GetCollapsedTile());
        // }
        //
        // if (y < states.GetLength(1) - 1)
        // {
        //     neighborRooms.Add(states[x, y + 1].GetCollapsedTile());
        // }

        // remove nulls and empty strings
        var nonNullNeighborRooms = new Array<string>();
        nonNullNeighborRooms.AddRange(neighborRooms.Where(tile => tile != null && tile != ""));
    

    return nonNullNeighborRooms;
    }
    
    private string GetCollapsedTile() {
        if (IsCollapsed) {
            return PossibleRooms[0];
        }
        return null;
    }
    
    
    
    
    
   

    
}