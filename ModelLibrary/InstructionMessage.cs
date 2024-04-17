using ModelLibrary.Interfaces;

namespace ModelLibrary
{
    public class InstructionMessage : IMessage
    {
        public string Message { get; private set; }

        public InstructionMessage(string message)
        {
            Message = message;
        }
    }
}