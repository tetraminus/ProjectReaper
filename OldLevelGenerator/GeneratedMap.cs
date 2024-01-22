using Godot;
using ProjectReaper.Util;

namespace ProjectReaper.Tilesets;

public partial class GeneratedMap : TileMap
{
    public override void _Ready()
    {
        LevelGenerator.Instance.GenerateLevel(this, TileSet, 20, 20, GD.Randi());
    }
}