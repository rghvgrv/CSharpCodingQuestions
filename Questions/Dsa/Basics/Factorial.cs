namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 4, Title = "Factorial", Level = Easy, Problem = """
    The factorial of `n` (written `n!`) is `1 × 2 × 3 × … × n`. For example `5! = 120`. By definition `0! = 1`.
    """)]
public static class Factorial
{
    [Approach(Name = "Recursion", Time = "O(n)", Space = "O(n)", Idea = """
        `n! = n × (n - 1)!`, so the method calls itself with a smaller number until it reaches 1 (the base case).
        Each waiting call takes stack memory, so a very large `n` could overflow the stack.
        """)]
    public static long FactorialRecursive(int n)
    {
        if (n <= 1)
        {
            return 1;
        }
        return n * FactorialRecursive(n - 1);
    }

    [Approach(Name = "Loop", Time = "O(n)", Space = "O(1)", Idea = """
        Start with 1 and multiply by 2, 3, … up to `n`. Same number of steps, but no extra memory.

        Note: `long` can hold up to `20!`. For bigger numbers use `System.Numerics.BigInteger`.
        """)]
    public static long FactorialLoop(int n)
    {
        long result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([5], 120L),
        new([0], 1L),
        new([20], 2432902008176640000L),
    ];
}
