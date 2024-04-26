using System;
using System.Collections.Generic;
using System.Linq;
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

            if (amountInInventory > SelfBuildThreshold)
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
            InventoryItems[] excludedItems = { trade.OfferedItem };
            List<InventoryItems> resources = Enum.GetValues(typeof(InventoryItems))
                .Cast<InventoryItems>()
                .Where(item => !excludedItems.Contains(item))
                .ToList();
            foreach (InventoryItems resource in resources)
            {
                var newTrade = new Trade(resource, trade.RequestedAmount, trade.OfferedItem, trade.OfferedAmount, trade.targetName, trade.originName);
                try
                {
                    Calculate(newTrade, target);
                    return newTrade;
                } catch (SelfBuildException)
                {
                    //Ignore
                }
            }

            throw new SelfBuildException(trade, "I Want to build the build myself.");
        }
    }

   
}