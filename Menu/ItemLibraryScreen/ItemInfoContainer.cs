using Godot;
using ProjectReaper.Items;

namespace ProjectReaper.Menu.ItemLibraryScreen;

public partial class ItemInfoContainer : VBoxContainer
{
    private RichTextLabel _itemDescription;
    private RichTextLabel _itemFlavorText;
    private Label _itemName;
    public AbstractItem Item { get; set; }

    public override void _Ready()
    {
        _itemName = GetNode<Label>("Title");
        _itemFlavorText = GetNode<RichTextLabel>("Flavor");
        _itemDescription = GetNode<RichTextLabel>("Description");
    }

    public void Reset()
    {
        _itemName.Text = "";
        _itemDescription.Text = "";
        _itemFlavorText.Text = "";
    }

    public void SetItem(AbstractItem item)
    {
        Item = item;
        _itemName.Text = item.GetNameKey();
        _itemDescription.Text = item.GetDescriptionKey();
        _itemFlavorText.Text = item.GetFlavorKey();
    }
}