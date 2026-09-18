namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 10, Title = "Split Into Two Equal Halves", Level = Medium, Problem = """
    Can the numbers be divided into two groups with the **same sum**?
    `[1, 5, 11, 5]` → `true` (`1 + 5 + 5 = 11`). `[1, 2, 3, 5]` → `false`.
    """)]
public static class PartitionEqualSubsetSum
{
    [Approach(Name = "Plain Recursion", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        If the total is odd, it's impossible. Otherwise, the question becomes: can some numbers add up to **half** the total?
        For each number, either use it or don't, and recurse.
        """)]
    public static bool CanSplitRecursive(int[] numbers)
    {
        int total = numbers.Sum();
        if (total % 2 == 1)
        {
            return false;
        }
        return CanReach(numbers, 0, total / 2);
    }

    private static bool CanReach(int[] numbers, int index, int remaining)
    {
        if (remaining == 0)
        {
            return true;
        }
        if (index == numbers.Length || remaining < 0)
        {
            return false;
        }
        return CanReach(numbers, index + 1, remaining - numbers[index])
            || CanReach(numbers, index + 1, remaining);
    }

    [Approach(Name = "Table of Reachable Sums", Time = "O(n · sum)", Space = "O(sum)", Idea = """
        `reachable[s]` = "some of the numbers seen so far add up to `s`". Start with `reachable[0] = true`.
        For each number, mark `s` reachable if `s - number` was reachable before.
        Walk `s` from high to low, so each number is used at most once (the same trick as 0/1 Knapsack).
        """)]
    public static bool CanSplitTable(int[] numbers)
    {
        int total = numbers.Sum();
        if (total % 2 == 1)
        {
            return false;
        }

        int target = total / 2;
        bool[] reachable = new bool[target + 1];
        reachable[0] = true;
        foreach (int number in numbers)
        {
            for (int sum = target; sum >= number; sum--)
            {
                if (reachable[sum - number])
                {
                    reachable[sum] = true;
                }
            }
        }
        return reachable[target];
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 5, 11, 5 }], true),
        new([new[] { 1, 2, 3, 5 }], false),
        new([new[] { 2, 2, 3, 5 }], false),
    ];
}
