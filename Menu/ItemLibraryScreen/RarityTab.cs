using Godot;
using ProjectReaper.Player;

namespace ProjectReaper.Menu.ItemLibraryScreen;

public partial class RarityTab : ScrollContainer
{
	
	private HFlowContainer ItemContainer => GetNode<HFlowContainer>("HFlowContainer");
	
	public void AddItem(ItemDisplay itemDisplay)
	{
		ItemContainer.AddChild(itemDisplay);
	}
}