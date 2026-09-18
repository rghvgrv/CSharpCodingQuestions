namespace CSharpCodingQuestions.Questions.Dsa.Hashing;

[Question(Order = 2, Title = "Contains Duplicate", Level = Easy, Problem = """
    Return `true` if any value appears at least twice. `[1, 2, 3, 1]` → `true`, `[1, 2, 3, 4]` → `false`.
    """)]
public static class ContainsDuplicate
{
    [Approach(Name = "Compare Every Pair", Time = "O(n²)", Space = "O(1)", Idea = """
        Compare each number with every number after it.
        """)]
    public static bool HasDuplicateBruteForce(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            for (int j = i + 1; j < numbers.Length; j++)
            {
                if (numbers[i] == numbers[j])
                {
                    return true;
                }
            }
        }
        return false;
    }

    [Approach(Name = "Sort, Then Check Neighbors", Time = "O(n log n)", Space = "O(n)", Idea = """
        After sorting, equal values sit next to each other, so one pass comparing neighbors finds any duplicate.
        """)]
    public static bool HasDuplicateBySorting(int[] numbers)
    {
        int[] sorted = (int[])numbers.Clone();
        Array.Sort(sorted);
        for (int i = 1; i < sorted.Length; i++)
        {
            if (sorted[i] == sorted[i - 1])
            {
                return true;
            }
        }
        return false;
    }

    [Approach(Name = "HashSet", Time = "O(n)", Space = "O(n)", Idea = """
        Add each number to a `HashSet`. `Add` returns `false` when the value is already in the set, which means we found a duplicate.
        """)]
    public static bool HasDuplicateWithHashSet(int[] numbers)
    {
        var seen = new HashSet<int>();
        foreach (int number in numbers)
        {
            if (!seen.Add(number))
            {
                return true;
            }
        }
        return false;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3, 1 }], true),
        new([new[] { 1, 2, 3, 4 }], false),
        new([new[] { 1, 1, 1, 3, 3, 4, 3, 2, 4, 2 }], true),
    ];
}
