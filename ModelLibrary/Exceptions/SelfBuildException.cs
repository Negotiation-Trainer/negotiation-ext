using System;

namespace ModelLibrary.Exceptions;

public class SelfBuildException(Trade trade, string message)
    : OfferDeclinedException(trade, message);