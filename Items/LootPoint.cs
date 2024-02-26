using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Items;

public partial class LootPoint : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LootDirector.Instance.AddLootPoint(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}