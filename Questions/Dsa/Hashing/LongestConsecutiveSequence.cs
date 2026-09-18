namespace CSharpCodingQuestions.Questions.Dsa.Hashing;

[Question(Order = 3, Title = "Longest Consecutive Sequence", Level = Medium, Problem = """
    In an unsorted array, find the length of the longest run of consecutive numbers (like 1, 2, 3, 4), in any order in the array.
    `[100, 4, 200, 1, 3, 2]` → `4`, because of `1, 2, 3, 4`.
    """)]
public static class LongestConsecutiveSequence
{
    [Approach(Name = "Search the Array for the Next Number", Time = "O(n³)", Space = "O(1)", Idea = """
        From each number `x`, keep checking whether `x + 1`, `x + 2`, … exist, searching the whole array every time.
        """)]
    public static int LongestBruteForce(int[] numbers)
    {
        int best = 0;
        foreach (int number in numbers)
        {
            int length = 1;
            while (numbers.Contains(number + length))
            {
                length++;
            }
            best = Math.Max(best, length);
        }
        return best;
    }

    [Approach(Name = "Sort", Time = "O(n log n)", Space = "O(n)", Idea = """
        After sorting, consecutive numbers are neighbors. Walk once, extending the current run when the next number is exactly one bigger,
        ignoring repeated numbers, and resetting otherwise.
        """)]
    public static int LongestBySorting(int[] numbers)
    {
        if (numbers.Length == 0)
        {
            return 0;
        }

        int[] sorted = (int[])numbers.Clone();
        Array.Sort(sorted);
        int best = 1;
        int current = 1;
        for (int i = 1; i < sorted.Length; i++)
        {
            if (sorted[i] == sorted[i - 1])
            {
                continue;
            }
            if (sorted[i] == sorted[i - 1] + 1)
            {
                current++;
            }
            else
            {
                current = 1;
            }
            best = Math.Max(best, current);
        }
        return best;
    }

    [Approach(Name = "HashSet, Count Only From Run Starts", Time = "O(n)", Space = "O(n)", Idea = """
        Put all numbers in a `HashSet` so "is `x` present?" is instant.

        A number starts a run only if `x - 1` is **not** in the set. Only from those starts, count upward `x + 1`, `x + 2`, …
        This way every number is counted just once in total.
        """)]
    public static int LongestWithHashSet(int[] numbers)
    {
        var present = new HashSet<int>(numbers);
        int best = 0;
        foreach (int number in present)
        {
            if (present.Contains(number - 1))
            {
                continue;
            }

            int length = 1;
            while (present.Contains(number + length))
            {
                length++;
            }
            best = Math.Max(best, length);
        }
        return best;
    }

    public static Example[] Examples =>
    [
        new([new[] { 100, 4, 200, 1, 3, 2 }], 4),
        new([new[] { 0, 3, 7, 2, 5, 8, 4, 6, 0, 1 }], 9),
        new([Array.Empty<int>()], 0),
    ];
}
