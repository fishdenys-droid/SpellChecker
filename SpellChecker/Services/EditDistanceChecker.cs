using SpellChecker.Interfaces;
using SpellChecker.Models;

namespace SpellChecker.Services;

public class EditDistanceChecker : IEditDistanceChecker
{
    private const int MAX_ALLOWED_EDITS = 2;
    private const int DISTANCE_EXCEEDS_LIMIT = MAX_ALLOWED_EDITS + 1;


    public int GetDistance(string source, string target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);

        var state = new EditDistanceState(
            source,
            target,
            0,
            0,
            0,
            EditOperation.None,
            -1,
            -1);

        return Search(state);
    }


    private int Search(EditDistanceState context)
    {
        if (context.Edits > MAX_ALLOWED_EDITS)
            return DISTANCE_EXCEEDS_LIMIT;


        if (IsFinished(context))
            return context.Edits;


        if (IsImpossible(context))
            return DISTANCE_EXCEEDS_LIMIT;


        if (context.SourceIndex == context.Source.Length)
        {
            return RemainingLength(
                context.Target.Length - context.TargetIndex,
                context.Edits);
        }


        if (context.TargetIndex == context.Target.Length)
        {
            return RemainingLength(
                context.Source.Length - context.SourceIndex,
                context.Edits);
        }


        if (context.Source[context.SourceIndex] ==
            context.Target[context.TargetIndex])
        {
            return Search(context.MoveNext());
        }


        var result = DISTANCE_EXCEEDS_LIMIT;


        if (!IsRepeatedDelete(context))
        {
            result = Math.Min(
                result,
                Search(context.Delete()));
        }


        if (!IsRepeatedInsert(context))
        {
            result = Math.Min(
                result,
                Search(context.Insert()));
        }


        return result;
    }


    private bool IsImpossible(EditDistanceState context)
    {
        var remainingSource =
            context.Source.Length - context.SourceIndex;

        var remainingTarget =
            context.Target.Length - context.TargetIndex;

        return Math.Abs(remainingSource - remainingTarget) >
               MAX_ALLOWED_EDITS - context.Edits;
    }


    private int RemainingLength(
        int length,
        int edits)
    {
        var result = length + edits;

        return result <= MAX_ALLOWED_EDITS
            ? result
            : DISTANCE_EXCEEDS_LIMIT;
    }


    private bool IsFinished(EditDistanceState context)
    {
        return context.SourceIndex == context.Source.Length &&
               context.TargetIndex == context.Target.Length;
    }


    private bool IsRepeatedDelete(EditDistanceState context)
    {
        return context.LastOperation == EditOperation.Delete &&
               context.SourceIndex == context.LastDeleteIndex + 1;
    }


    private bool IsRepeatedInsert(EditDistanceState context)
    {
        return context.LastOperation == EditOperation.Insert &&
               context.TargetIndex == context.LastInsertIndex + 1;
    }
}