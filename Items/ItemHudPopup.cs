using Godot;

namespace ProjectReaper.Items;

public partial class ItemHudPopup : Control
{
	public Label NameLabel => GetNode<Label>("%NameLabel");
	public RichTextLabel DescriptionLabel => GetNode<RichTextLabel>("%DescriptionLabel");
	private PanelContainer _panel => GetNode<PanelContainer>("HudPanel");
	private ItemRarity _rarity;
	public override void _Ready()
	{
		base._Ready();
		Visible = false;
	}

	public void SetItem(AbstractItem item)
	{
		NameLabel.Text = item.GetNameKey();
		DescriptionLabel.Text = item.GetDescriptionKey();
		_rarity = item.Rarity;
	}

	public override void _Process(double delta)
	{
		if (!Visible) return;
		if (_rarity != null)
		{
			_panel.SelfModulate = _rarity.Color;
		}
		else {
			_panel.SelfModulate = new Color(1, 1, 1);
		}
		// stay near the mouse, but not on it
		// also stay within the screen bounds
		
		
		var mousePos = GetGlobalMousePosition();
		var screenSize = GetViewportRect().Size;
		var panelSize = _panel.GetRect().Size;
		var offset = new Vector2(20, 20);
		var pos = mousePos + offset;
		if (pos.X + panelSize.X > screenSize.X)
		{
			pos.X = screenSize.X - panelSize.X;
		}

		if (pos.Y + panelSize.Y > screenSize.Y)
		{
			pos.Y = screenSize.Y - panelSize.Y;
		}

		GlobalPosition = pos;
		
		
		base._Process(delta);
	}

	public void SetInfo(string infoTitle, string infoText) {
		NameLabel.Text = infoTitle;
		DescriptionLabel.Text = infoText;
		_rarity = null;
	}
}