using SpellChecker.Services;

namespace SpellChecker.Tests.Unit
{
    public class EditDistanceCheckerTests
    {
        private readonly EditDistanceChecker _checker = new();


        [Fact]
        public void SameWords_ShouldReturnZero()
        {
            var result = _checker.GetDistance("hello", "hello");

            Assert.Equal(0, result);
        }


        [Fact]
        public void OneInsertion_ShouldReturnOne()
        {
            var result = _checker.GetDistance("pain", "plain");

            Assert.Equal(1, result);
        }


        [Fact]
        public void OneDeletion_ShouldReturnOne()
        {
            var result = _checker.GetDistance("plain", "pain");

            Assert.Equal(1, result);
        }


        [Fact]
        public void InsertAndDelete_ShouldReturnTwo()
        {
            var result = _checker.GetDistance("hte", "the");

            Assert.Equal(2, result);
        }


        [Fact]
        public void AdjacentDeletes_ShouldBeInvalid()
        {
            var result = _checker.GetDistance("abcdef", "abef");

            Assert.Equal(3, result);
        }


        [Fact]
        public void NonAdjacentDeletes_ShouldBeValid()
        {
            var result = _checker.GetDistance("abcdef", "acef");

            Assert.Equal(2, result);
        }


        [Fact]
        public void MoreThanTwoEdits_ShouldReturnThree()
        {
            var result = _checker.GetDistance("abcdef", "xyz");

            Assert.Equal(3, result);
        }


        [Fact]
        public void MultiplePossibleDeletes_ShouldFindValidPath()
        {
            var result = _checker.GetDistance("aaaa", "aa");

            Assert.Equal(2, result);
        }

        [Fact]
        public void AdjacentDeletes_ShouldNotBeAllowed()
        {
            var checker = new EditDistanceChecker();

            var result =
                checker.GetDistance(
                    "abcdef",
                    "abef");


            Assert.Equal(
                3,
                result);
        }
    }
}
