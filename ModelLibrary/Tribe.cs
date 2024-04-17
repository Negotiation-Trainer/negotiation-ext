using System.Collections.Generic;

namespace ModelLibrary
{
    public class Tribe(string name)
    {
        public readonly Inventory Inventory = new();
        public string Name { get; private set; } = name;
        public int Points { get; set; } = 0;
        public Dictionary<(InventoryItems, Tribe), int>? PointTable { get; set; }
        public Dictionary<Tribe, int>? GoodWill { get; set; }
    }
}