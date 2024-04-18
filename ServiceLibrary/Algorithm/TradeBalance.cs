using System.Linq;
using ModelLibrary;
using ModelLibrary.Exceptions;

namespace ServiceLibrary.Algorithm
{
    public class TradeBalance
    {
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
            }
            int advantage = trade.OfferedAmount + goodWill - trade.RequestedAmount;
            target.GoodWill[originator] = advantage;
        }
    }
}