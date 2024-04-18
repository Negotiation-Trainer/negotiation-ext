using System;
using ModelLibrary;

namespace ServiceLibrary.Algorithm
{
    public class Usefulness
    {
        private readonly Random _random;

        public Usefulness(Random random)
        {
            _random = random;
        }
        public bool Calculate(Trade trade, Tribe target)
        {
            if (target.Inventory.GetInventoryAmount(trade.OfferedItem) + trade.OfferedAmount > 5) return true;
            if (target.Inventory.GetInventoryAmount(trade.OfferedItem) + trade.OfferedAmount == 5) return _random.NextDouble() > 0.5f;
            return false;
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