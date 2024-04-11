using System.Linq;
using Models;

namespace ServiceLibrary.Algorithm
{
    public class TradeBalance
    {
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
    }
}