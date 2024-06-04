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
        
        /// <summary>
        /// Calculates if the target tribe should build the requested building themselves.
        /// </summary>
        /// <param name="trade">The proposed trade</param>
        /// <param name="target">The target tribe</param>
        /// <exception cref="SelfBuildException">Thrown when the tribe is better off building it themselves</exception>
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

        /// <summary>
        /// Calculates the counter trade that the target tribe can make to the originator tribe.
        /// </summary>
        /// <param name="trade">The proposed trade</param>
        /// <param name="target">The target tribe</param>
        /// <returns>A new counter offer to the proposed trade</returns>
        /// <exception cref="SelfBuildException">Thrown when Self build</exception>
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