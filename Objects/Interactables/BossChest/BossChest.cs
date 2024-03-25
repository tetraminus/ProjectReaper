using System;
using System.Collections.Generic;
using Godot;

using ProjectReaper.Globals;
using ProjectReaper.Items;
using Key = ProjectReaper.Objects.Key.Key;
public partial class BossChest : Node2D, IInteractable
{
	private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("HighlightComponent/Sprite");
	private bool _opened;
	private List<AbstractItem> _items = new List<AbstractItem>();
	private ShaderMaterial _shaderMaterial;
	private const int Items = 2;
    
    
    
	public override void _Ready()
	{

		for (var i = 0; i < Items; i++)
		{
			_items.Add(ItemLibrary.Instance.RollItem(ItemRarity.Rare));
		}
		_opened = false;
		_shaderMaterial = AnimatedSprite2D.Material as ShaderMaterial;
	}
    
	public void Interact()
	{
		if (!_opened)
		{
			_opened = true;
			foreach (var item in _items)
			{
				item.Drop(GlobalPosition);
			}
			AnimatedSprite2D.Play("Open");
			GameManager.Player.UseKey(Key.BasicKeyId, 5);
		}
	}
    
    

	public bool CanInteract() {
		return !_opened && GameManager.Player.HasKey(Key.BasicKeyId);
	}

	public string GetPrompt(bool Interactable = false)
	{
		if (_opened)
		{
			return "";
		}
		return Interactable ? "ui_open_chest" : "ui_locked_chest" + 5;
	}
}
