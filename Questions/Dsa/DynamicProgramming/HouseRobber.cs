namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 2, Title = "House Robber", Level = Medium, Problem = """
    Houses stand in a row with some money in each. You can't take from **two neighboring** houses.
    What is the most money you can collect? `[2, 7, 9, 3, 1]` → `12` (houses 2 + 9 + 1).
    """)]
public static class HouseRobber
{
    [Approach(Name = "Plain Recursion", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        At house `i`, you have two choices:

        - **Skip** it → best from house `i + 1` onward.
        - **Take** it → its money + best from house `i + 2` onward (the neighbor must be skipped).

        Take the larger. Many sub-problems repeat.
        """)]
    public static int RobRecursive(int[] money)
    {
        return BestFrom(money, 0);
    }

    private static int BestFrom(int[] money, int house)
    {
        if (house >= money.Length)
        {
            return 0;
        }
        int skip = BestFrom(money, house + 1);
        int take = money[house] + BestFrom(money, house + 2);
        return Math.Max(skip, take);
    }

    [Approach(Name = "Memoization", Time = "O(n)", Space = "O(n)", Idea = """
        Same choices, but remember the best answer for each starting house so it's computed only once.
        `-1` marks "not computed yet".
        """)]
    public static int RobMemoized(int[] money)
    {
        int[] memory = new int[money.Length];
        Array.Fill(memory, -1);
        return BestFromMemo(money, 0, memory);
    }

    private static int BestFromMemo(int[] money, int house, int[] memory)
    {
        if (house >= money.Length)
        {
            return 0;
        }
        if (memory[house] == -1)
        {
            int skip = BestFromMemo(money, house + 1, memory);
            int take = money[house] + BestFromMemo(money, house + 2, memory);
            memory[house] = Math.Max(skip, take);
        }
        return memory[house];
    }

    [Approach(Name = "Bottom-Up With Two Variables", Time = "O(n)", Space = "O(1)", Idea = """
        Walk the houses left to right, keeping:

        - `withoutPrevious`: the best total if the previous house was skipped
        - `best`: the best total up to the previous house

        For each house, `best = max(best, withoutPrevious + money)`.
        """)]
    public static int RobBottomUp(int[] money)
    {
        int withoutPrevious = 0;
        int best = 0;
        foreach (int amount in money)
        {
            int newBest = Math.Max(best, withoutPrevious + amount);
            withoutPrevious = best;
            best = newBest;
        }
        return best;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3, 1 }], 4),
        new([new[] { 2, 7, 9, 3, 1 }], 12),
        new([new[] { 2, 1, 1, 2 }], 4),
    ];
}
