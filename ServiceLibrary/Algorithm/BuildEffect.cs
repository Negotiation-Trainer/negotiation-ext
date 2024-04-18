using System;
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
    }
}