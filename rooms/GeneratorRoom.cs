using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public partial class GeneratorRoom : GodotObject
{
    public GeneratorRoom(Array<RoomDef> possibleRooms, Vector2I position)
    {
        PossibleRooms = possibleRooms.Duplicate();
        Position = position;
    }

    public bool IsCollapsed { get; set; }
    public Array<RoomDef> PossibleRooms { get; set; }
    public Vector2I Position { get; set; }

    public int GetEntropy()
    {
        return PossibleRooms.Count - 1;
    }

    public void Collapse(RandomNumberGenerator random, GeneratorRoom[,] states, RoomSet roomSet)
    {
        RecalculatePossibleRooms(roomSet, states);

        if (PossibleRooms.Count == 0)
        {
            GD.Print($"Room at {Position} has no possible rooms");
            Globals.LevelGenerator.Instance.Failed = true;
            return;
        }

        var room = PossibleRooms[random.RandiRange(0, PossibleRooms.Count - 1)];
        PossibleRooms = new Array<RoomDef> { room };
        IsCollapsed = true;
        GD.Print($"Room at {Position} collapsed to {room.ID}");
    }

    public Array<RoomDef> GetALLPossibleRooms(RoomSet set, GeneratorRoom[,] state, GeneratorRoom exclude = null)
    {
        var possibleRooms = new Array<RoomDef>();
        foreach (var room in set.Rooms)
        {
            var allowed = true;
            foreach (var side in Enum.GetValues(typeof(RoomDef.Side)).Cast<RoomDef.Side>())
            {
                if (GetNeighbor(side, state) != null && GetNeighbor(side, state).PossibleRooms.Count == 0) continue;
                allowed = allowed && CheckConnection(side, room, state, set);
            }

            if (allowed) possibleRooms.Add(room);
        }

        return possibleRooms;
    }


    public void RecalculatePossibleRooms(RoomSet set, GeneratorRoom[,] state, GeneratorRoom exclude = null)
    {
        if (IsCollapsed) return;
        var possibleRooms = GetALLPossibleRooms(set, state, exclude);

        PossibleRooms = possibleRooms;
        if (PossibleRooms.Count == 1) IsCollapsed = true;
        if (PossibleRooms.Count == 0)
        {
            GD.Print($"Room at {Position} has no possible rooms");
            Globals.LevelGenerator.Instance.Failed = true;
        }

        GD.Print($"Room at {Position} has {PossibleRooms.Count} possible rooms");
    }

    public void Reset(RoomSet r)
    {
        IsCollapsed = false;
        PossibleRooms = r.Rooms;
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

    private GeneratorRoom GetNeighbor(RoomDef.Side side, GeneratorRoom[,] state)
    {
        var pos = Position;
        switch (side)
        {
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