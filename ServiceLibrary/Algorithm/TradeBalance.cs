using ModelLibrary;

namespace ServiceLibrary.Algorithm
{
    public class TradeBalance
    {
        public bool Calculate(Trade trade)
        {
            if(trade.OfferedAmount < trade.RequestedAmount) return false;
            return true;
        }
    }
}