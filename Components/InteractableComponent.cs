using Godot;
using ProjectReaper.Globals;

namespace ProjectReaper.Interactables
{
    public partial class InteractableComponent : Node2D
    {
        private Area2D Area2D => GetNode<Area2D>("Area2D");
        private TextureRect TextureRect => GetNode<TextureRect>("PromptPivot/Prompt");
        private Node2D PromptPivot => GetNode<Node2D>("PromptPivot");
        private bool _playerInRadius;
        
        [Export] public Node2D Interactable { get; set; }
        [Export] public CanvasGroup Highlight { get; set; }
        private IInteractable _interactable;
        private HighlightComponent _highlightComponent;

        public override void _Ready()
        {
            PromptPivot.GlobalRotation = 0;
            if (Interactable is IInteractable interactable)
            {
                _interactable = interactable;
            }
            else if (Interactable != null)
            {
                GD.PrintErr("Interactable is not of type IInteractable");
            }
            
            if (Highlight is HighlightComponent highlightComponent)
            {
                _highlightComponent = highlightComponent;
            }
            else if (Highlight != null)
            {
                GD.PrintErr("Highlight is not of type HighlightComponent");
            }
            Area2D.BodyEntered += OnBodyEntered;
            Area2D.BodyExited += OnBodyExited;
            
            TextureRect.Hide();
        }
        
        public void Enable()
        {
            Area2D.Monitoring = true;
        }
        
        public void Disable()
        {
            Area2D.Monitoring = false;
        }

        private void OnBodyEntered(Node body)
        {
            if (body == GameManager.Player)
            {
                _playerInRadius = true;
                
            }
        }

        private void OnBodyExited(Node body)
        {
            if (body == GameManager.Player)
            {
                _playerInRadius = false;
                
            }
        }

        public override void _Process(double delta)
        {
            
            if (_playerInRadius  && _interactable.CanInteract())
            {
                if (_highlightComponent != null && !_highlightComponent.IsOn())
                {
                    _highlightComponent.Enable();
                    TextureRect.Show();
                }
                
                if (Input.IsActionJustPressed("interact"))
                {
                    Interact();
                }
                
            } else if (_highlightComponent != null && _highlightComponent.IsOn())
            {
                    _highlightComponent.Disable();
                    TextureRect.Hide();
            }
            
        }

        protected virtual void Interact()
        {
            _interactable.Interact();
        }
    }
}

public interface IInteractable
{
    void Interact();
    bool CanInteract();
}