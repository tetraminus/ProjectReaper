using Godot;
using ProjectReaper.Globals;
using ProjectReaper.Items;
using Key = ProjectReaper.Objects.Key.Key;

namespace ProjectReaper.Interactables;

public partial class Chest : Node2D, IInteractable
{
    
    private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("HighlightComponent/Sprite");
    private bool _opened;
    private AbstractItem _item;
    private ShaderMaterial _shaderMaterial;
    
    
    
    public override void _Ready()
    {
     
        _item = ItemLibrary.Instance.RollItem();
        _opened = false;
        _shaderMaterial = AnimatedSprite2D.Material as ShaderMaterial;
    }
    
    public void Interact()
    {
        if (!_opened)
        {
            _opened = true;
            _item.Drop(GlobalPosition);
            AnimatedSprite2D.Play("open");
            GameManager.Player.UseKey(Key.BasicKeyId);
        }
    }

    public bool CanInteract() {
        return !_opened && GameManager.Player.HasKey(Key.BasicKeyId);
    }
}