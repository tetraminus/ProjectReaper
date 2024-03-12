using Godot;
using System;
using ProjectReaper.Components;
using ProjectReaper.Enemies;

public partial class StateMachineComponent : Node
{
   

    private AbstractState _currentState;
    private AbstractState _previousState;
    public AbstractCreature Creature { get; set; }
    [Export] public Node InitialState { get; set; }
    
    public override void _Ready()
    {
        if (InitialState != null)
        {
            NextFrameChangeState(InitialState.Name);
        }
        foreach (var child in GetChildren())
        {
            if (child is AbstractState state)
            {
                state.StateMachine = this;
            }
            
        }
    }

    private async void NextFrameChangeState(StringName initialStateName)
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        ChangeState(initialStateName);
    }

    public void ChangeState(string stateName, params object[] stateArgs)
    {
        _previousState = _currentState;
        _currentState = GetNode<AbstractState>(stateName);
        if (_previousState != null)
        {
            _previousState.OnExit();
        }
        
        _currentState.OnEnter(stateArgs);
        
    }
    
    public string GetCurrentStateName()
    {
        return _currentState.Name;
    }
    
    
    public void Process(double delta)
    {
        if (_currentState != null)
        {
            _currentState.Update(delta);
        }
    }
    
    public void PhysicsProcess(double delta)
    {
        if (_currentState != null)
        {
            _currentState.PhysicsUpdate(delta);
        }
    }
    
    
}
