using SpellChecker.Interfaces;

namespace SpellChecker.Services;

public class EditDistanceChecker : IEditDistanceChecker
{
    private const int MaxAllowedEdits = 2;
    private const int DistanceExceedsLimit = MaxAllowedEdits + 1;


    public int GetDistance(
        string source,
        string target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);


        var difference = source.Length - target.Length;


        if (Math.Abs(difference) > MaxAllowedEdits)
        {
            return DistanceExceedsLimit;
        }


        return difference switch
        {
            0 => AreEqual(source, target)
                ? 0
                : IsOneInsertOneDeleteApart(source, target)
                    ? MaxAllowedEdits
                    : DistanceExceedsLimit,


            1 => IsSingleDeletionApart(source, target)
                ? 1
                : DistanceExceedsLimit,


            -1 => IsSingleDeletionApart(target, source)
                ? 1
                : DistanceExceedsLimit,


            2 => IsTwoNonAdjacentDeletionsApart(
                    source,
                    target)
                ? MaxAllowedEdits
                : DistanceExceedsLimit,


            -2 => IsTwoNonAdjacentDeletionsApart(
                    target,
                    source)
                ? MaxAllowedEdits
                : DistanceExceedsLimit,


            _ => DistanceExceedsLimit
        };
    }


    private static bool AreEqual(
        string a,
        string b)
    {
        return string.Equals(
            a,
            b,
            StringComparison.OrdinalIgnoreCase);
    }


    private static bool CharEquals(
        char a,
        char b)
    {
        return char.ToLowerInvariant(a) ==
               char.ToLowerInvariant(b);
    }


    private static bool IsSingleDeletionApart(
        string longer,
        string shorter)
    {
        var i = 0;
        var j = 0;
        var skipped = false;


        while (i < longer.Length &&
               j < shorter.Length)
        {
            if (CharEquals(
                    longer[i],
                    shorter[j]))
            {
                i++;
                j++;
            }
            else if (skipped)
            {
                return false;
            }
            else
            {
                skipped = true;
                i++;
            }
        }


        return true;
    }


    private static bool IsTwoNonAdjacentDeletionsApart(
        string longer,
        string shorter)
    {
        for (var first = 0;
             first < longer.Length;
             first++)
        {
            for (var second = first + 2;
                 second < longer.Length;
                 second++)
            {
                if (MatchesWithoutTwo(
                        longer,
                        first,
                        second,
                        shorter))
                {
                    return true;
                }
            }
        }


        return false;
    }


    private static bool MatchesWithoutTwo(
        string longer,
        int skipA,
        int skipB,
        string shorter)
    {
        var j = 0;


        for (var i = 0;
             i < longer.Length;
             i++)
        {
            if (i == skipA ||
                i == skipB)
            {
                continue;
            }


            if (j >= shorter.Length ||
                !CharEquals(
                    longer[i],
                    shorter[j]))
            {
                return false;
            }


            j++;
        }


        return j == shorter.Length;
    }


    private static bool IsOneInsertOneDeleteApart(
        string a,
        string b)
    {
        var length = a.Length;


        var start = 0;


        while (start < length &&
               CharEquals(
                   a[start],
                   b[start]))
        {
            start++;
        }


        var end = length - 1;


        while (end >= 0 &&
               CharEquals(
                   a[end],
                   b[end]))
        {
            end--;
        }


        var shiftA =
            RegionEquals(
                a,
                start + 1,
                end,
                b,
                start,
                end - 1);


        var shiftB =
            RegionEquals(
                a,
                start,
                end - 1,
                b,
                start + 1,
                end);


        return shiftA || shiftB;
    }


    private static bool RegionEquals(
        string a,
        int aStart,
        int aEnd,
        string b,
        int bStart,
        int bEnd)
    {
        if (aEnd - aStart !=
            bEnd - bStart)
        {
            return false;
        }


        for (var offset = 0;
             aStart + offset <= aEnd;
             offset++)
        {
            if (!CharEquals(
                    a[aStart + offset],
                    b[bStart + offset]))
            {
                return false;
            }
        }


        return true;
    }
}