namespace ProjectReaper.Util; 

public partial class Stats {
	
	public float Health { get; set; }
	
	public float MaxHealth { get; set; }
	
	public float Damage { get; set; }
	
	public float Defense { get; set; }
	
	public float Armor { get; set; }
	
	public float Speed { get; set; }
	public float CritChance { get; set; }
	public float CritDamage { get; set; }
	public float AttackSpeed { get; set; }
	
	public float Range { get; set; }
	
	public float AttackDamage { get; set; }
	
	public float AttackKnockback { get; set; }
	
	public float AttackStun { get; set; }
	
	public float AttackDuration { get; set; }
	
	public float AttackSpread { get; set; }

	public void Init() {
		Health = 100;
		MaxHealth = 100;
		Damage = 10;
		Speed = 100;
		Range = 100;
		Defense = 0;
		Armor = 0;
		CritChance = 0.01f;
		CritDamage = 2f;
		AttackSpeed = 1f;
		AttackDamage = 10f;
		AttackKnockback = 0f;
		AttackStun = 0f;
		AttackDuration = 1f;
		AttackSpread = 0f;
	}
}
