namespace CodingQuestions.Dsa.Basics;

[Q(1_01_06, "Prime Numbers & Sieve of Eratosthenes", Easy,
"Check whether a number is prime, then list all primes up to n with the Sieve of Eratosthenes.")]
public static class PrimeNumbers
{
    // Time O(√n): a factor larger than √n would need a partner smaller than √n.
    public static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int i = 2; (long)i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    // Time O(n log log n): cross out multiples of every prime, starting at p².
    public static List<int> Sieve(int n)
    {
        var composite = new bool[n + 1];
        var primes = new List<int>();
        for (int p = 2; p <= n; p++)
        {
            if (composite[p]) continue;
            primes.Add(p);
            for (long m = (long)p * p; m <= n; m += p) composite[m] = true;
        }
        return primes;
    }

    public static void Run()
    {
        Check("IsPrime(97)", IsPrime(97), true);
        Check("IsPrime(1)", IsPrime(1), false);
        Check("IsPrime(91)", IsPrime(91), false);
        Check("Sieve(50)", Sieve(50), [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47]);
        Check("Sieve(1_000_000).Count", Sieve(1_000_000).Count, 78498);
    }
}
