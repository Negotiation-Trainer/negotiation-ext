namespace ModelLibrary.Exceptions;

public class TradeBalanceException(Trade trade, int goodWill, string message)
    : OfferDeclinedException(trade, message)
{
    public int GoodWill { get; } = goodWill;
}