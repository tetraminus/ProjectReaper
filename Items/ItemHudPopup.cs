using Godot;

namespace ProjectReaper.Items;

public partial class ItemHudPopup : Control
{
	public Label NameLabel => GetNode<Label>("%NameLabel");
	public RichTextLabel DescriptionLabel => GetNode<RichTextLabel>("%DescriptionLabel");
	private PanelContainer _panel => GetNode<PanelContainer>("HudPanel");

	public override void _Ready()
	{
		base._Ready();
		Visible = false;
	}

	public void SetItem(AbstractItem item)
	{
		NameLabel.Text = item.GetNameKey();
		DescriptionLabel.Text = item.GetDescriptionKey();
	}

	public override void _Process(double delta)
	{
		
		
		// Clamp to top of screen
		var panelRect = _panel.GetRect();
		var screenSize = GetViewportRect().Size;
		var newPos = Position;
		
		if (Position.Y - panelRect.Size.Y < 0)
		{
			Position = GetGlobalMousePosition();

		}
		else
		{
			Position = GetGlobalMousePosition() + new Vector2(0, panelRect.Size.Y);

		}

		base._Process(delta);
	}
}