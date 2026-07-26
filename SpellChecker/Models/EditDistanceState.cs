
namespace SpellChecker.Models;

public class EditDistanceState
{
    public string Source { get; }
    public string Target { get; }

    public int SourceIndex { get; }
    public int TargetIndex { get; }

    public int Edits { get; }

    public EditOperation LastOperation { get; }

    public int LastDeleteIndex { get; }
    public int LastInsertIndex { get; }


    public EditDistanceState(
        string source,
        string target,
        int sourceIndex,
        int targetIndex,
        int edits,
        EditOperation lastOperation,
        int lastDeleteIndex,
        int lastInsertIndex)
    {
        Source = source;
        Target = target;

        SourceIndex = sourceIndex;
        TargetIndex = targetIndex;

        Edits = edits;

        LastOperation = lastOperation;

        LastDeleteIndex = lastDeleteIndex;
        LastInsertIndex = lastInsertIndex;
    }


    public EditDistanceState MoveNext()
    {
        return new EditDistanceState(
            Source,
            Target,
            SourceIndex + 1,
            TargetIndex + 1,
            Edits,
            LastOperation,
            LastDeleteIndex,
            LastInsertIndex);
    }


    public EditDistanceState Delete()
    {
        return new EditDistanceState(
            Source,
            Target,
            SourceIndex + 1,
            TargetIndex,
            Edits + 1,
            EditOperation.Delete,
            SourceIndex,
            LastInsertIndex);
    }


    public EditDistanceState Insert()
    {
        return new EditDistanceState(
            Source,
            Target,
            SourceIndex,
            TargetIndex + 1,
            Edits + 1,
            EditOperation.Insert,
            LastDeleteIndex,
            TargetIndex);
    }
}