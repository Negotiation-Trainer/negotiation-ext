namespace ModelLibrary.Exceptions;

public class UsefulnessException(Trade trade, string message) : OfferDeclinedException(trade, message);