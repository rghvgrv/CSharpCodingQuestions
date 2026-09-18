namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 5, Title = "Longest Increasing Subsequence", Level = Medium, Problem = """
    A subsequence keeps the original order but may skip items. Find the length of the longest one whose numbers strictly increase.
    `[10, 9, 2, 5, 3, 7, 101, 18]` → `4` (for example `2, 3, 7, 18`).
    """)]
public static class LongestIncreasingSubsequence
{
    [Approach(Name = "DP Over Every Earlier Number", Time = "O(n²)", Space = "O(n)", Idea = """
        `longest[i]` = the length of the longest increasing subsequence that **ends at** `numbers[i]`.
        It's 1, plus the best `longest[j]` of any earlier `j` with a smaller number. The answer is the biggest `longest[i]`.
        """)]
    public static int LengthQuadratic(int[] numbers)
    {
        int[] longest = new int[numbers.Length];
        int best = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            longest[i] = 1;
            for (int j = 0; j < i; j++)
            {
                if (numbers[j] < numbers[i])
                {
                    longest[i] = Math.Max(longest[i], longest[j] + 1);
                }
            }
            best = Math.Max(best, longest[i]);
        }
        return best;
    }

    [Approach(Name = "Smallest Tails + Binary Search", Time = "O(n log n)", Space = "O(n)", Idea = """
        Keep a list `tails`, where `tails[k]` is the **smallest possible last number** of an increasing subsequence of length `k + 1`.
        This list is always sorted, so for each number:

        - If it's bigger than every tail, it extends the longest subsequence: append it.
        - Otherwise, find (binary search) the first tail ≥ it and replace that tail. A smaller tail is easier to extend later.

        The length of `tails` is the answer.
        """)]
    public static int LengthWithBinarySearch(int[] numbers)
    {
        var tails = new List<int>();
        foreach (int number in numbers)
        {
            int left = 0;
            int right = tails.Count;
            while (left < right)
            {
                int middle = (left + right) / 2;
                if (tails[middle] < number)
                {
                    left = middle + 1;
                }
                else
                {
                    right = middle;
                }
            }

            if (left == tails.Count)
            {
                tails.Add(number);
            }
            else
            {
                tails[left] = number;
            }
        }
        return tails.Count;
    }

    public static Example[] Examples =>
    [
        new([new[] { 10, 9, 2, 5, 3, 7, 101, 18 }], 4),
        new([new[] { 0, 1, 0, 3, 2, 3 }], 4),
        new([new[] { 7, 7, 7, 7 }], 1),
    ];
}
