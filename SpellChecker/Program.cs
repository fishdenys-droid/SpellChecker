using Microsoft.Extensions.DependencyInjection;
using SpellChecker.Application;
using SpellChecker.Common;
using SpellChecker.Infrastructure;
using SpellChecker.Interfaces;
using SpellChecker.Models;
using SpellChecker.Services;


try
{
    if (args.Length != 2)
    {
        Console.Error.WriteLine(
            "Usage: SpellChecker <input-file> <output-file>");

        return ExitCodes.InvalidArguments;
    }


    var inputFile = args[0];

    var outputFile = args[1];


    var inputReader = new InputReader();


    var inputData =
        inputReader.Read(inputFile);


    var serviceProvider =
        ConfigureServices(inputData);


    var application =
        serviceProvider
            .GetRequiredService<SpellCheckerApplication>();


    application.Run(
        inputData,
        outputFile);


    return ExitCodes.Success;
}
catch (InvalidInputFormatException ex)
{
    Console.Error.WriteLine(ex.Message);

    return ExitCodes.InvalidInputFormat;
}
catch (FileNotFoundException ex)
{
    Console.Error.WriteLine(ex.Message);

    return ExitCodes.GeneralError;
}
catch (DirectoryNotFoundException ex)
{
    Console.Error.WriteLine(ex.Message);

    return ExitCodes.GeneralError;
}
catch (UnauthorizedAccessException ex)
{
    Console.Error.WriteLine(ex.Message);

    return ExitCodes.GeneralError;
}
catch (IOException ex)
{
    Console.Error.WriteLine(ex.Message);

    return ExitCodes.GeneralError;
}
catch (Exception ex)
{
    Console.Error.WriteLine(
        $"Unexpected error: {ex.Message}");

    return ExitCodes.GeneralError;
}



static ServiceProvider ConfigureServices(
    InputData inputData)
{
    var dictionary =
        new DictionaryIndex(
            inputData.DictionaryWords);


    var services =
        new ServiceCollection();


    services.AddSingleton(dictionary);


    services.AddSingleton<IOutputWriter,
                          OutputWriter>();


    services.AddSingleton<ITextTokenizer,
                          ITextTokenizer>();


    services.AddSingleton<IEditDistanceChecker,
                          EditDistanceChecker>();


    services.AddSingleton<ISpellCheckerService,
                          SpellCheckerService>();


    services.AddSingleton<ITextSpellChecker,
                          TextSpellChecker>();


    services.AddSingleton<SpellCheckerApplication>();


    return services.BuildServiceProvider();
}