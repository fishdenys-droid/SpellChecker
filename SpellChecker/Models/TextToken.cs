namespace SpellChecker.Models;

public class TextToken
{
    public TokenType Type { get; }

    public string Value { get; }


    public TextToken(
        TokenType type,
        string value)
    {
        Type = type;
        Value = value;
    }
}