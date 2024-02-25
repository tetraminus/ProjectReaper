using System.Globalization;
using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Player;

public partial class PlayerHud : Control
{
    public static PlayerHud Instance { get; private set; }
    public static PackedScene ItemDisplay => GD.Load<PackedScene>("res://Player/ItemDisplay.tscn");
    public Label FPS => GetNode<Label>("FPS");
    public override void _Ready()
    {
        Instance = this;

        GetTree().ProcessFrame += OnProcessFrame;
    }

    public void UpdateHealth(float oldHealth, float health)
    {
        GetNode<Label>("Health/Value").Text = health.ToString();
    }

    public override void _Process(double delta)
    {
        FPS.Text = Engine.GetFramesPerSecond().ToString();
        base._Process(delta);
    }

    public void OnProcessFrame()
    {
        if (GameManager.Player != null)
        {
            GameManager.Player.Stats.HealthChanged += UpdateHealth;
            UpdateHealth(0, GameManager.Player.Stats.Health);
        }

        GetTree().ProcessFrame -= OnProcessFrame;
    }
    
    public void AddItem(AbstractItem item)
    {
        var itemDisplay = ItemDisplay.Instantiate<ItemDisplay>();
        itemDisplay.SetItem(item);
        GetNode<GridContainer>("ItemGrid").AddChild(itemDisplay);
    }
    
    public void RemoveItem(AbstractItem item)
    {
        foreach (var child in GetNode<GridContainer>("ItemGrid").GetChildren())
        {
            if (child is ItemDisplay itemDisplay && itemDisplay.Item == item)
            {
                itemDisplay.QueueFree();
                break;
            }
        }
    }
}

