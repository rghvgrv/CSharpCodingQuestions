using System.Numerics;

namespace CodingQuestions.Dsa.Basics;

[Q(1_01_04, "Factorial", Easy,
"Compute n! iteratively and recursively. Handle large n (for example 25!) without overflow.")]
public static class Factorial
{
    // Time O(n), Space O(1)
    public static long Iterative(int n)
    {
        long result = 1;
        for (int i = 2; i <= n; i++) result *= i;
        return result;
    }

    // Time O(n), Space O(n) for the call stack
    public static long Recursive(int n) => n <= 1 ? 1 : n * Recursive(n - 1);

    // long overflows after 20!, so BigInteger handles anything bigger
    public static BigInteger Big(int n)
    {
        BigInteger result = 1;
        for (int i = 2; i <= n; i++) result *= i;
        return result;
    }

    public static void Run()
    {
        Check("Iterative(5)", Iterative(5), 120L);
        Check("Recursive(10)", Recursive(10), 3628800L);
        Check("Iterative(20)", Iterative(20), 2432902008176640000L);
        Check("Big(25)", Big(25).ToString(), "15511210043330985984000000");
    }
}
