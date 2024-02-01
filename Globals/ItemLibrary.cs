using Godot;
using Godot.Collections;

namespace ProjectReaper.Globals; 

public partial class ItemLibrary : Node {
    
        public static ItemLibrary Instance { get; private set; }
        
        public Array<AbstractItem> Items { get; set; } = new Array<AbstractItem>();
        
        [Signal]
        public delegate void LoadItemEventHandler(ItemLibrary itemLibrary);
        
        public override void _Ready() {
            Instance = this;
            
            LoadBaseItems();
            
            EmitSignal(SignalName.LoadItem, this);
        }
        
        public void LoadBaseItems() {
            AbstractItem item;
            
            item = new BoomStick();
            item.init();
            item.Stacks = 1;
            Items.Add(item);
            
            
        }
    
}