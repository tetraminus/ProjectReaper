using System.Collections.Generic;
using System.Globalization;
using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;

namespace ProjectReaper.Player;

public partial class PlayerHud : Control
{
    public static PackedScene ItemHudPopupScn => GD.Load<PackedScene>("res://Items/ItemHudPopup.tscn");
    public static PackedScene ItemDisplay => GD.Load<PackedScene>("res://Items/ItemDisplay.tscn");
    
    public ItemHudPopup InfoHudPopup;
    public Label FPS;
    public VBoxContainer KeyInventory;
    public DreamCollapseHud DreamCollapseHud;
    
    private const int Numberofdeathquotes = 19;
    [Signal]
    public delegate void FightAnimFinishedEventHandler();
    public override void _Ready()
    {
        
        GameManager.PlayerHud = this;

        InfoHudPopup = ItemHudPopupScn.Instantiate<ItemHudPopup>();
        GetNode<CanvasLayer>("%OverFilterLayer").AddChild(InfoHudPopup);
        
        InfoHudPopup.Visible = false;
        
        
        GetNode<AnimationPlayer>("FightAnimPlayer").Connect(AnimationMixer.SignalName.AnimationFinished, new Callable(this, MethodName.OnFightAnimFinished));
        FPS = GetNode<Label>("FPS");
        
        GetNode<Control>("DeathHud").Hide();
        
        KeyInventory = GetNode<VBoxContainer>("%KeyInventory");
        
        DreamCollapseHud = GetNode<DreamCollapseHud>("DreamCollapseHud");
    }
    
    public void ShowDeathHud()
    {
        GetNode<Control>("DeathHud").Show();
        
        var label = GetNode<RichTextLabel>("%DeathQuote");

        label.Text = "death_" + GD.RandRange(1, Numberofdeathquotes);
        
        
        //label.Show();
        label.VisibleRatio = 0;
        var tween = GetTree().CreateTween();
        tween.TweenProperty(label, "visible_ratio", 1, 1);
        tween.Pause();
        GetTree().CreateTimer(1.5).Timeout += () =>
        {
            tween.Play();
        };
        
    }
    
    public void UpdateKeyInventory(Dictionary<string,int> inventory)
    {
       
        foreach (var child in KeyInventory.GetChildren())
        {
            child.QueueFree();
        }

        var KeyDisplaySettings = new LabelSettings();
        KeyDisplaySettings.FontSize = 70;
        foreach (var key in inventory)
        {
            var keyDisplay = new Label();
            
            keyDisplay.Text = $"{Tr(key.Key)}: {key.Value}";
            keyDisplay.LabelSettings = KeyDisplaySettings;
            
            
            KeyInventory.AddChild(keyDisplay);
        }
        
    }
    
    public void PlayFightAnim()
    {
        var anim = GetNode<AnimationPlayer>("FightAnimPlayer");
        anim.Play("fight");
        
    }
    
    public void OnFightAnimFinished(string animName)
    {
        
        EmitSignal(SignalName.FightAnimFinished);
    }
    

    public void UpdateHealth(float oldHealth, float health)
    {
        GetNode<TextureProgressBar>("%HealthBar").Value = health;
        GetNode<Label>("%HealthBar/Value").Text = Mathf.Round(health).ToString(CultureInfo.InvariantCulture);
    }
    private void UpdateMaxHealth(float i, float statsMaxHealth)
    {
        GetNode<TextureProgressBar>("%HealthBar").MaxValue = statsMaxHealth;
    }

    public override void _Process(double delta)
    {
        if (!GameManager.InRun) return;
        if (GameManager.CurrentRun == null) return;
        FPS.Text = $"{GameManager.CurrentRun.Time:0.00}";
        base._Process(delta);
    }

    public void SetPlayer(Player player)
    {
        player.Stats.HealthChanged += UpdateHealth;
        player.Stats.MaxHealthChanged += UpdateMaxHealth;
        UpdateHealth(0, player.Stats.Health);
        UpdateMaxHealth(0, player.Stats.MaxHealth);
        
    }

    

    public void AddItem(AbstractItem item)
    {
        var itemDisplay = ItemDisplay.Instantiate<ItemDisplay>();
        itemDisplay.FocusMode = FocusModeEnum.None;
        itemDisplay.SetItem(item);
        itemDisplay.MouseEnteredItem += ShowItemInfo;
        itemDisplay.MouseExitedItem += HideItemInfo;
        
        GetNode<HFlowContainer>("%ItemGrid").AddChild(itemDisplay);
    }
    
    public void RemoveItem(AbstractItem item)
    {
        foreach (var child in GetNode<HFlowContainer>("%ItemGrid").GetChildren())
        {
            if (child is ItemDisplay itemDisplay && itemDisplay.Item == item)
            {
                itemDisplay.QueueFree();
                break;
            }
        }
    }

    public void ShowItemInfo(AbstractItem item)
    {
        if (InfoHudPopup.IsQueuedForDeletion()) return;
        InfoHudPopup.SetItem(item);
        InfoHudPopup.Show();
        
    }
    
    public void HideItemInfo()
    {
        if (InfoHudPopup.IsQueuedForDeletion()) return;
        InfoHudPopup.Hide();
    }

    public void ShowInfoHud(string infoTitle, string infoText) {
        
        InfoHudPopup.SetInfo(infoTitle, infoText);
        InfoHudPopup.Show();
    }
    
    public void OnQuitButtonPressed()
    {
        GameManager.GoToMainMenu();
    }

    public void Reset()
    {
        foreach (var child in GetNode<HFlowContainer>("%ItemGrid").GetChildren())
        {
            if (child is ItemDisplay itemDisplay)
            {
                itemDisplay.QueueFree();
            }
        }
        UpdateKeyInventory(new Dictionary<string, int>());
        
        GetNode<Control>("DeathHud").Hide();
    }

    
}

