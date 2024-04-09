using Godot;
using System;
using System.Collections.Generic;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using ProjectReaper.Player;

public partial class ChooseItemMenu : Control
{
	private HBoxContainer _itemContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_itemContainer = GetNode<HBoxContainer>("%ItemContainer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void AddItems(List<AbstractItem> items) {
		foreach (var item in items) {
			var itemButton = ItemLibrary.ItemDisplayScene.Instantiate<ItemDisplay>();
			_itemContainer.AddChild(itemButton);
			itemButton.SetItem(item);
			itemButton.ClickedItem += OnItemClicked;
			itemButton.HideStacks();
			itemButton.SetSize(itemButton.Size * 2);
		}
	}
	
	private void OnItemClicked(AbstractItem item) {
		GameManager.Player.AddItem(item);
		GameManager.HideScreen(this);
		Clear();
	}
	
	public void Clear() {
		foreach (Node child in _itemContainer.GetChildren()) {
			if (child is ItemDisplay itemDisplay) {
				itemDisplay.ClickedItem -= OnItemClicked;
			}
			child.QueueFree();
			
		}
	}
	
}
