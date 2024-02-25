using System;
using Godot;
using Godot.Collections;

namespace ProjectReaper.Util;

public partial class RoomDef : GodotObject
{
    public enum Side
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3
    }

    public RoomDef(string id, Dictionary<Side, int> sideConnections, bool rotatable = false)
    {
        ID = id;
        SideConnections = sideConnections;
    }

    public RoomDef()
    {
        ID = null;
        SideConnections = null;
    }

    public string ID { get; set; }

    public Dictionary<Side, int> SideConnections { get; set; }

    public bool Rotatable { get; set; } = false;

    public int Rotation { get; set; } // 0 - 3

    public int GetConnectionType(Side side)
    {
        // account for rotation
        var rotatedSide = (Side)(((int)side + Rotation) % 4);
        return SideConnections[rotatedSide];
    }

    public static Side GetOppositeSide(Side side)
    {
        return side switch
        {
            Side.Top => Side.Bottom,
            Side.Bottom => Side.Top,
            Side.Left => Side.Right,
            Side.Right => Side.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
        };
    }

    public RoomDef Copy()
    {
        return new RoomDef(ID, SideConnections, Rotatable) { Rotation = Rotation };
    }
}