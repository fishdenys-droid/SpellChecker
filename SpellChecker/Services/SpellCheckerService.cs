using SpellChecker.Interfaces;

namespace SpellChecker.Services
{
    public class SpellCheckerService : ISpellCheckerService
    {
        private readonly DictionaryIndex _dictionary;
        private readonly IEditDistanceChecker _distanceChecker;


        public SpellCheckerService(
            DictionaryIndex dictionary,
            IEditDistanceChecker distanceChecker)
        {
            _dictionary = dictionary;
            _distanceChecker = distanceChecker;
        }


        public string CheckWord(string word)
        {
            if (_dictionary.Contains(word))
            {
                return word;
            }


            var oneEdit = new List<string>();
            var twoEdits = new List<string>();


            foreach (var candidate in _dictionary.GetCandidates(word))
            {
                var distance = _distanceChecker.GetDistance(
                    word.ToLowerInvariant(),
                    candidate.ToLowerInvariant());


                if (distance == 1)
                {
                    oneEdit.Add(candidate);
                }
                else if (distance == 2)
                {
                    twoEdits.Add(candidate);
                }
            }


            if (oneEdit.Count > 0)
            {
                return FormatResult(oneEdit, word);
            }


            if (twoEdits.Count > 0)
            {
                return FormatResult(twoEdits, word);
            }


            return $"{{{word}?}}";
        }


        private string FormatResult(
            List<string> corrections,
            string original)
        {
            if (corrections.Count == 1)
            {
                return corrections[0];
            }


            return $"{{{string.Join(" ", corrections)}}}";
        }
    }
}
