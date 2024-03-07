using Godot;

namespace ProjectReaper.Util;

public partial class RunInfo : GodotObject
{
    public int CurrentLevel { get; set; }
    public ulong Seed { get; set; }
    public Player.Player Player { get; set; }
}