namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 8, Title = "Greatest Common Divisor (GCD)", Level = Easy, Problem = """
    Find the biggest number that divides both `a` and `b` evenly. For 48 and 18 it's 6.

    Bonus: the least common multiple is `a / GCD(a, b) * b`.
    """)]
public static class GreatestCommonDivisor
{
    [Approach(Name = "Try Every Number", Time = "O(min(a, b))", Space = "O(1)", Idea = """
        Count down from the smaller number. The first number that divides both is the answer.
        """)]
    public static int GcdByTrying(int a, int b)
    {
        for (int candidate = Math.Min(a, b); candidate > 1; candidate--)
        {
            if (a % candidate == 0 && b % candidate == 0)
            {
                return candidate;
            }
        }
        return 1;
    }

    [Approach(Name = "Euclid's Algorithm", Time = "O(log(min(a, b)))", Space = "O(1)", Idea = """
        A 2,300-year-old trick: `GCD(a, b) = GCD(b, a % b)`, and `GCD(a, 0) = a`.
        The numbers shrink very quickly: `GCD(48, 18) → GCD(18, 12) → GCD(12, 6) → GCD(6, 0) = 6`.
        """)]
    public static int GcdEuclid(int a, int b)
    {
        while (b != 0)
        {
            int remainder = a % b;
            a = b;
            b = remainder;
        }
        return a;
    }

    public static Example[] Examples =>
    [
        new([48, 18], 6),
        new([17, 5], 1),
        new([100, 75], 25),
    ];
}
