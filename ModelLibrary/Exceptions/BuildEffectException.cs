namespace ModelLibrary.Exceptions;

public class BuildEffectException(Trade trade, string message) : OfferDeclinedException(trade, message);