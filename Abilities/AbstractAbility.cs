using Godot;

namespace ProjectReaper.Abilities;

public abstract partial class  AbstractAbility: Node
{
    private Timer _timer;
    private bool _isOnCooldown;
    
    public override void _Ready()
    {
        _timer = new Timer();
        AddChild(_timer);
        _timer.Timeout += () => _isOnCooldown = false;
    }
    
    public virtual void Use() {}
    public virtual void Use(Vector2 pos) {}
    public virtual void Use(float angle) {}
    
    public abstract float Cooldown { get; set; }
    
    public int Charges { get; set; } = 1;
    
    public void StartCooldown()
    {
        _timer.Start(Cooldown);
        _isOnCooldown = true;
    }
    
    public bool CheckCooldown()
    {
        return _isOnCooldown;
    }
}