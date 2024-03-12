using Godot;
using System;

public partial class ScreenFader : ColorRect
{
	
	[Signal]
	public delegate void FadeOutCompleteEventHandler();
	
	[Signal]
	public delegate void FadeInCompleteEventHandler();
	
	public void FadeOut(float duration)
	{
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "color", new Color(0, 0, 0, 1), duration);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.Finished += () => { EmitSignal(SignalName.FadeOutComplete); };
	}
	
	public void FadeIn(float duration)
	{
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "color", new Color(0, 0, 0, 0), duration);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.Finished += () => { EmitSignal(SignalName.FadeInComplete); };
	}
	
	
	
		
}
