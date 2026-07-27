using SpellChecker.Interfaces;

namespace SpellChecker.Infrastructure;

public class OutputWriter : IOutputWriter
{
    public void Write(
        string filePath,
        string content)
    {
        File.WriteAllText(
            filePath,
            content);
    }
}