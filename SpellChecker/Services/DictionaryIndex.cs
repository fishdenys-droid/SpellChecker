namespace SpellChecker.Services
{
    public class DictionaryIndex
    {
        private const int MAX_ALLOWED_EDITS = 2;

        private readonly HashSet<string> _words;
        private readonly List<string> _orderedWords;
        private readonly Dictionary<int, List<string>> _wordsByLength;


        public DictionaryIndex(IEnumerable<string> words)
        {
            _words = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            _orderedWords = new List<string>();

            _wordsByLength = new();


            foreach (var word in words)
            {
                var normalized = word.ToLowerInvariant();

                _words.Add(normalized);

                _orderedWords.Add(word);


                if (!_wordsByLength.TryGetValue(word.Length, out var list))
                {
                    list = new List<string>();
                    _wordsByLength[word.Length] = list;
                }

                list.Add(word);
            }
        }


        public bool Contains(string word)
        {
            return _words.Contains(word.ToLowerInvariant());
        }


        public IEnumerable<string> GetCandidates(string word)
        {
            var minLength = Math.Max(
                0,
                word.Length - MAX_ALLOWED_EDITS);

            var maxLength =
                word.Length + MAX_ALLOWED_EDITS;


            foreach (var candidate in _orderedWords)
            {
                if (candidate.Length >= minLength &&
                    candidate.Length <= maxLength)
                {
                    yield return candidate;
                }
            }
        }
    }
}