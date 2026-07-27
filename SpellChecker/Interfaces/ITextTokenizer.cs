using SpellChecker.Models;

namespace SpellChecker.Interfaces;

public interface ITextTokenizer
{
    IEnumerable<TextToken> Tokenize(string text);
}