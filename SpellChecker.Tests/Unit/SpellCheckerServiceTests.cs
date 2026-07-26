
using SpellChecker.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpellChecker.Tests.Unit
{
    public class SpellCheckerServiceTests
    {
        [Fact]
        public void ExistingWord_ShouldRemainUnchanged()
        {
            var dictionary = new DictionaryIndex(
                new[]
                {
            "Hello"
                });


            var service =
                new SpellCheckerService(
                    dictionary,
                    new EditDistanceChecker());


            var result =
                service.CheckWord("HELLO");


            Assert.Equal(
                "HELLO",
                result);
        }
    }
}
