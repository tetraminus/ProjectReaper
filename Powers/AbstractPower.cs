using System;
using System.Collections.Generic;
using Godot;
using ProjectReaper.Enemies;
using ProjectReaper.Globals;

namespace ProjectReaper.Powers;

public abstract partial class AbstractPower : Node
{
    
    private static Dictionary<Type, string> _idCache = new();
    
    public abstract string Id { get; }
    public AbstractCreature Creature { get; set; }
    public abstract float Duration { get; }
    public int Stacks { get; private set; }
    protected Timer Timer;
    

    public override void _Ready()
    {
        // setup the timer if the duration is greater than 0
        if (Duration > 0)
        {
            Timer = new Timer
            {
                Autostart = false,
                OneShot = true
            };
            AddChild(Timer);
            Timer.Timeout += OnTimerTimeout;
        }
        
        
        
    }
    
    public void SetStacks(int stacks)
    {
        Stacks = stacks;
    }

    private void OnTimerTimeout()
    {
        Creature.RemovePower(this);
    }

    public void AddStacks(int stacks, bool resetDuration = false)
    {
        if (resetDuration)
        {
            Timer.Stop();
            StartTimer();
        }
        Stacks += stacks;
        OnStack(stacks);
    }
    
    protected void StartTimer()
    {
        Timer.Start(Duration);
    }
    
    public double GetTimeRemaining()
    {
        return Timer.TimeLeft;
    }
    
    
    
    public virtual void OnApply()
    {
        
        
    }
    
    public virtual void OnStack(int newStacks)
    {
        
    }
    
    public static string GetId<T>() where T : AbstractPower
    {
        var type = typeof(T);
        if (_idCache.TryGetValue(type, out var id1))
        {
            return id1;
        }
        else
        {
            var power = Activator.CreateInstance<T>();
            var id = power.Id;
            _idCache[type] = id;
            return id;
        }
    }
    
    
    public virtual void OnRemove()
    {
        
    }
    
}
