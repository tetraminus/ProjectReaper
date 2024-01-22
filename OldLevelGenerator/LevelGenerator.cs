using System.Linq;
using System.Xml;
using Godot;
using Godot.Collections;

namespace ProjectReaper.Util; 

public partial class LevelGenerator : Node {
    
    // Wave Function Collapse level generator
    
    public static LevelGenerator Instance { get; private set; }
    
    public override void _Ready() {
        Instance = this;
    }
    
    private float SimilarityWeight = 0.8f;
    private GeneratorTile[,] _states;
    private TilesetWeights _weights;

    public void GenerateLevel(TileMap tileMap, TileSet set, int width, int height, ulong seed)
    {
        ulong startTime = Time.GetTicksMsec();
        
        var random = new RandomNumberGenerator();
        random.Seed = seed;
        // clear map
        

        // set states from existing tiles
        _states = new GeneratorTile[width, height];
        TileMapPattern tilemapStates;
        Array<Vector2I> coords = new Array<Vector2I>();
        
        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++)
            {
                coords.Add(new Vector2I(x,y));
                _states[x,y] = new GeneratorTile(_weights.GetTileIds(), new Vector2I(x,y));
            }
        }
        
        tilemapStates = tileMap.GetPattern(0, coords);
        
        foreach (var coord in coords)
        {
            var tileId = tilemapStates.GetCellAtlasCoords(coord);
            if (tileId == new Vector2I(0,0)) {
                _states[coord.X, coord.Y].IsCollapsed = true;
                _states[coord.X, coord.Y].PossibleTiles = new[] {"Ground"};
            } else if (tileId == new Vector2I(1,0)) {
                _states[coord.X, coord.Y].IsCollapsed = true;
                _states[coord.X, coord.Y].PossibleTiles = new[] {"Wall"};
            }
        }
        
        GD.Print("done setting states" + (Time.GetTicksMsec() - startTime));
        
        
        while (!IsCollapsed())
        {
            Array<Vector2> stack = new Array<Vector2>();
            var tilePos = GetRandomLowEntropyTile(random);
            var tile = _states[(int) tilePos.X, (int) tilePos.Y];
            // collapse tile
            var tileId = tile.Collapse(random, _states, _weights);
            
            // add neighbors to stack
            foreach (var neighborTile in GetNeighborTiles(tilePos)) {
                if (!_states[(int)neighborTile.X, (int)neighborTile.Y].IsCollapsed)
                {
                    stack.Add(neighborTile);
                }
            }

            while (stack.Count > 0) {
                var neighborTilePos = stack[0];
                stack.RemoveAt(0);
                var neighborTile = _states[(int) neighborTilePos.X, (int) neighborTilePos.Y];
                // collapse tile
                var neighborTileId = neighborTile.Collapse(random, _states, _weights);
                
                // add neighbors to stack
                foreach (var neighborNeighborTile in GetNeighborTiles(neighborTilePos)) {
                    if (!_states[(int)neighborNeighborTile.X, (int)neighborNeighborTile.Y].IsCollapsed && !stack.Contains(neighborNeighborTile))
                    {
                        stack.Add(neighborNeighborTile);
                    }
                }
                GD.Print("current stack count: " + stack.Count);
                GD.Print("current tile: " + neighborTilePos);
            }
            
            // collapse neighbors
            
        }
        
       
        //GD.Print("done collapsing" + (Time.GetTicksMsec() - startTime));
        // print map
        for (var x = 0; x < width; x++) {
            var line = "";
            for (var y = 0; y < height; y++)
            {
                var tile = _states[x,y];
                var tileId = tile.PossibleTiles[0];
                if (tileId == "Ground") {
                    line += "G";
                } else if (tileId == "Wall") {
                    line += "W";
                }
            }
            GD.Print(line);
        }
        
        
        // set tiles
        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++)
            {
                var tile = _states[x,y];
                var tileId = tile.PossibleTiles[0];
                if (tileId == "Ground") {
                    tileMap.SetCell(0, tile.Position, 0, new Vector2I(0,0));
                } else if (tileId == "Wall") {
                    tileMap.SetCell(0, tile.Position, 0, new Vector2I(1,0));
                }
            }
        }
        GD.Print("done" + (Time.GetTicksMsec() - startTime));
        
        
        
    }
    
    private Vector2 GetRandomLowEntropyTile(RandomNumberGenerator random) {
        var lowestEntropy = int.MaxValue;
        
        foreach (var state in _states) {
            if (state.GetEntropy() < lowestEntropy && !state.IsCollapsed) {
                lowestEntropy = state.GetEntropy();
            }
        }
        
        var possibleTiles = new Array<Vector2>();
        foreach (var state in _states) {
            if (state.GetEntropy() == lowestEntropy && !state.IsCollapsed) {
                possibleTiles.Add(state.Position);
            }
        }
        
        return possibleTiles[random.RandiRange(0, possibleTiles.Count - 1)];
    }
    private Vector2 GetRandomLowEntropyNeighborTile(RandomNumberGenerator random, Vector2 tilePos) {
        var lowestEntropy = int.MaxValue;
        GeneratorTile[] neighborTiles = new GeneratorTile[0];
        foreach (var neighborTile in GetNeighborTiles(tilePos)) {
            var state = _states[(int) neighborTile.X, (int) neighborTile.Y];
            if (state.GetEntropy() < lowestEntropy && !state.IsCollapsed) {
                lowestEntropy = state.GetEntropy();
            }
        }
        
        var possibleTiles = new Array<Vector2>();
        foreach (var neighborTile in GetNeighborTiles(tilePos)) {
            var state = _states[(int) neighborTile.X, (int) neighborTile.Y];
            if (state.GetEntropy() == lowestEntropy && !state.IsCollapsed) {
                possibleTiles.Add(state.Position);
            }
        }
        
        return possibleTiles[random.RandiRange(0, possibleTiles.Count - 1)];
    }
    
    private Array<Vector2> GetNeighborTiles(Vector2 tilePos) {
        return _states[(int) tilePos.X, (int) tilePos.Y].GetNeighborTiles(_states);
        
    }

    
    
    
    public bool IsCollapsed() {
        foreach (var state in _states) {
            if (!state.IsCollapsed) {
                return false;
            }
        }
        return true;
    }


    public override void _EnterTree()
    {
        _weights = TileWeightLoader.GetTilesetWeights("TestSet");
        base._EnterTree();
    }


    
}