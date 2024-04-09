using ServiceLibrary;

namespace ServiceLibraryTest
{
    public class DialogueGenerationServiceTests
    {
        [Test]
        public void SplitTextToDialogueMessages_OneSentence_ReturnsDialogueMessage()
        {
            //Given
            string testText = "This is a sentence";
            DialogueGenerationService dialogueGenerationService = new DialogueGenerationService();
            
            //When
            var result= dialogueGenerationService.SplitTextToDialogueMessages(testText, 1);
            
            //Then
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Message, Is.EqualTo(testText));
            Assert.That(result[0].TribeId, Is.EqualTo(1));
        }
        
        [Test]
        public void SplitTextToDialogueMessages_MultipleSentence_ReturnsDialogueMessages()
        {
            //Given
            string testText = "This is one sentence {nm} This is second sentence {nm} and a third sentence";
            DialogueGenerationService dialogueGenerationService = new DialogueGenerationService();
            
            //When
            var result= dialogueGenerationService.SplitTextToDialogueMessages(testText, 1);
            
            //Then
            Assert.That(result.Length, Is.EqualTo(3));
        }
        
        [Test]
        public void SplitTextToInstructionMessages_OneSentence_ReturnsInstructionMessage()
        {
            //Given
            string testText = "This is a sentence";
            DialogueGenerationService dialogueGenerationService = new DialogueGenerationService();
            
            //When
            var result= dialogueGenerationService.SplitTextToInstructionMessages(testText);
            
            //Then
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Message, Is.EqualTo(testText));
        }
        
        [Test]
        public void SplitTextToInstructionMessages_MultipleSentence_ReturnsInstructionMessages()
        {
            //Given
            string testText = "This is one sentence {nm} This is second sentence {nm} and a third sentence";
            DialogueGenerationService dialogueGenerationService = new DialogueGenerationService();
            
            //When
            var result= dialogueGenerationService.SplitTextToInstructionMessages(testText);
            
            //Then
            Assert.That(result.Length, Is.EqualTo(3));
        }
    }
}
