using System;
using System.Linq;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace ProjectReaper.Util;

public partial class GeneratorRoom : GodotObject
{

    public bool IsCollapsed { get; set; } = false;
    public Array<RoomDef> PossibleRooms { get; set; }
    public Vector2I Position { get; set; }

    public GeneratorRoom(Array<RoomDef> possibleRooms, Vector2I position)
    {
        PossibleRooms = possibleRooms;
        Position = position;
    }

    public int GetEntropy()
    {
        return PossibleRooms.Count - 1;
    }



    public void Collapse(RandomNumberGenerator random, GeneratorRoom[,] states, RoomSet roomSet)
    {
        var room = PossibleRooms[random.RandiRange(0, PossibleRooms.Count - 1)];
        PossibleRooms = new Array<RoomDef> { room };
        IsCollapsed = true;
        GD.Print($"Room at {Position} collapsed to {room.ID}");

    }

    public Array<RoomDef> GetALLPossibleRooms(RoomSet set, GeneratorRoom[,] state)
    {
        var possibleRooms = new Array<RoomDef>();
        foreach (var room in set.Rooms)
        {
            var allowed = true;
            foreach (var side in Enum.GetValues(typeof(RoomDef.Side)).Cast<RoomDef.Side>())
            {
                allowed = allowed && CheckConnection(side, room, state, set);
            }

            if (allowed) possibleRooms.Add(room);
        }

        return possibleRooms;

    }


    public void RecalculatePossibleRooms(RoomSet set, GeneratorRoom[,] state)
    {
        if (IsCollapsed) return;
        var possibleRooms = GetALLPossibleRooms(set, state);

        PossibleRooms = possibleRooms;
        if (PossibleRooms.Count == 1) IsCollapsed = true;
        if (PossibleRooms.Count == 0)
        {
            // see if collapsed neighbors can be changed to allow this room
            foreach (var side in Enum.GetValues(typeof(RoomDef.Side)).Cast<RoomDef.Side>())
            {
                var neighbor = GetNeighbor(side, state);
                if (neighbor == null) continue;
                var numPossibleRooms = neighbor.GetALLPossibleRooms(set, state).Count;
                if (numPossibleRooms > 1)
                {
                    neighbor.IsCollapsed = false;
                    neighbor.RecalculatePossibleRooms(set, state);
                    
                }
            }
            RecalculatePossibleRooms(set, state);

        }
        
        
        
GD.Print($"Room at {Position} has {PossibleRooms.Count} possible rooms");
    }

    private bool CheckConnection(RoomDef.Side side, RoomDef room, GeneratorRoom[,] state, RoomSet set)
    {
        var neighbor = GetNeighbor(side, state);
        if (neighbor == null) return true;
        var connection = set.GetConnectionOfId(room.GetConnectionType(side));
        var allowedConnections = connection.AllowedConnections;
        var neighborSideConnectionType = neighbor.PossibleRooms[0].GetConnectionType(RoomDef.GetOppositeSide(side));
        
       
        
        return allowedConnections.Contains(neighborSideConnectionType);
    }

    private GeneratorRoom GetNeighbor(RoomDef.Side side, GeneratorRoom[,] state) {
        var pos = Position;
        switch (side) {
            case RoomDef.Side.Top:
                if (pos.Y == 0) return null;
                return state[pos.X, pos.Y - 1];
            case RoomDef.Side.Bottom:
                if (pos.Y == state.GetLength(1) - 1) return null;
                return state[pos.X, pos.Y + 1];
            case RoomDef.Side.Left:
                if (pos.X == 0) return null;
                return state[pos.X - 1, pos.Y];
            case RoomDef.Side.Right:
                if (pos.X == state.GetLength(0) - 1) return null;
                return state[pos.X + 1, pos.Y];
            default:
                throw new ArgumentOutOfRangeException(nameof(side), side, null);
        }
    }
}