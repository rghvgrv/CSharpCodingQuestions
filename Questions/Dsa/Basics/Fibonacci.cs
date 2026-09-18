namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 5, Title = "Fibonacci Number", Level = Easy, Problem = """
    The Fibonacci sequence starts `0, 1` and each next number is the sum of the two before it:
    `0, 1, 1, 2, 3, 5, 8, 13, …`. Return the `n`-th number (counting from 0).
    """)]
public static class Fibonacci
{
    [Approach(Name = "Plain Recursion", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        Directly follow the definition: `Fib(n) = Fib(n - 1) + Fib(n - 2)`.

        Very slow, because the same values are computed again and again. `Fib(40)` makes over 300 million calls.
        """)]
    public static long FibonacciRecursive(int n)
    {
        if (n < 2)
        {
            return n;
        }
        return FibonacciRecursive(n - 1) + FibonacciRecursive(n - 2);
    }

    [Approach(Name = "Recursion + Memory (Memoization)", Time = "O(n)", Space = "O(n)", Idea = """
        Same recursion, but save every answer in a dictionary the first time it's computed.
        Next time the same `n` is asked for, return the saved answer. Each value is computed only once.
        """)]
    public static long FibonacciMemoized(int n)
    {
        return FibonacciWithMemory(n, new Dictionary<int, long>());
    }

    private static long FibonacciWithMemory(int n, Dictionary<int, long> memory)
    {
        if (n < 2)
        {
            return n;
        }
        if (memory.ContainsKey(n))
        {
            return memory[n];
        }

        long result = FibonacciWithMemory(n - 1, memory) + FibonacciWithMemory(n - 2, memory);
        memory[n] = result;
        return result;
    }

    [Approach(Name = "Loop with Two Variables", Time = "O(n)", Space = "O(1)", Idea = """
        Each number needs only the previous two, so keep just those two and slide forward `n` times.
        Fast, and it uses no extra memory.
        """)]
    public static long FibonacciLoop(int n)
    {
        long previous = 0;
        long current = 1;
        for (int i = 0; i < n; i++)
        {
            long next = previous + current;
            previous = current;
            current = next;
        }
        return previous;
    }

    public static Example[] Examples =>
    [
        new([10], 55L),
        new([1], 1L),
        new([30], 832040L),
    ];
}
