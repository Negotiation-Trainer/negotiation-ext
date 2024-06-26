using System;
using System.Collections.Generic;

namespace ModelLibrary
{
    public class Inventory
    {
        public event EventHandler? InventoryUpdate;
        private readonly Dictionary<InventoryItems, int> _inventory = new();


        public Inventory()
        {
            foreach (InventoryItems item in Enum.GetValues(typeof(InventoryItems)))
            {
                _inventory.Add(item, 0);
            }
        }
        
        /// <summary>
        /// Get the amount of an item in the inventory
        /// </summary>
        /// <param name="item">The Inventory Item</param>
        /// <returns></returns>
        public int GetInventoryAmount(InventoryItems item)
        {
            return _inventory[item];
        }

        /// <summary>
        /// x
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void AddToInventory(InventoryItems item, int amount)
        {
            _inventory[item] += amount;
            InventoryUpdate?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Remove a set amount of a item in the inventory.
        /// </summary>
        /// <param name="item">The inventory item</param>
        /// <param name="amount">The amount to remove</param>
        public void RemoveFromInventory(InventoryItems item, int amount)
        {
            if(_inventory[item] == 0) return;
            _inventory[item] -= amount;
            InventoryUpdate?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// invoke the inventory update event
        /// </summary>
        public void UpdateInventory()
        {
            InventoryUpdate?.Invoke(this,EventArgs.Empty);
        }

        public new string ToString()
        {
            return $"Wood: {_inventory[InventoryItems.Wood]} Lenses: {_inventory[InventoryItems.Lenses]} Clay: {_inventory[InventoryItems.Clay]} Gold: {_inventory[InventoryItems.Gold]} Steel: {_inventory[InventoryItems.Steel]} Insulation: {_inventory[InventoryItems.Insulation]} Fertilizer: {_inventory[InventoryItems.Fertilizer]} Stone: {_inventory[InventoryItems.Stone]}";
        }
    }
}