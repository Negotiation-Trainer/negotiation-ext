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
            
            switch (amountInInventory)
            {
                case < 5:
                    throw new UsefulnessException(trade, "This trade is not useful for me.");
                case 5 when random.NextDouble() > 0.5f:
                    throw new UsefulnessException(trade, "This trade is not useful for me.");
            }
        }
    }
}