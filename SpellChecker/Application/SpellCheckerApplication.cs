using SpellChecker.Interfaces;
using SpellChecker.Models;
using System.Text;

namespace SpellChecker.Application;

public class SpellCheckerApplication
{
    private readonly ITextSpellChecker _spellChecker;


    public SpellCheckerApplication(
        ITextSpellChecker spellChecker)
    {
        _spellChecker = spellChecker;
    }


    public void Run(
        InputData inputData,
        string outputFile)
    {
        var result = new StringBuilder();


        foreach (var line in inputData.TextLines)
        {
            result.Append(
        _spellChecker.CheckText(line));

            result.AppendLine();
        }


        File.WriteAllText(
            outputFile,
            result.ToString());
    }
}