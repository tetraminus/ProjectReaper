using System;
using System.Linq;
using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using ProjectReaper.Player;

namespace ProjectReaper.Menu.ItemLibraryScreen;

public partial class ItemLibraryScreen : Control
{
	private ItemInfoContainer _descriptionContainer;
	private TabContainer _RarityTabContainer;
	private PackedScene _rarityTabScene = GD.Load<PackedScene>("res://Menu/ItemLibraryScreen/RarityTab.tscn");
	private PackedScene _itemDisplayScene = GD.Load<PackedScene>("res://Player/ItemDisplay.tscn");
	public event Action CloseRequested; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_descriptionContainer = GetNode<ItemInfoContainer>("%ItemInfoContainer");
		_RarityTabContainer = GetNode<TabContainer>("%TabContainer");
		
		LoadItems();
	}
	
	public void LoadItems()
	{
		var items = ItemLibrary.Instance.ItemsByRarity.ToDictionary(x => x.Key, x => x.Value);
		
		items = items.OrderBy(x => x.Key.Value).ToDictionary( x => x.Key, x => x.Value);
		
		foreach (var Rarity in items.Keys)
		{
			var tab = _rarityTabScene.Instantiate<RarityTab>();
			_RarityTabContainer.AddChild(tab);
			tab.Name = Rarity.NameKey;
			
			var item = items[Rarity];
			foreach (var abstractItem in item)
			{
				var itemDisplay = _itemDisplayScene.Instantiate<ItemDisplay>();
				
				itemDisplay.FocusMode = FocusModeEnum.All;
				
				itemDisplay.SetItem(abstractItem);
				itemDisplay.HideStacks();
				itemDisplay.MouseEnteredItem += ItemHovered;
				itemDisplay.FocusEnteredItem += ItemHovered;
				
				
				tab.AddItem(itemDisplay);
				
			}
		}
		
		_RarityTabContainer.TabChanged += TabChanged;
	}
	
	public void JumpToItem(AbstractItem item)
	{
		var tab = _RarityTabContainer.GetNode<RarityTab>(item.Rarity.NameKey);
		_RarityTabContainer.CurrentTab = tab.GetIndex();
		tab.JumpToItem(item);
	}
	
	public void ItemHovered(AbstractItem item)
	{
		_descriptionContainer.SetItem(item);
	}
	
	public void TabChanged(long index)
	{
		_descriptionContainer.Reset();
	}
	
	public void OnBackButtonPressed()
	{
		CloseRequested?.Invoke();
	}


	public void Focus()
	{
		// Focus the first item in the first tab
		_RarityTabContainer.GetChild<RarityTab>(0).FocusFirst();
	}
}


