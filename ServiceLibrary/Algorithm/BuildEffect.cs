using System;
using System.Collections.Generic;
using System.Linq;
using ModelLibrary;
using ModelLibrary.Exceptions;

namespace ServiceLibrary.Algorithm
{
    public class BuildEffect
    {
        private readonly Random _random;
        
        public BuildEffect(Random random)
        {
            _random = random;
        }
        
        ///Checks if letting the trade originator build the requested building is beneficial
        ///<returns>
        ///true if gain points.
        ///random if neutral.
        ///false if lose points 
        ///</returns>
        public void Calculate(Trade trade, Tribe target, Tribe originator)
        {
            var points = target.PointTable?[(trade.RequestedItem, originator)];
            var tradeShouldBeAccepted = points switch
            {
                5 => true,
                0 => _random.NextDouble() > 0.5f,
                _ => false
            };
            
            if (!tradeShouldBeAccepted)
            {
                throw new BuildEffectException(trade, "This trade has a negative effect on my tribe.");
            }
        }
        
        /// <summary>
        /// Calculates the counter trade that the target tribe can make to the originator tribe.
        /// </summary>
        /// <param name="trade">Trade that was proposed but declined</param>
        /// <param name="target">Target of the trade</param>
        /// <param name="originator">originator of the trade</param>
        /// <returns>A new counter offer to the proposed trade</returns>
        /// <exception cref="BuildEffectException">Thrown when the build effect could not be fixed</exception>
        public Trade CalculateCounter(Trade trade, Tribe target, Tribe originator)
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
                    Calculate(newTrade, target, originator);
                    return newTrade;
                } catch (BuildEffectException)
                {
                    //Ignore
                }
            }

            throw new BuildEffectException(trade, "This trade has a negative effect on my tribe.");
        }
    }
}