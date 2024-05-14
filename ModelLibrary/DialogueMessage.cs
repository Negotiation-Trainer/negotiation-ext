using ModelLibrary.Interfaces;

namespace ModelLibrary
{
    public class DialogueMessage(string tribeName, string message) : IMessage
    {
        public string TribeName { get; private set; } = tribeName;
        public string Message { get; private set; } = message;
    }
}