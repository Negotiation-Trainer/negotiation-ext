using System;
using ModelLibrary;

namespace ServiceLibrary.Algorithm
{
    public class SelfBuild
    {
        public int SelfBuildThreshold { get; set; }
        private readonly Random _random;
        public SelfBuild(Random random)
        {
            SelfBuildThreshold = 5;
            _random = random;
        }
        public bool Calculate(Trade trade, Tribe target)
        {
            if (target.Inventory.GetInventoryAmount(trade.RequestedItem) == SelfBuildThreshold) return (_random.NextDouble() > 0.5f);
            else return target.Inventory.GetInventoryAmount(trade.RequestedItem) < SelfBuildThreshold;
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