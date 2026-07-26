using SpellChecker.Application;
using SpellChecker.Infrastructure;
using SpellChecker.Models;
using SpellChecker.Services;

namespace SpellChecker.Tests.Integration;

public class SpellCheckerApplicationTests
{
    [Fact]
    public void ShouldProduceExpectedOutputForExampleInput()
    {
        var inputFile = Path.GetTempFileName();
        var outputFile = Path.GetTempFileName();

        File.WriteAllText(
            inputFile,
            """
            rain spain plain plaint pain main mainly
            the in on fall falls his was
            ===
            hte rame in pain fells
            mainy oon teh lain
            was hints pliant
            ===
            """);

        var dictionary = new DictionaryIndex(
            new[]
            {
                "rain",
                "spain",
                "plain",
                "plaint",
                "pain",
                "main",
                "mainly",
                "the",
                "in",
                "on",
                "fall",
                "falls",
                "his",
                "was"
            });

        var application = CreateApplication(dictionary);

        application.Run(
            ReadInput(inputFile),
            outputFile);

        var result = File.ReadAllText(outputFile);

        Assert.Equal(
            $"the {{rame?}} in pain falls{Environment.NewLine}" +
            $"{{main mainly}} on the plain{Environment.NewLine}" +
            $"was {{hints?}} plaint{Environment.NewLine}",
            result);
    }

    [Fact]
    public void ShouldKeepDictionaryCaseInCorrection()
    {
        var dictionary = new DictionaryIndex(
            new[]
            {
                "Hello"
            });

        var application = CreateApplication(dictionary);

        var inputFile = Path.GetTempFileName();
        var outputFile = Path.GetTempFileName();

        File.WriteAllText(
            inputFile,
            $"==={Environment.NewLine}" +
            $"Helo{Environment.NewLine}" +
            $"==={Environment.NewLine}");

        application.Run(
            ReadInput(inputFile),
            outputFile);

        var result = File.ReadAllText(outputFile);

        Assert.Equal(
            $"Hello{Environment.NewLine}",
            result);
    }

    [Fact]
    public void ShouldPreserveEmptyLines()
    {
        var dictionary = new DictionaryIndex(
            new[]
            {
                "test"
            });

        var application = CreateApplication(dictionary);

        var inputFile = Path.GetTempFileName();
        var outputFile = Path.GetTempFileName();

        File.WriteAllText(
            inputFile,
            $"test{Environment.NewLine}" +
            $"==={Environment.NewLine}" +
            $"test{Environment.NewLine}" +
            $"{Environment.NewLine}" +
            $"test{Environment.NewLine}" +
            $"==={Environment.NewLine}");

        application.Run(
            ReadInput(inputFile),
            outputFile);

        var result = File.ReadAllText(outputFile);

        Assert.Equal(
            $"test{Environment.NewLine}" +
            $"{Environment.NewLine}" +
            $"test{Environment.NewLine}",
            result);
    }

    [Fact]
    public void ShouldPreserveMultipleSpaces()
    {
        var dictionary = new DictionaryIndex(
            new[]
            {
                "test"
            });

        var application = CreateApplication(dictionary);

        var inputFile = Path.GetTempFileName();
        var outputFile = Path.GetTempFileName();

        File.WriteAllText(
            inputFile,
            $"==={Environment.NewLine}" +
            $"test     test{Environment.NewLine}" +
            $"==={Environment.NewLine}");

        application.Run(
            ReadInput(inputFile),
            outputFile);

        var result = File.ReadAllText(outputFile);

        Assert.Equal(
            $"test     test{Environment.NewLine}",
            result);
    }

    [Fact]
    public void ShouldMarkAllWordsAsUnknownWhenDictionaryIsEmpty()
    {
        var dictionary = new DictionaryIndex(
            Array.Empty<string>());

        var application = CreateApplication(dictionary);

        var inputFile = Path.GetTempFileName();
        var outputFile = Path.GetTempFileName();

        File.WriteAllText(
            inputFile,
            $"==={Environment.NewLine}" +
            $"hello world{Environment.NewLine}" +
            $"==={Environment.NewLine}");

        application.Run(
            ReadInput(inputFile),
            outputFile);

        var result = File.ReadAllText(outputFile);

        Assert.Equal(
            $"{{hello?}} {{world?}}{Environment.NewLine}",
            result);
    }

    [Fact]
    public void ShouldProcessLongWordWithoutException()
    {
        var dictionary = new DictionaryIndex(
            new[]
            {
                "test"
            });

        var application = CreateApplication(dictionary);

        var longWord = new string('a', 50);

        var inputFile = Path.GetTempFileName();
        var outputFile = Path.GetTempFileName();

        File.WriteAllText(
            inputFile,
            $"test{Environment.NewLine}" +
            $"==={Environment.NewLine}" +
            $"{longWord}{Environment.NewLine}" +
            $"==={Environment.NewLine}");

        var exception = Record.Exception(() =>
            application.Run(
                ReadInput(inputFile),
                outputFile));

        Assert.Null(exception);

        var result = File.ReadAllText(outputFile);

        Assert.Equal(
            $"{{{longWord}?}}{Environment.NewLine}",
            result);
    }

    [Fact]
    public void ShouldPreserveTabs()
    {
        var dictionary = new DictionaryIndex(
            new[]
            {
                "test",
                "word"
            });

        var application = CreateApplication(dictionary);

        var inputFile = Path.GetTempFileName();
        var outputFile = Path.GetTempFileName();

        File.WriteAllText(
            inputFile,
            $"==={Environment.NewLine}" +
            $"test\t\tword{Environment.NewLine}" +
            $"==={Environment.NewLine}");

        application.Run(
            ReadInput(inputFile),
            outputFile);

        var result = File.ReadAllText(outputFile);

        Assert.Equal(
            $"test\t\tword{Environment.NewLine}",
            result);
    }

    [Fact]
    public void ShouldPreserveOriginalCaseWhenWordExistsInDictionary()
    {
        var dictionary = new DictionaryIndex(
            new[]
            {
                "Hello"
            });

        var application = CreateApplication(dictionary);

        var inputFile = Path.GetTempFileName();
        var outputFile = Path.GetTempFileName();

        File.WriteAllText(
            inputFile,
            $"==={Environment.NewLine}" +
            $"HELLO{Environment.NewLine}" +
            $"==={Environment.NewLine}");

        application.Run(
            ReadInput(inputFile),
            outputFile);

        var result = File.ReadAllText(outputFile);

        Assert.Equal(
            $"HELLO{Environment.NewLine}",
            result);
    }

    private static InputData ReadInput(string inputFile)
    {
        return new InputReader().Read(inputFile);
    }

    private static SpellCheckerApplication CreateApplication(
        DictionaryIndex dictionary)
    {
        var distanceChecker = new EditDistanceChecker();

        var spellChecker = new SpellCheckerService(
            dictionary,
            distanceChecker);

        var textChecker = new TextSpellChecker(
            new TextTokenizer(),
            spellChecker);

        return new SpellCheckerApplication(
            textChecker);
    }
}