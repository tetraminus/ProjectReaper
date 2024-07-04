using Godot;
using System;

public partial class FocusRedirectComponent : Control
{
	
	[Export]
	private Control _focusTarget;
	public override void _Ready()
	{
		if (_focusTarget == null) {
			throw new Exception("Focus target is not set");
		}
		
	}
	
	public void Focus() {
		_focusTarget.GrabFocus();
	}

}
