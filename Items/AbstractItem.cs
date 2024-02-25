using Godot;

namespace ProjectReaper.Items;

public abstract partial class AbstractItem : Node2D
{
    
    public delegate int StackChangeEventHandler(AbstractItem item, int stacks);
    private int _stacks { get; set; }
    public int Stacks
    {
        get => _stacks;
        set
        {
            _stacks = value;
            StacksChanged?.Invoke(this, _stacks);
            if (_stacks <= 0)
            {
                QueueFree();
            }
        }
    }
    public event StackChangeEventHandler StacksChanged;
    
    
    public abstract string Id { get; }
    public abstract Texture2D Icon { get; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public void Gain()
    {
        OnInitalPickup();
    }

    public virtual void OnInitalPickup() { }


    public int GetStacks()
    {
        return Stacks;
    }

    public abstract void Init();

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }


    /// <summary>
    ///     Make a copy of the item, used for creating items from the library
    /// </summary>
    /// <returns> A copy of the item </returns>
    public AbstractItem MakeCopy()
    {
        var copy = (AbstractItem)Duplicate();
        return copy;
    }
    
    public string GetLocalizedName()
    {
        return TranslationServer.Translate(GetNameKey());
    }
    public string GetNameKey()
    {
        return "n_" + Id;
    }
    
    public string GetLocalizedDescription()
    {
        return TranslationServer.Translate(GetDescriptionKey());
    }
    public string GetDescriptionKey()
    {
        return "d_" + Id;
    }
    
    public string GetLocalizedFlavor()
    {
        return TranslationServer.Translate(GetFlavorKey());
    }
    public string GetFlavorKey()
    {
        return "f_" + Id;
    }
}