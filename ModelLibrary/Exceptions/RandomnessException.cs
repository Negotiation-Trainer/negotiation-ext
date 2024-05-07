namespace ModelLibrary.Exceptions;

public class RandomnessException(Trade trade, string message) : OfferDeclinedException(trade, message);