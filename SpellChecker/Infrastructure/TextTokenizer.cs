using SpellChecker.Interfaces;
using SpellChecker.Models;
using System.Text;

namespace SpellChecker.Infrastructure
{
    public class TextTokenizer: ITextTokenizer
    {
        public IEnumerable<TextToken> Tokenize(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;


            var buffer = new StringBuilder();

            var currentIsLetter =
                char.IsLetter(text[0]);


            foreach (var ch in text)
            {
                var isLetter = char.IsLetter(ch);


                if (isLetter == currentIsLetter)
                {
                    buffer.Append(ch);
                }
                else
                {
                    yield return CreateToken(
                        currentIsLetter,
                        buffer.ToString());

                    buffer.Clear();

                    buffer.Append(ch);

                    currentIsLetter = isLetter;
                }
            }


            if (buffer.Length > 0)
            {
                yield return CreateToken(
                    currentIsLetter,
                    buffer.ToString());
            }
        }


        private static TextToken CreateToken(
            bool isWord,
            string value)
        {
            return new TextToken(
                isWord
                    ? TokenType.Word
                    : TokenType.Whitespace,
                value);
        }
    }
}
