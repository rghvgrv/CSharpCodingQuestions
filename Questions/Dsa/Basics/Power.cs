namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 9, Title = "Power (x to the n)", Level = Medium, Problem = """
    Compute `x` raised to the power `n` (with `n ≥ 0`) without `Math.Pow`. For example `2¹⁰ = 1024`.
    """)]
public static class Power
{
    [Approach(Name = "Multiply n Times", Time = "O(n)", Space = "O(1)", Idea = """
        Start with 1 and multiply by `x`, `n` times.
        """)]
    public static long PowerByLoop(long x, int n)
    {
        long result = 1;
        for (int i = 0; i < n; i++)
        {
            result *= x;
        }
        return result;
    }

    [Approach(Name = "Fast Power (Squaring)", Time = "O(log n)", Space = "O(1)", Idea = """
        Use `x¹⁰ = (x²)⁵`: squaring `x` halves the exponent.

        1. If `n` is odd, multiply the result by `x` once.
        2. Square `x` and halve `n`.
        3. Repeat until `n` is 0.

        `2¹⁰⁰⁰` takes about 10 steps instead of 1,000.
        """)]
    public static long PowerBySquaring(long x, int n)
    {
        long result = 1;
        while (n > 0)
        {
            if (n % 2 == 1)
            {
                result *= x;
            }
            x *= x;
            n /= 2;
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([2L, 10], 1024L),
        new([3L, 5], 243L),
        new([7L, 0], 1L),
    ];
}
