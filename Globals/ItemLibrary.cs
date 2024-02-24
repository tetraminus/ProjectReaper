using Godot;
using Godot.Collections;

namespace ProjectReaper.Globals; 

public partial class ItemLibrary : Node {
        public static ItemLibrary Instance { get; private set; }
        public Dictionary<string, AbstractItem> Items { get; set; } = new ();
        /// <summary>
        /// Event for when the item library is loaded, inject methods here to load items
        /// </summary>
        public delegate void LoadItemEventHandler(ItemLibrary itemLibrary);
        public LoadItemEventHandler LoadItemEvent;
        
        public override void _Ready() {
            Instance = this;
            LoadBaseItems();
            LoadItemEvent?.Invoke(this);
        }
        /// <summary>
        /// Load the base items into the library
        /// </summary>
        public void LoadBaseItems() {
            AbstractItem item;
            
            item = new BoomStick();
            Items.Add(item.ID, item);
            
            item = new DamageDestroyer();
            Items.Add(item.ID, item);
            
        }

        public AbstractItem CreateItem(string id) {
            if (Items.ContainsKey(id)) {
                return Items[id].MakeCopy();
            }
            return null;
        }
}