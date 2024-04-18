using System;

namespace ModelLibrary.Exceptions;

public class UserInputException(string message) : Exception(message);