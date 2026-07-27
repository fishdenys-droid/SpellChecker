using SpellChecker.Common;
using SpellChecker.Interfaces;
using SpellChecker.Models;

namespace SpellChecker.Infrastructure
{
    public class InputReader
    {

        private const string SectionSeparator = "===";

        public InputData Read(string filePath)
        {
            var dictionaryWords = new List<string>();
            var textLines = new List<string>();
            var sawSeparator = false;

            var dictionaryPart = true;


            foreach (var line in File.ReadLines(filePath))
            {
                if (line.Trim() == SectionSeparator)
                {
                    dictionaryPart = false;
                    sawSeparator = true;
                    continue;
                }

                if (dictionaryPart)
                {
                    dictionaryWords.AddRange(
                        line.Split(
                            ' ',
                            StringSplitOptions.RemoveEmptyEntries));
                }
                else
                {
                    textLines.Add(line);
                }
            }

            if (!sawSeparator)
            {
                throw new InvalidInputFormatException(
                    "Input is missing the '===' separator between the dictionary and the text section.");
            }

            return new InputData(
                dictionaryWords,
                textLines);
        }
    }
}

