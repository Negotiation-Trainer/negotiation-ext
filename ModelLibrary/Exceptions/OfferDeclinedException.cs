using System;

namespace ModelLibrary.Exceptions;

public class OfferDeclinedException(Trade trade, string message) : Exception(message)
{
    public Trade Trade { get; } = trade;
}