using Microsoft.Extensions.DependencyInjection;
using SpellChecker.Application;
using SpellChecker.Infrastructure;
using SpellChecker.Interfaces;
using SpellChecker.Services;


if (args.Length != 2)
{
    Console.WriteLine(
        "Usage: SpellChecker <input-file> <output-file>");

    return;
}


var inputFile = args[0];
var outputFile = args[1];


var inputReader = new InputReader();

var inputData =
    inputReader.Read(inputFile);


var dictionary =
    new DictionaryIndex(
        inputData.DictionaryWords);


var services = new ServiceCollection();


services.AddSingleton(dictionary);

services.AddSingleton<TextTokenizer>();

services.AddSingleton<IEditDistanceChecker,
                      EditDistanceChecker>();

services.AddSingleton<ISpellCheckerService,
                      SpellCheckerService>();

services.AddSingleton<ITextSpellChecker,
                      TextSpellChecker>();

services.AddSingleton<SpellCheckerApplication>();


using var serviceProvider =
    services.BuildServiceProvider();


var application =
    serviceProvider
        .GetRequiredService<SpellCheckerApplication>();


application.Run(
    inputData,
    outputFile);