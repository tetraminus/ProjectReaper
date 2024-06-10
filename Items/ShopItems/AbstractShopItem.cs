using Godot;
using System;
using ProjectReaper.Globals;
using Key = ProjectReaper.Objects.Key.Key;

public abstract partial class AbstractShopItem : Node2D, IInteractable
{
	
	public int Cost { get; set; }
	public Sprite2D Sprite2D;

	public override void _Ready()
	{
		base._Ready();
		Sprite2D = new Sprite2D();
	}

	public void Interact()
	{
		if (CanInteract())
		{
			Buy();
		}
		else
		{
			
		}
		
	}

	public abstract void Buy();

	public bool CanInteract()
	{
		if (GameManager.Player.HasKey(Key.BasicKeyId, Cost))
		{
			return true;
		}
		return false;
	}

	public string GetPrompt(bool Interactable = false)
	{
		if (CanInteract())
		{
			return "Buy";
		}
		return "Not enough keys";
	}
}
