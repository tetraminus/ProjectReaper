using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Items.Collectables;

public partial class DreamCar : AbstractItem
{
	public override string Id => "dream_car";
	public override ItemRarity Rarity => ItemRarity.Common;
	private const float CooldownReduction = 0.85f; // 15% per stack
	
	public override void OnInitalPickup()
	{
		Callbacks.Instance.CalculateStat += CalculateStat;
	}
	
	private void CalculateStat(ref float stat, string statName, AbstractCreature creature)
	{
		if (statName == "AbilityCooldown" + AbilityManager.AbilitySlot.Utility && creature == GetHolder())
		{
			
			stat *= Mathf.Pow(CooldownReduction, Stacks);
		}
		
	}
	
	public override void Cleanup()
	{
		Callbacks.Instance.CalculateStat -= CalculateStat;
	}
	
}
