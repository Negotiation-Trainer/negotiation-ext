using System.Text;
using ModelLibrary;

namespace ServiceLibrary
{
    public class DialogueGenerationService
    {
        public DialogueMessage[] SplitTextToDialogueMessages(string text, string tribeName)
        {
            string textToSplit = text;
            if (!text.Contains("{nm}") && text.Length > 200)
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

        public InstructionMessage[] SplitTextToInstructionMessages(string text)
        {
            string[] messages = text.Split("{nm}");
            InstructionMessage[] instructionMessages = new InstructionMessage[messages.Length];
            for (int i = 0; i < messages.Length; i++)
            {
                instructionMessages[i] = new InstructionMessage(messages[i]);
            }

            return instructionMessages;
        }
        
        private string SplitMessageIntoChunks(string message)
        {
            StringBuilder chunks = new StringBuilder();
            int currentIndex = 0;

            while (currentIndex < message.Length)
            {
                int chunkSize = 200;
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