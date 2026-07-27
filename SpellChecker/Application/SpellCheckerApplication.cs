using SpellChecker.Interfaces;
using SpellChecker.Models;
using System.Text;

namespace SpellChecker.Application;

public class SpellCheckerApplication
{
    private readonly ITextSpellChecker _spellChecker;

    private readonly IOutputWriter _outputWriter;


    public SpellCheckerApplication(
        ITextSpellChecker spellChecker,
        IOutputWriter outputWriter)
    {
        _spellChecker = spellChecker;
        _outputWriter = outputWriter;
    }


    public void Run(
        InputData inputData,
        string outputFile)
    {
        var result = new StringBuilder();


        foreach (var line in inputData.TextLines)
        {
            result.AppendLine(
                _spellChecker.CheckText(line));
        }


        _outputWriter.Write(
            outputFile,
            result.ToString());
    }
}