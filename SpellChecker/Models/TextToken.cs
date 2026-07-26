
namespace SpellChecker.Models
{
    public enum TokenType
    {
        Word,
        Whitespace
    }


    public record TextToken(
        TokenType Type,
        string Value);
}
