namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 12, Title = "Missing Number", Level = Easy, Problem = """
    The array holds `n` different numbers taken from `0, 1, 2, …, n`, so exactly one number is missing. Find it.
    `[3, 0, 1]` → `2`.
    """)]
public static class MissingNumber
{
    [Approach(Name = "Sort", Time = "O(n log n)", Space = "O(n)", Idea = """
        Sort a copy. Then position `i` should hold the value `i`; the first place where it doesn't is the missing number.
        If every position matches, the missing number is `n`.
        """)]
    public static int MissingBySorting(int[] numbers)
    {
        int[] sorted = (int[])numbers.Clone();
        Array.Sort(sorted);
        for (int i = 0; i < sorted.Length; i++)
        {
            if (sorted[i] != i)
            {
                return i;
            }
        }
        return sorted.Length;
    }

    [Approach(Name = "HashSet", Time = "O(n)", Space = "O(n)", Idea = """
        Put every number in a `HashSet`, then check `0, 1, 2, …, n` until one isn't in the set.
        """)]
    public static int MissingWithHashSet(int[] numbers)
    {
        var present = new HashSet<int>(numbers);
        for (int i = 0; i <= numbers.Length; i++)
        {
            if (!present.Contains(i))
            {
                return i;
            }
        }
        return -1;
    }

    [Approach(Name = "Sum Formula", Time = "O(n)", Space = "O(1)", Idea = """
        The sum `0 + 1 + … + n` is `n × (n + 1) / 2`. Subtract the actual sum of the array, and what's left is the missing number.
        """)]
    public static int MissingBySum(int[] numbers)
    {
        int n = numbers.Length;
        int expectedSum = n * (n + 1) / 2;
        int actualSum = 0;
        foreach (int number in numbers)
        {
            actualSum += number;
        }
        return expectedSum - actualSum;
    }

    public static Example[] Examples =>
    [
        new([new[] { 3, 0, 1 }], 2),
        new([new[] { 9, 6, 4, 2, 3, 5, 7, 0, 1 }], 8),
        new([new[] { 0, 1 }], 2),
    ];
}
