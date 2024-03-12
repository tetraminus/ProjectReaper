using System.Linq;
using Godot;
using ProjectReaper.Items;
using ProjectReaper.Player;

namespace ProjectReaper.Menu.ItemLibraryScreen;

public partial class RarityTab : ScrollContainer
{
	
	private HFlowContainer ItemContainer => GetNode<HFlowContainer>("HFlowContainer");
	
	public void AddItem(ItemDisplay itemDisplay)
	{
		ItemContainer.AddChild(itemDisplay);
	}

	public void JumpToItem(AbstractItem item)
	{
		foreach (ItemDisplay itemDisplay in ItemContainer.GetChildren().OfType<ItemDisplay>())
		{
			if (itemDisplay.Item != item) continue;
			itemDisplay.GrabFocus();
			break;
		}
	}
	
	public void FocusFirst()
	{
		var itemDisplay = ItemContainer.GetChildren().OfType<ItemDisplay>().FirstOrDefault();
		itemDisplay?.GrabFocus();
	}
	
}