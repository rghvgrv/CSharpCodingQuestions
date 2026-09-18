namespace CSharpCodingQuestions.Questions.Dsa.Greedy;

[Question(Order = 2, Title = "Jump Game", Level = Medium, Problem = """
    You start at index 0. `jumps[i]` is the **maximum** number of steps you can jump forward from index `i`.
    Can you reach the last index? `[2, 3, 1, 1, 4]` → `true`. `[3, 2, 1, 0, 4]` → `false` (you always get stuck on the 0).
    """)]
public static class JumpGame
{
    [Approach(Name = "Try Every Jump (Recursion)", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        From each index, try every jump length from 1 up to `jumps[i]`, and recurse.
        It explores the same positions many times, so it's exponential.
        """)]
    public static bool CanReachRecursive(int[] jumps)
    {
        return CanReachFrom(jumps, 0);
    }

    private static bool CanReachFrom(int[] jumps, int index)
    {
        if (index >= jumps.Length - 1)
        {
            return true;
        }
        for (int step = 1; step <= jumps[index]; step++)
        {
            if (CanReachFrom(jumps, index + step))
            {
                return true;
            }
        }
        return false;
    }

    [Approach(Name = "Greedy: Track the Farthest Reach", Time = "O(n)", Space = "O(1)", Idea = """
        Walk left to right, keeping `farthest` = the farthest index reachable so far.
        If you ever stand on an index beyond `farthest`, you couldn't have got there, so the answer is `false`.
        Otherwise update `farthest = max(farthest, i + jumps[i])`.
        """)]
    public static bool CanReachGreedy(int[] jumps)
    {
        int farthest = 0;
        for (int i = 0; i < jumps.Length; i++)
        {
            if (i > farthest)
            {
                return false;
            }
            farthest = Math.Max(farthest, i + jumps[i]);
        }
        return true;
    }

    public static Example[] Examples =>
    [
        new([new[] { 2, 3, 1, 1, 4 }], true),
        new([new[] { 3, 2, 1, 0, 4 }], false),
        new([new[] { 0 }], true),
    ];
}
