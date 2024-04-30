using System;
using System.Linq;
using ModelLibrary;
using ModelLibrary.Exceptions;

namespace ServiceLibrary.Algorithm
{
    public class TradeBalance
    {
        private readonly Random _random;
        
        public TradeBalance(Random random)
        {
            _random = random;
        }
        
        public void Calculate(Trade trade, Tribe target, Tribe originator)
        {
            if (!target.GoodWill.Keys.Contains(originator))
            {
                target.GoodWill[originator] = 0;
            }
            int goodWill = target.GoodWill[originator];
            
            if(trade.OfferedAmount + goodWill < trade.RequestedAmount) throw new TradeBalanceException(trade, goodWill,"Trade is not balanced.");
            if (trade.OfferedAmount + goodWill == trade.RequestedAmount)
            {
                target.GoodWill[originator] = 0;
                return;
            }
            int advantage = trade.OfferedAmount + goodWill - trade.RequestedAmount;
            target.GoodWill[originator] = advantage;
        }
        
        public Trade CalculateCounter(Trade trade)
        {
            return _random.NextDouble() > 0.5f ? 
                new Trade(trade.RequestedItem, trade.OfferedAmount, trade.OfferedItem, trade.OfferedAmount) 
                : new Trade(trade.RequestedItem, trade.RequestedAmount, trade.OfferedItem, trade.RequestedAmount);
            
        }
    }
}