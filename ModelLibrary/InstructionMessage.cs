using ModelLibrary.Interfaces;

namespace ModelLibrary
{
    public class InstructionMessage(string message) : IMessage
    {
        public string Message { get; private set; } = message;
    }
}