
namespace SpellChecker.Models
{
    public class InputData
    {
        public IReadOnlyList<string> DictionaryWords { get; }

        public IReadOnlyList<string> TextLines { get; }


        public InputData(
            IReadOnlyList<string> dictionaryWords,
            IReadOnlyList<string> textLines)
        {
            DictionaryWords = dictionaryWords;
            TextLines = textLines;
        }
    }
}
