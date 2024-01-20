using System.Linq;
using Godot;
using Godot.Collections;


namespace ProjectReaper.Util;

public partial class GeneratorTile: GodotObject
{
    public bool IsCollapsed { get; set; } = false;
    public string[] PossibleTiles { get; set; }
    
    public Vector2I Position { get; set; }
    
    public GeneratorTile(string[] possibleTiles, Vector2I position)  {
        PossibleTiles = possibleTiles;
        Position = position;
    }
    
    public int GetEntropy() {
        return PossibleTiles.Length - 1;
    }
    
    

    public object Collapse(RandomNumberGenerator random, GeneratorTile[,] states, TilesetWeights weights)
    {
        // collapse tile to one of the possible tiles based on weights, and this tile's neighbors
        var neighborTiles = GetNeighborTileIds(states);
        
        var tileWeights = new Dictionary<string, double>();
        foreach (var tileId in PossibleTiles) {
            double weight = 0;
            int neighborCount = 0;
            foreach (var neighborTile in neighborTiles) {
                var neighborWeight = weights.GetWeights(neighborTile);
                weight += neighborWeight.GetWeight(tileId);
                neighborCount++;
            }
            if (neighborCount > 0) {
                tileWeights[tileId] = weight / neighborCount;
            }
            else
            {
                // no neighbors, use self weight
                tileWeights[tileId] = weights.GetWeights(tileId).GetSelfWeight();
            }
        }
        
        var chosentileId = GetRandomWeightedTile(random, tileWeights);
        
        // collapse tile
        
        IsCollapsed = true;
        PossibleTiles = new[] {chosentileId};
        
        
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
    
    public Array<Vector2> GetNeighborTiles(GeneratorTile[,] states) {
        var neighborTiles = new Array<Vector2>();
        var x = (int)Position.X;
        var y = (int)Position.Y;
        if (x > 0) {
            neighborTiles.Add(new Vector2(x - 1, y));
        }
        
        if (x < states.GetLength(0) - 1) {
            neighborTiles.Add(new Vector2(x + 1, y));
        }
        
        if (y > 0) {
            neighborTiles.Add(new Vector2(x, y - 1));
        }
        
        if (y < states.GetLength(1) - 1) {
            neighborTiles.Add(new Vector2(x, y + 1));
        }
        
        return neighborTiles;
    }

    private Array<string> GetNeighborTileIds(GeneratorTile[,] states)
    {
        var neighborTiles = new Array<string>();
        var x = (int)Position.X;
        var y = (int)Position.Y;
        if (x > 0)
        {
            neighborTiles.Add(states[x - 1, y].GetCollapsedTile());
        }

        if (x < states.GetLength(0) - 1)
        {
            neighborTiles.Add(states[x + 1, y].GetCollapsedTile());
        }

        if (y > 0)
        {
            neighborTiles.Add(states[x, y - 1].GetCollapsedTile());
        }

        if (y < states.GetLength(1) - 1)
        {
            neighborTiles.Add(states[x, y + 1].GetCollapsedTile());
        }

        // remove nulls and empty strings
        var nonNullNeighborTiles = new Array<string>();
        nonNullNeighborTiles.AddRange(neighborTiles.Where(tile => tile != null && tile != ""));
    

    return nonNullNeighborTiles;
    }
    
    private string GetCollapsedTile() {
        if (IsCollapsed) {
            return PossibleTiles[0];
        }
        return null;
    }
    
    
    
    
    
   
}