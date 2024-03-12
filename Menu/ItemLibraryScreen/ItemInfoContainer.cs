using System;
using Godot;
using ProjectReaper.Items;

namespace ProjectReaper.Menu.ItemLibraryScreen;

public partial class ItemInfoContainer : VBoxContainer
{
	public AbstractItem Item { get; set; }
	private Label _itemName;
	private RichTextLabel _itemDescription;
	private RichTextLabel _itemFlavorText;
	
	public override void _Ready()
	{
		_itemName = GetNode<Label>("Title");
		_itemFlavorText = GetNode<RichTextLabel>("Flavor");
		_itemDescription = GetNode<RichTextLabel>("Description");
	}
	
	public void Reset()
	{
		_itemName.Text = "";
		_itemDescription.Text = "";
		_itemFlavorText.Text = "";
	}

	public void SetItem(AbstractItem item)
	{
		Item = item;
		_itemName.Text = item.GetNameKey();
		_itemDescription.Text = item.GetDescriptionKey();
		_itemFlavorText.Text = item.GetFlavorKey();
	}
}