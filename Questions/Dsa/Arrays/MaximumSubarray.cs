namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 9, Title = "Maximum Subarray Sum", Level = Medium, Problem = """
    A subarray is a run of neighboring items. Find the largest sum of any subarray.
    `[-2, 1, -3, 4, -1, 2, 1, -5, 4]` → `6`, from `[4, -1, 2, 1]`.
    """)]
public static class MaximumSubarray
{
    [Approach(Name = "Try Every Subarray", Time = "O(n²)", Space = "O(1)", Idea = """
        For each start position, extend the subarray one item at a time, keeping a running sum, and remember the best sum seen.
        """)]
    public static int MaxSubarrayBruteForce(int[] numbers)
    {
        int best = int.MinValue;
        for (int start = 0; start < numbers.Length; start++)
        {
            int sum = 0;
            for (int end = start; end < numbers.Length; end++)
            {
                sum += numbers[end];
                best = Math.Max(best, sum);
            }
        }
        return best;
    }

    [Approach(Name = "Kadane's Algorithm", Time = "O(n)", Space = "O(1)", Idea = """
        Walk left to right, keeping `current` = the best sum of a subarray that **ends here**.
        At each number, either extend the previous run or start fresh from this number:

        `current = max(number, current + number)`

        If the running sum turns negative, it can only hurt what comes next, so start again.
        """)]
    public static int MaxSubarrayKadane(int[] numbers)
    {
        int current = numbers[0];
        int best = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            current = Math.Max(numbers[i], current + numbers[i]);
            best = Math.Max(best, current);
        }
        return best;
    }

    public static Example[] Examples =>
    [
        new([new[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }], 6),
        new([new[] { 5, 4, -1, 7, 8 }], 23),
        new([new[] { -3, -1, -2 }], -1),
    ];
}
