namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 7, Title = "All Primes up to N", Level = Medium, Problem = """
    Return every prime number from 2 up to `n`.
    """)]
public static class PrimesUpToN
{
    [Approach(Name = "Check Each Number", Time = "O(n√n)", Space = "O(1) extra", Idea = """
        For every number from 2 to `n`, test whether it's prime by trying divisors up to its square root.
        """)]
    public static List<int> PrimesByChecking(int n)
    {
        var primes = new List<int>();
        for (int number = 2; number <= n; number++)
        {
            bool isPrime = true;
            for (int divisor = 2; divisor * divisor <= number; divisor++)
            {
                if (number % divisor == 0)
                {
                    isPrime = false;
                    break;
                }
            }
            if (isPrime)
            {
                primes.Add(number);
            }
        }
        return primes;
    }

    [Approach(Name = "Sieve of Eratosthenes", Time = "O(n log log n)", Space = "O(n)", Idea = """
        Instead of testing numbers, cross out the ones that can't be prime:

        1. Start with every number from 2 to `n` marked as "maybe prime".
        2. Take the next number that's still marked: it's prime.
        3. Cross out all its multiples, since they can't be prime.
        4. Repeat until the end.

        Each number gets crossed out only a few times, so this is much faster for large `n`.
        """)]
    public static List<int> PrimesBySieve(int n)
    {
        bool[] isCrossedOut = new bool[n + 1];
        var primes = new List<int>();

        for (int number = 2; number <= n; number++)
        {
            if (isCrossedOut[number])
            {
                continue;
            }

            primes.Add(number);
            for (int multiple = number * 2; multiple <= n; multiple += number)
            {
                isCrossedOut[multiple] = true;
            }
        }
        return primes;
    }

    public static Example[] Examples =>
    [
        new([30], new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 }),
        new([2], new[] { 2 }),
        new([1], Array.Empty<int>()),
    ];
}
