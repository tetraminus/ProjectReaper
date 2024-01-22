using System;
using System.Collections.Generic;
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
        var rooms = new Godot.Collections.Dictionary<string, PackedScene>();
        foreach (var room in set.Rooms)
        {
            rooms[room.ID] = ResourceLoader.Load<PackedScene>($"res://rooms/{set.ID}/{room.ID}.tscn");
        }
        
        // creating random number generator
        var random = new RandomNumberGenerator();
        random.Randomize();
        random.Seed = seed;

        
        
        // creating state
        GeneratorRoom[,] state;
        do
        {
            Failed = false;
            
            
            state = DefaultState(set, width, height);
            
            // create state history for backtracking
            var stateHistory = new Stack<GeneratorRoom[,]>();
            stateHistory.Push(state);

            // generating level
            while (!state.Cast<GeneratorRoom>().All(room => room.IsCollapsed))
            {
                Stack<GeneratorRoom> stack = new Stack<GeneratorRoom>();
                // grab lowest entropy
                var lowestEntropy = state.Cast<GeneratorRoom>().Where(room => !room.IsCollapsed).Min(room => room.GetEntropy());
                // pick random room with lowest entropy
                var lowEntRoom = state.Cast<GeneratorRoom>().Where(room => !room.IsCollapsed && room.GetEntropy() == lowestEntropy)
                    .ToList()[random.RandiRange(0, state.Cast<GeneratorRoom>().Count(room => !room.IsCollapsed && room.GetEntropy() == lowestEntropy) - 1)];
                

                lowEntRoom.Collapse(random, state, set);

                // propagate information

                foreach (var neighbor in GetNeighbors(lowEntRoom, state))
                {
                    stack.Push(neighbor);
                }

                while (stack.Count > 0) {
                    var room = stack.Pop();
                    room.RecalculatePossibleRooms(set, state);
                    // if collapsed, propagate
                    if (room.IsCollapsed)
                    {
                        foreach (var neighbor in GetNeighbors(room, state))
                        {
                            stack.Push(neighbor);
                        }
                    }
                    if (Failed) {
                        BlastRooms(set, width, height, state);
                        Failed = false;
                    }
                }
                if (Failed)
                {
                    BlastRooms(set, width, height, state);
                    Failed = false;
                    
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

    private static void BlastRooms(RoomSet set, int width, int height, GeneratorRoom[,] state) {
        // find failed rooms, and reset an area around them
        var failedRooms = state.Cast<GeneratorRoom>().Where(room => !room.IsCollapsed && room.PossibleRooms.Count == 0)
            .ToList();
        foreach (var failedRoom in failedRooms) {
            var pos = failedRoom.Position;
            for (var i = pos.X - 1; i <= pos.X + 1; i++) {
                for (var j = pos.Y - 1; j <= pos.Y + 1; j++) {
                    if (i < 0 || i >= width || j < 0 || j >= height) continue;
                    state[i, j].Reset(set);
                }
            }

            for (var i = pos.X - 1; i <= pos.X + 1; i++) {
                for (var j = pos.Y - 1; j <= pos.Y + 1; j++) {
                    if (i < 0 || i >= width || j < 0 || j >= height) continue;
                    state[i, j].RecalculatePossibleRooms(set, state);
                }
            }
        }
    }

    private static GeneratorRoom[,] DefaultState(RoomSet set, int width, int height) {
        GeneratorRoom[,] state = new GeneratorRoom[width, height];
        for (var i = 0; i < width; i++) {
            for (var j = 0; j < height; j++) state[i, j] = new GeneratorRoom(set.Rooms, new Vector2I(i, j));
        }
        return state;
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