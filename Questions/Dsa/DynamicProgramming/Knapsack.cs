namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 4, Title = "0/1 Knapsack", Level = Medium, Problem = """
    Each item has a `weight` and a `value`. Your bag holds at most `capacity`. Each item is taken **whole or not at all**.
    Maximize the total value.
    `weights = [1, 3, 4, 5]`, `values = [1, 4, 5, 7]`, `capacity = 7` → `9` (the items weighing 3 and 4).
    """)]
public static class Knapsack
{
    [Approach(Name = "Plain Recursion", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        For each item, choose: **skip** it, or **take** it (if it fits) and lose its weight from the space left.
        Return the better of the two. With `n` items that's 2ⁿ combinations.
        """)]
    public static int MaxValueRecursive(int[] weights, int[] values, int capacity)
    {
        return Best(weights, values, 0, capacity);
    }

    private static int Best(int[] weights, int[] values, int item, int spaceLeft)
    {
        if (item == weights.Length)
        {
            return 0;
        }
        int skip = Best(weights, values, item + 1, spaceLeft);
        if (weights[item] > spaceLeft)
        {
            return skip;
        }
        int take = values[item] + Best(weights, values, item + 1, spaceLeft - weights[item]);
        return Math.Max(skip, take);
    }

    [Approach(Name = "Table (Bottom-Up)", Time = "O(n · capacity)", Space = "O(n · capacity)", Idea = """
        `best[i, w]` = the best value using only the first `i` items with a bag of size `w`.

        - Skip item `i`: `best[i - 1, w]`
        - Take item `i` (if it fits): `value + best[i - 1, w - weight]`

        Fill the table row by row. The answer is in the bottom-right cell.
        """)]
    public static int MaxValueTable(int[] weights, int[] values, int capacity)
    {
        int n = weights.Length;
        int[,] best = new int[n + 1, capacity + 1];
        for (int i = 1; i <= n; i++)
        {
            for (int w = 0; w <= capacity; w++)
            {
                best[i, w] = best[i - 1, w];
                if (weights[i - 1] <= w)
                {
                    best[i, w] = Math.Max(best[i, w], values[i - 1] + best[i - 1, w - weights[i - 1]]);
                }
            }
        }
        return best[n, capacity];
    }

    [Approach(Name = "One Row", Time = "O(n · capacity)", Space = "O(capacity)", Idea = """
        Each row only reads the row above, so one array is enough, **if** `w` goes from high to low.
        Going downward means `best[w - weight]` still holds the old row's value, so each item is used at most once.
        """)]
    public static int MaxValueOneRow(int[] weights, int[] values, int capacity)
    {
        int[] best = new int[capacity + 1];
        for (int i = 0; i < weights.Length; i++)
        {
            for (int w = capacity; w >= weights[i]; w--)
            {
                best[w] = Math.Max(best[w], values[i] + best[w - weights[i]]);
            }
        }
        return best[capacity];
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 3, 4, 5 }, new[] { 1, 4, 5, 7 }, 7], 9),
        new([new[] { 10, 20, 30 }, new[] { 60, 100, 120 }, 50], 220),
    ];
}
