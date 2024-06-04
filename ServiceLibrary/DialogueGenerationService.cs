using System.Text;
using ModelLibrary;

namespace ServiceLibrary
{
    public class DialogueGenerationService
    {
        private readonly int _maxMessageLength = 200;
        
        /// <summary>
        /// Splits the text into chunks of dialogue messages if the text exceed the maximum length of 200 characters.
        /// </summary>
        /// <param name="text">The full text that needs to be displayed</param>
        /// <param name="tribeName">The name of the tribe speaking</param>
        /// <returns>Returns a array of dialogue message that can be displayed</returns>
        public DialogueMessage[] SplitTextToDialogueMessages(string text, string tribeName)
        {
            string textToSplit = text;
            
            // If the text contains the {nm} separator, we don't need to add it
            if (!text.Contains("{nm}") && text.Length > _maxMessageLength)
            {
                textToSplit = SplitMessageIntoChunks(text);
            }
            
            string[] messages = textToSplit.Split("{nm}");
            DialogueMessage[] dialogueMessages = new DialogueMessage[messages.Length];
            for (int i = 0; i < messages.Length; i++)
            {
                dialogueMessages[i] = new DialogueMessage(tribeName, messages[i]);
            }

            return dialogueMessages;
        }

        /// <summary>
        /// Split the text into chunks of instruction messages if the text exceed the maximum length of 200 characters.
        /// </summary>
        /// <param name="text">The full instruction text that needs to be split up in different messages</param>
        /// <returns>Returns a array of the instruction messages that can be displayed</returns>
        public InstructionMessage[] SplitTextToInstructionMessages(string text)
        {
            string textToSplit = text;
            
            // If the text contains the {nm} separator, we don't need to add it
            if (!text.Contains("{nm}") && text.Length > _maxMessageLength)
            {
                textToSplit = SplitMessageIntoChunks(text);
            }
            
            string[] messages = textToSplit.Split("{nm}");
            InstructionMessage[] instructionMessages = new InstructionMessage[messages.Length];
            for (int i = 0; i < messages.Length; i++)
            {
                instructionMessages[i] = new InstructionMessage(messages[i]);
            }

            return instructionMessages;
        }
        
        // Splits the messages into chunks of the maximum message length
        private string SplitMessageIntoChunks(string message)
        {
            StringBuilder chunks = new StringBuilder();
            int currentIndex = 0;

            while (currentIndex < message.Length)
            {
                int chunkSize = _maxMessageLength;
                if (currentIndex + chunkSize > message.Length)
                {
                    chunkSize = message.Length - currentIndex;
                }
                else
                {
                    while (message[currentIndex + chunkSize] != ' ' && chunkSize > 0)
                    {
                        chunkSize--;
                    }
                }

                chunks.Append(message.Substring(currentIndex, chunkSize));
                currentIndex += chunkSize;

                // Add the separator if there is more text to process
                if (currentIndex < message.Length)
                {
                    chunks.Append("{nm}");
                }
            }

            return chunks.ToString();
        }
    }
}