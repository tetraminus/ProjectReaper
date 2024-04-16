using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;
using ProjectReaper.Player;

namespace ProjectReaper.Abilities;

public abstract partial class AbstractAbility : Node
{
    private bool _isOnCooldown;
    private Timer _timer;
    private AbilityManager.AbilitySlot _slot;

    protected abstract float Cooldown { get; set; }
    
    

    public int Charges { get; set; } = 1;
    public AbstractCreature Creature { get; set; }
    public bool AttackSpeedEffectsCooldown = false;

    public override void _Ready()
    {
        _timer = new Timer();
        AddChild(_timer);
        _timer.Timeout += () => _isOnCooldown = false;
        
    }
    
    public void SetCreature(AbstractCreature creature)
    {
        Creature = creature;
    }
    
    public void SetSlot(AbilityManager.AbilitySlot slot)
    {
        _slot = slot;
    }

    public virtual void Use()
    {
    }

    public virtual void Use(Vector2 pos)
    {
    }

    public virtual void Use(float angle)
    {
    }

    public void StartCooldown()
    {
        
        var cd = Cooldown;
        Callbacks.Instance.CalculateStat?.Invoke(ref cd, "AbilityCooldown" + _slot, Creature);
        if (AttackSpeedEffectsCooldown)
        {
            cd /= Creature.Stats.AttackSpeed;
        }
        _timer.Start(cd);
        _isOnCooldown = true;
        
    }

    public bool CheckCooldown()
    {
        return _isOnCooldown;
    }
}