using Godot;
using System;
using ProjectReaper.Items;

public partial class RarityIndicator : Node2D
{
	
	private TextureRect _rarityIcon;
	private ItemRarity _rarity;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rarityIcon = GetNode<TextureRect>("RarityIcon");
		
	}
	
	public void SetRarity(ItemRarity rarity) {
		_rarity = rarity;
		_rarityIcon.Modulate = rarity.Color;
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_rarityIcon.Modulate = _rarity?.Color ?? Colors.Transparent;
	}
}
