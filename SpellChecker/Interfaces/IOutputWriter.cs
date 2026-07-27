namespace SpellChecker.Interfaces;

public interface IOutputWriter
{
    void Write(string filePath, string content);
}
