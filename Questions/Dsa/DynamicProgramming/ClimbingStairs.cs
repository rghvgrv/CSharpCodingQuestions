namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 1, Title = "Climbing Stairs", Level = Easy, Problem = """
    You climb a staircase of `n` steps, taking **1 or 2 steps** at a time. In how many different ways can you reach the top?
    `n = 3` → `3` (1+1+1, 1+2, 2+1).
    """)]
public static class ClimbingStairs
{
    [Approach(Name = "Plain Recursion", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        Your last move was either 1 step or 2 steps, so:
        `Ways(n) = Ways(n - 1) + Ways(n - 2)`, with `Ways(0) = Ways(1) = 1`.

        Correct, but the same values are recomputed again and again.
        """)]
    public static long WaysRecursive(int n)
    {
        if (n <= 1)
        {
            return 1;
        }
        return WaysRecursive(n - 1) + WaysRecursive(n - 2);
    }

    [Approach(Name = "Memoization (Top-Down)", Time = "O(n)", Space = "O(n)", Idea = """
        Same recursion, but store each answer in an array the first time. Later calls for the same `n` just read it.
        """)]
    public static long WaysMemoized(int n)
    {
        return Ways(n, new long[n + 1]);
    }

    private static long Ways(int n, long[] memory)
    {
        if (n <= 1)
        {
            return 1;
        }
        if (memory[n] == 0)
        {
            memory[n] = Ways(n - 1, memory) + Ways(n - 2, memory);
        }
        return memory[n];
    }

    [Approach(Name = "Bottom-Up With Two Variables", Time = "O(n)", Space = "O(1)", Idea = """
        Build up from the bottom step. Each answer needs only the two before it, so keep just two variables.
        (It's the Fibonacci sequence in disguise.)
        """)]
    public static long WaysBottomUp(int n)
    {
        long twoBelow = 1;
        long oneBelow = 1;
        for (int step = 2; step <= n; step++)
        {
            long current = oneBelow + twoBelow;
            twoBelow = oneBelow;
            oneBelow = current;
        }
        return oneBelow;
    }

    public static Example[] Examples =>
    [
        new([3], 3L),
        new([5], 8L),
        new([30], 1346269L),
    ];
}
