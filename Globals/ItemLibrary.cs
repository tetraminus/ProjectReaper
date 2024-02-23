using Godot;
using Godot.Collections;

namespace ProjectReaper.Globals; 

public partial class ItemLibrary : Node {
        public static ItemLibrary Instance { get; private set; }
        public Dictionary<AbstractItem, string> Items { get; set; } = new ();
        
        public delegate void LoadItemEventHandler(ItemLibrary itemLibrary);
        public LoadItemEventHandler LoadItemEvent;
        
        public override void _Ready() {
            Instance = this;
            LoadBaseItems();
        }
        
        public void LoadBaseItems() {
            AbstractItem item;
            
            item = new BoomStick();
            Items.Add(item, item.ID);
            
            item = new DamageDestroyer();
            Items.Add(item, item.ID);
            
            
            
        }
    
}