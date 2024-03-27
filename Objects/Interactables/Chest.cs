using Godot;
using Godot.Collections;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using Key = ProjectReaper.Objects.Key.Key;

namespace ProjectReaper.Interactables;

public partial class Chest : Node2D, IInteractable
{
    
    private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("HighlightComponent/Sprite");
    private bool _opened;
    private Array<AbstractItem> _items = new Array<AbstractItem>();
    private ShaderMaterial _shaderMaterial;
    private const int Items = 1;
    
    
    
    public override void _Ready()
    {
     
        for (var i = 0; i < Items; i++)
        {
            _items.Add(ItemLibrary.Instance.RollItem());
        }
        _opened = false;
        _shaderMaterial = AnimatedSprite2D.Material as ShaderMaterial;
    }
    
    public void Interact()
    {
        if (!_opened)
        {
            _opened = true;
            foreach (var item in _items)
            {
                item.Drop(GlobalPosition);
            }
            AnimatedSprite2D.Play("open");
            GameManager.Player.UseKey(Key.BasicKeyId);
        }
    }
    
    

    public bool CanInteract() {
        return !_opened && GameManager.Player.HasKey(Key.BasicKeyId);
    }

    public string GetPrompt(bool Interactable = false)
    {
        if (_opened)
        {
            return "";
        }
        return Interactable ? Tr("ui_open_chest") : Tr("ui_locked_chest") + 1;
    }
}