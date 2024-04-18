using System;
using ModelLibrary;
using ModelLibrary.Exceptions;

namespace ServiceLibrary.Algorithm
{
    public class SelfBuild
    {
        private int SelfBuildThreshold { get; }
        private readonly Random _random;
        public SelfBuild(Random random)
        {
            SelfBuildThreshold = 5;
            _random = random;
        }
        public void Calculate(Trade trade, Tribe target)
        {
            
            var amountInInventory = target.Inventory.GetInventoryAmount(trade.RequestedItem);

            if (amountInInventory < SelfBuildThreshold)
            {
                throw new SelfBuildException(trade, "I Want to build the build myself.");
            }

            if (amountInInventory == SelfBuildThreshold && _random.NextDouble() > 0.5f)
            {
                throw new SelfBuildException(trade, "I Want to build the build myself.");
            }
        }

        public Trade CalculateCounter(Trade trade, Tribe target)
        {
            foreach (InventoryItems resource in Enum.GetValues(typeof(InventoryItems)))
            {
                var newTrade = new Trade(resource, trade.RequestedAmount, trade.OfferedItem, trade.OfferedAmount);
                if (Calculate(newTrade, target)) return newTrade;
            }

            return trade;
        }
    }

   
}