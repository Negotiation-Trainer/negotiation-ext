using System;
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
            foreach (InventoryItems resource in Enum.GetValues(typeof(InventoryItems)))
            {
                var newTrade = new Trade(trade.RequestedItem, trade.RequestedAmount, resource, trade.OfferedAmount, trade.targetName, trade.originName);

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
        
        public Trade CalculateCounter(Trade trade, Tribe target)
        {
            foreach (InventoryItems resource in Enum.GetValues(typeof(InventoryItems)))
            {
                var newTrade = new Trade(trade.RequestedItem, trade.RequestedAmount, resource, trade.OfferedAmount);
                if (Calculate(newTrade, target)) return newTrade;
            }

            return trade;
        }
    }
}