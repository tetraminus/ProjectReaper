using Godot;
using System;
using ProjectReaper.Globals;
using Key = ProjectReaper.Objects.Key.Key;

public abstract partial class AbstractShopItem : Node2D, IInteractable
{
	
	

	public override void _Ready()
	{
		base._Ready();
		
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
	public abstract bool CheckCost();

	public bool CanInteract()
	{
		if (CheckCost())
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
