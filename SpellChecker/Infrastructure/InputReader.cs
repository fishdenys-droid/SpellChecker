using SpellChecker.Models;

namespace SpellChecker.Infrastructure
{
    public class InputReader
    {
        public InputData Read(string filePath)
        {
            var dictionaryWords = new List<string>();
            var textLines = new List<string>();

            var dictionaryPart = true;


            foreach (var line in File.ReadLines(filePath))
            {
                if (line == "===")
                {
                    dictionaryPart = false;
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


            return new InputData(
                dictionaryWords,
                textLines);
        }
    }
}

