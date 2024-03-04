using Godot;

namespace ProjectReaper.Components;

public abstract partial class AbstractState : Node
{
    public StateMachineComponent StateMachine { get; set; }
    
    public virtual void OnEnter(object[] args)
    {
        
    }
    
    public virtual void Update(double delta)
    {
        
    }
    
    public virtual void PhysicsUpdate(double delta)
    {
        
    }
    
    public virtual void OnExit()
    {
        
    }
    
}