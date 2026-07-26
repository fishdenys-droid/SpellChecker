using SpellChecker.Services;

namespace SpellChecker.Tests.Unit
{
    public class DictionaryIndexTests
    {
        [Fact]
        public void Contains_ShouldIgnoreCase()
        {
            var index = new DictionaryIndex(
                new[] { "Hello" });


            Assert.True(index.Contains("hello"));
        }


        [Fact]
        public void Candidates_ShouldReturnNearbyLengths()
        {
            var index = new DictionaryIndex(
                new[]
                {
                "a",
                "rain",
                "plain",
                "mainly",
                "somethingLong"
                });


            var result = index
                .GetCandidates("pain")
                .ToList();


            Assert.Contains("rain", result);
            Assert.Contains("plain", result);
            Assert.Contains("mainly", result);
        }
    }
}