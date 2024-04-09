using Godot;
using System;
using ProjectReaper.Globals;

public partial class ScreenFader : ColorRect
{
	[Signal]
	public delegate void FadeOutCompleteEventHandler();
	
	[Signal]
	public delegate void FadeInCompleteEventHandler();
	
	
	public void FadeOut(float duration)
	{
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, CanvasItem.PropertyName.Modulate.ToString(), new Color(1, 1, 1, 1), duration);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.Finished += () =>
		{
			EmitSignal(SignalName.FadeOutComplete); 
			GameManager.MainNode.EmitSignal(Main.SignalName.TransitionComplete);
		};
		
	}
	
	public void FadeIn(float duration)
	{
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, CanvasItem.PropertyName.Modulate.ToString(), new Color(1, 1, 1, 0), duration);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.Finished += () => { EmitSignal(SignalName.FadeInComplete); };
	}
	
	
	
		
}
