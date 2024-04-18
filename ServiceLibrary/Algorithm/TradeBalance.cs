using System;
using System.Linq;
using ModelLibrary;

namespace ServiceLibrary.Algorithm
{
    public class TradeBalance
    {
        private readonly Random _random;
        
        public TradeBalance(Random random)
        {
            _random = random;
        }
        
        public bool Calculate(Trade trade, Tribe target, Tribe originator)
        {
            if (!target.GoodWill.Keys.Contains(originator))
            {
                target.GoodWill[originator] = 0;
            }
            int goodWill = target.GoodWill[originator];
            
            if(trade.OfferedAmount + goodWill < trade.RequestedAmount) return false;
            if (trade.OfferedAmount + goodWill == trade.RequestedAmount)
            {
                target.GoodWill[originator] = 0;
                return true;
            }
            int advantage = trade.OfferedAmount + goodWill - trade.RequestedAmount;
            target.GoodWill[originator] = advantage;
            return true;
        }
        
        public Trade CalculateCounter(Trade trade, Tribe target, Tribe originator)
        {
            if (_random.NextDouble() > 0.5f)
            {
                return new Trade(trade.RequestedItem, trade.OfferedAmount, trade.OfferedItem, trade.OfferedAmount);
            }
            return new Trade(trade.RequestedItem, trade.RequestedAmount, trade.OfferedItem, trade.RequestedAmount);
        }
    }
}