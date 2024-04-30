using System;
using System.Collections.Generic;
using System.Linq;
using ModelLibrary;
using ModelLibrary.Exceptions;

namespace ServiceLibrary.Algorithm
{
    public class Usefulness(Random random)
    {
        public void Calculate(Trade trade, Tribe target)
        {
            var amountInInventory = target.Inventory.GetInventoryAmount(trade.OfferedItem);
            
            switch (amountInInventory + trade.OfferedAmount)
            {
                case < 5:
                    throw new UsefulnessException(trade, "This trade is not useful for me.");
                case 5 when random.NextDouble() > 0.5f:
                    throw new UsefulnessException(trade, "This trade is not useful for me.");
            }
        }
        
        public Trade CalculateCounter(Trade trade, Tribe target)
        {
            InventoryItems[] excludedItems = { trade.RequestedItem };
            List<InventoryItems> resources = Enum.GetValues(typeof(InventoryItems))
                .Cast<InventoryItems>()
                .Where(item => !excludedItems.Contains(item))
                .ToList();
            
            foreach (InventoryItems resource in resources)
            {
                var newTrade = new Trade(trade.RequestedItem, trade.RequestedAmount, resource, trade.OfferedAmount, target.Name, trade.originName);

                try
                {
                    Calculate(newTrade, target);
                    return newTrade;
                } catch (UsefulnessException)
                {
                    //Ignore
                }
            }

            throw new UsefulnessException(trade, "This trade is not useful for me.");
        }
    }
}