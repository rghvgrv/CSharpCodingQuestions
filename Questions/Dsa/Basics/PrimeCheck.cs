namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 6, Title = "Is It a Prime Number?", Level = Easy, Problem = """
    A prime number is bigger than 1 and can be divided evenly only by 1 and itself: 2, 3, 5, 7, 11, …
    Return `true` if `n` is prime.
    """)]
public static class PrimeCheck
{
    [Approach(Name = "Try Every Divisor", Time = "O(n)", Space = "O(1)", Idea = """
        Try dividing `n` by every number from 2 to `n - 1`. If any divides evenly, `n` isn't prime.
        """)]
    public static bool IsPrimeSlow(int n)
    {
        if (n < 2)
        {
            return false;
        }
        for (int divisor = 2; divisor < n; divisor++)
        {
            if (n % divisor == 0)
            {
                return false;
            }
        }
        return true;
    }

    [Approach(Name = "Divisors up to √n", Time = "O(√n)", Space = "O(1)", Idea = """
        Divisors come in pairs: `36 = 4 × 9`. One number of each pair is always at most `√n`.
        So if nothing up to `√n` divides `n`, nothing bigger will either.
        For `n` = 1,000,000 that's 1,000 checks instead of 1,000,000.
        """)]
    public static bool IsPrimeFast(int n)
    {
        if (n < 2)
        {
            return false;
        }
        for (int divisor = 2; divisor * divisor <= n; divisor++)
        {
            if (n % divisor == 0)
            {
                return false;
            }
        }
        return true;
    }

    public static Example[] Examples =>
    [
        new([97], true),
        new([91], false),
        new([2], true),
        new([1], false),
    ];
}
