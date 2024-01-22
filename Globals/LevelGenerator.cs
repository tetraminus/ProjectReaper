using System;
using System.Linq;
using Godot;
using Godot.Collections;
using ProjectReaper.Util;

namespace ProjectReaper.Globals;

public partial class LevelGenerator : Node
{
    
    public static LevelGenerator Instance { get; private set; }
    public bool Failed { get; set; } = false;
    
    public override void _Ready()
    {
        Instance = this;
    }
    
    
    public void GenerateLevel(RoomSet set, int width, int height, ulong seed)
    {
        Failed = false;
        // preloading rooms
        var rooms = new Dictionary<string, PackedScene>();
        foreach (var room in set.Rooms)
        {
            rooms[room.ID] = ResourceLoader.Load<PackedScene>($"res://rooms/{set.ID}/{room.ID}.tscn");
        }
        
        // creating random number generator
        var random = new RandomNumberGenerator();
        random.Randomize();
        random.Seed = seed;

        
        
        // creating state
        var state = new GeneratorRoom[width, height];
        do
        {
            Failed = false;
            
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++) state[i, j] = new GeneratorRoom(set.Rooms, new Vector2I(i, j));
            }

            // generating level
            while (!state.Cast<GeneratorRoom>().All(room => room.IsCollapsed))
            {
                Array<GeneratorRoom> stack = new Array<GeneratorRoom>();
                // grab lowest entropy
                var lowestEntropy = state.Cast<GeneratorRoom>().Where(room => !room.IsCollapsed).Min(room => room.GetEntropy());
                // pick random room with lowest entropy
                var lowEntRoom = state.Cast<GeneratorRoom>().Where(room => !room.IsCollapsed && room.GetEntropy() == lowestEntropy)
                    .ToList()[random.RandiRange(0, state.Cast<GeneratorRoom>().Count(room => !room.IsCollapsed && room.GetEntropy() == lowestEntropy) - 1)];
                

                lowEntRoom.Collapse(random, state, set);

                // propagate information

                stack.AddRange(GetNeighbors(lowEntRoom, state));

                while (stack.Count > 0)
                {
                    var room = stack[0];
                    stack.RemoveAt(0);
                    room.RecalculatePossibleRooms(set, state);
                    if (room.IsCollapsed) stack.AddRange(GetNeighbors(room, state));

                    // check if failed and reset state if so
                    if (Failed)
                    {
                        break;
                    }
                }
                
                if (Failed)
                {
                    break;
                }
            }
            if (Failed)
            {
                GD.Print("Failed, retrying");
                random.Seed++;
            }
        } while (Failed);

        // debug print layout
        for (var i = 0; i < width; i++)
        {
            var line = "";
            for (var j = 0; j < height; j++)
            {
                var room = state[i, j];
                line += room.PossibleRooms.Count + " ";
            }
            GD.Print(line);
        }
        
        // creating rooms
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++) {
                var room = state[i, j];
                var roomDef = room.PossibleRooms[0];
                var scene = rooms[roomDef.ID];
                var instance = (Node2D)scene.Instantiate();
                instance.Name = $"{roomDef.ID}_{i}_{j}";
                instance.Position = new Vector2(i * 160, j * 160);
                
                // rotate around center
                instance.RotationDegrees = roomDef.Rotation * -90;
                
                
                
                GameManager.Level.AddChild(instance);
                var label = new Label();
                label.Text = roomDef.Rotation.ToString();
                label.Position = new Vector2(0, 0);
                instance.AddChild(label);
        
                label.LabelSettings = new LabelSettings();
                label.LabelSettings.OutlineColor = new Color(0, 0, 0, 1);
                label.LabelSettings.OutlineSize = 3;
                
                // add debug text at each side showing connection type
                foreach (var side in Enum.GetValues(typeof(RoomDef.Side)).Cast<RoomDef.Side>())
                {
                    var sidelabel = new Label();
                    sidelabel.Text = roomDef.GetConnectionType(side - roomDef.Rotation).ToString();
                    sidelabel.Position = new Vector2(0, 0);
                    instance.AddChild(sidelabel);
        
                    sidelabel.LabelSettings = new LabelSettings();
                    sidelabel.LabelSettings.OutlineColor = new Color(0, 0, 0, 1);
                    sidelabel.LabelSettings.OutlineSize = 3;
                    
                    
                    switch (side)
                    {
                           case RoomDef.Side.Top:
                                sidelabel.Position = new Vector2(0, -50);
                                break;
                            case RoomDef.Side.Right:
                                sidelabel.Position = new Vector2(50, 0);
                                break;
                            case RoomDef.Side.Bottom:
                                sidelabel.Position = new Vector2(0, 50);
                                break;
                            case RoomDef.Side.Left:
                                sidelabel.Position = new Vector2(-50, 0);
                                break;
                       
                    }
                }
                
            }
        }
        
    }
    
    private Array<GeneratorRoom> GetNeighbors(GeneratorRoom room, GeneratorRoom[,] state)
    {
        var neighbors = new Array<GeneratorRoom>();
        var pos = room.Position;
        if (pos.X > 0) neighbors.Add(state[pos.X - 1, pos.Y]);
        if (pos.X < state.GetLength(0) - 1) neighbors.Add(state[pos.X + 1, pos.Y]);
        if (pos.Y > 0) neighbors.Add(state[pos.X, pos.Y - 1]);
        if (pos.Y < state.GetLength(1) - 1) neighbors.Add(state[pos.X, pos.Y + 1]);
        
        // remove collapsed rooms
        var NonCollapsedNeighbors = neighbors.Where(neighbor => !neighbor.IsCollapsed);
        
        return new Array<GeneratorRoom>(NonCollapsedNeighbors);
    }
    
}