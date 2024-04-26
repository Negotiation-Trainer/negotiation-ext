namespace ModelLibrary.Exceptions;

public class InsufficientResourcesException(Trade trade, int shortAmount, string message) : OfferDeclinedException(trade, message)
{
    public int ShortAmount { get; } = shortAmount;
}