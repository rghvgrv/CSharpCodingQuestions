namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 5, Title = "Integer Square Root", Level = Easy, Problem = """
    Return the square root of `n`, rounded down, without `Math.Sqrt`. `8` → `2` (because `2 × 2 = 4 ≤ 8 < 9 = 3 × 3`).
    """)]
public static class SquareRoot
{
    [Approach(Name = "Count Up", Time = "O(√n)", Space = "O(1)", Idea = """
        Try 1, 2, 3, … and stop just before the square becomes bigger than `n`.
        """)]
    public static int SqrtByCounting(int n)
    {
        long root = 0;
        while ((root + 1) * (root + 1) <= n)
        {
            root++;
        }
        return (int)root;
    }

    [Approach(Name = "Binary Search on the Answer", Time = "O(log n)", Space = "O(1)", Idea = """
        The answer is somewhere between 0 and `n`, and the question "is `m × m ≤ n`?" is *yes* for small `m` and *no* for large `m`.
        Binary search for the last *yes*. `long` is used so `m × m` can't overflow.
        """)]
    public static int SqrtByBinarySearch(int n)
    {
        long left = 0;
        long right = n;
        long answer = 0;
        while (left <= right)
        {
            long middle = left + (right - left) / 2;
            if (middle * middle <= n)
            {
                answer = middle;
                left = middle + 1;
            }
            else
            {
                right = middle - 1;
            }
        }
        return (int)answer;
    }

    public static Example[] Examples =>
    [
        new([8], 2),
        new([16], 4),
        new([2_147_395_600], 46340),
    ];
}
