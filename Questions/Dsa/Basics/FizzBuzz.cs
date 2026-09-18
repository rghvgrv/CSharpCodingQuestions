namespace CodingQuestions.Dsa.Basics;

[Q(1_01_03, "FizzBuzz", Easy,
"For numbers 1 to n: print \"Fizz\" for multiples of 3, \"Buzz\" for multiples of 5, \"FizzBuzz\" for multiples of both, otherwise the number.")]
public static class FizzBuzz
{
    // Time O(n). Check 15 first, or use a switch on the pair of remainders.
    public static List<string> Solve(int n)
    {
        var result = new List<string>();
        for (int i = 1; i <= n; i++)
        {
            result.Add((i % 3, i % 5) switch
            {
                (0, 0) => "FizzBuzz",
                (0, _) => "Fizz",
                (_, 0) => "Buzz",
                _ => i.ToString()
            });
        }
        return result;
    }

    public static void Run()
    {
        Check("Solve(15)", Solve(15), ["1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz"]);
    }
}
