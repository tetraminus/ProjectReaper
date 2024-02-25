using Godot;

namespace ProjectReaper.Player;

public partial class PlayerCamera : Camera2D
{
    [ExportCategory("Follow")] [Export] public CharacterBody2D Target { get; set; }

    [ExportCategory("Smooth")] [Export] public bool EnableSmoothing { get; set; } = true;

    [Export(PropertyHint.Range, "0, 10")] public int SmoothingDistance { get; set; } = 8;

    public override void _PhysicsProcess(double delta)
    {
        if (Target == null) return;
        float Weight;
        Vector2 camPosition;

        if (EnableSmoothing)
        {
            Weight = (11 - SmoothingDistance) / 100f;
            camPosition = GlobalPosition.Lerp(Target.GlobalPosition, Weight);
        }
        else
        {
            camPosition = Target.GlobalPosition;
        }

        GlobalPosition = camPosition.Floor();
    }
}