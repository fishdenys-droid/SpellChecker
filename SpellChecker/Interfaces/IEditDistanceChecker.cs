
namespace SpellChecker.Interfaces
{
    public interface IEditDistanceChecker
    {
        int GetDistance(string source, string target);
    }
}
