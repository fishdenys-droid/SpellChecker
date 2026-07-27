using SpellChecker.Infrastructure;
using SpellChecker.Interfaces;
using SpellChecker.Models;
using System.Text;

namespace SpellChecker.Services
{
    public class TextSpellChecker : ITextSpellChecker
    {        
        private readonly ISpellCheckerService _spellChecker;
        private readonly ITextTokenizer _tokenizer;


        public TextSpellChecker(
            ITextTokenizer tokenizer,
            ISpellCheckerService spellChecker)
        {
            _tokenizer = tokenizer;
            _spellChecker = spellChecker;
        }


        public string CheckText(string text)
        {
            var result = new StringBuilder();


            foreach (var token in _tokenizer.Tokenize(text))
            {
                if (token.Type == TokenType.Word)
                {
                    result.Append(
                        _spellChecker.CheckWord(token.Value));
                }
                else
                {
                    result.Append(token.Value);
                }
            }


            return result.ToString();
        }
    }
}
