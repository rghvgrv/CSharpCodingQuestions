namespace CodingQuestions.Dsa.Basics;

[Q(1_01_09, "Fast Power (Binary Exponentiation)", Medium,
"Compute x^n in O(log n) multiplications, including negative n. Then compute (a^b) mod m for large b.")]
public static class FastPower
{
    // x^n = (x²)^(n/2), times x once more when n is odd.
    public static double Pow(double x, long n)
    {
        if (n < 0) { x = 1 / x; n = -n; }
        double result = 1;
        while (n > 0)
        {
            if ((n & 1) == 1) result *= x;
            x *= x;
            n >>= 1;
        }
        return result;
    }

    public static long ModPow(long a, long b, long m)
    {
        long result = 1;
        a %= m;
        while (b > 0)
        {
            if ((b & 1) == 1) result = result * a % m;
            a = a * a % m;
            b >>= 1;
        }
        return result;
    }

    public static void Run()
    {
        Check("Pow(2, 10)", Pow(2, 10), 1024.0);
        Check("Pow(2.1, 3)", Pow(2.1, 3), 9.261);
        Check("Pow(2, -2)", Pow(2, -2), 0.25);
        Check("ModPow(2, 1_000_000_000, 1_000_000_007)", ModPow(2, 1_000_000_000, 1_000_000_007), 140625001L);
    }
}
