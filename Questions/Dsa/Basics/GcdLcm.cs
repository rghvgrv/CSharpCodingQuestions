namespace CodingQuestions.Dsa.Basics;

[Q(1_01_07, "GCD & LCM", Easy,
"Find the greatest common divisor and least common multiple of two numbers using Euclid's algorithm.")]
public static class GcdLcm
{
    // Time O(log min(a, b)): gcd(a, b) = gcd(b, a mod b)
    public static long Gcd(long a, long b) => b == 0 ? Math.Abs(a) : Gcd(b, a % b);

    // Divide before multiplying to avoid overflow.
    public static long Lcm(long a, long b) => a == 0 || b == 0 ? 0 : Math.Abs(a / Gcd(a, b) * b);

    public static void Run()
    {
        Check("Gcd(48, 18)", Gcd(48, 18), 6L);
        Check("Gcd(17, 5)", Gcd(17, 5), 1L);
        Check("Lcm(4, 6)", Lcm(4, 6), 12L);
        Check("Lcm(21, 6)", Lcm(21, 6), 42L);
        Check("Gcd of [12, 18, 24]", new long[] { 12, 18, 24 }.Aggregate(Gcd), 6L);
    }
}
