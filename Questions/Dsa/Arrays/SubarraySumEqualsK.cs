namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 15, Title = "Count Subarrays That Sum to K", Level = Medium, Problem = """
    Count the subarrays (runs of neighboring items) whose sum is exactly `k`. Numbers can be negative.
    `numbers = [1, 2, 3]`, `k = 3` → `2`: `[1, 2]` and `[3]`.
    """)]
public static class SubarraySumEqualsK
{
    [Approach(Name = "Try Every Subarray", Time = "O(n²)", Space = "O(1)", Idea = """
        For each start, extend to the right with a running sum, and count every time the sum equals `k`.
        """)]
    public static int CountBruteForce(int[] numbers, int k)
    {
        int count = 0;
        for (int start = 0; start < numbers.Length; start++)
        {
            int sum = 0;
            for (int end = start; end < numbers.Length; end++)
            {
                sum += numbers[end];
                if (sum == k)
                {
                    count++;
                }
            }
        }
        return count;
    }

    [Approach(Name = "Prefix Sums + Dictionary", Time = "O(n)", Space = "O(n)", Idea = """
        A **prefix sum** is the total of everything up to a position. The sum of a subarray is the difference of two prefix sums:
        `sum(i+1..j) = prefix[j] - prefix[i]`.

        So a subarray ending here sums to `k` when some **earlier** prefix equals `prefix - k`.
        Keep a dictionary of how many times each prefix sum has appeared, and look that up at every step.
        Start with `{0: 1}` for the empty prefix.
        """)]
    public static int CountWithPrefixSums(int[] numbers, int k)
    {
        var timesSeen = new Dictionary<int, int> { [0] = 1 };
        int prefix = 0;
        int count = 0;

        foreach (int number in numbers)
        {
            prefix += number;
            count += timesSeen.GetValueOrDefault(prefix - k);
            timesSeen[prefix] = timesSeen.GetValueOrDefault(prefix) + 1;
        }
        return count;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 1, 1 }, 2], 2),
        new([new[] { 1, 2, 3 }, 3], 2),
        new([new[] { 1, -1, 0 }, 0], 3),
    ];
}
