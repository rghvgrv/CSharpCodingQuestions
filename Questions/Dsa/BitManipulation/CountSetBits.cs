namespace CodingQuestions.Dsa.BitManipulation;

[Q(1_09_02, "Count Set Bits & Power of Two", Easy,
"Count the 1 bits in a number (Brian Kernighan's trick), check if a number is a power of two, and count bits for every number 0..n in O(n).")]
public static class CountSetBits
{
    // n & (n - 1) clears the lowest set bit. Loop runs once per 1 bit.
    public static int Kernighan(uint n)
    {
        int count = 0;
        for (; n != 0; n &= n - 1) count++;
        return count;
    }

    // A power of two has exactly one bit set.
    public static bool IsPowerOfTwo(int n) => n > 0 && (n & (n - 1)) == 0;

    // DP: bits(i) = bits(i >> 1) + lowest bit
    public static int[] CountAll(int n)
    {
        var bits = new int[n + 1];
        for (int i = 1; i <= n; i++) bits[i] = bits[i >> 1] + (i & 1);
        return bits;
    }

    public static void Run()
    {
        Check("Kernighan(11 = 1011)", Kernighan(11), 3);
        Check("Kernighan(uint.MaxValue)", Kernighan(uint.MaxValue), 32);
        Check("Built-in BitOperations.PopCount(11)", System.Numerics.BitOperations.PopCount(11), 3);
        Check("IsPowerOfTwo(64)", IsPowerOfTwo(64), true);
        Check("IsPowerOfTwo(6)", IsPowerOfTwo(6), false);
        Check("CountAll(5)", CountAll(5), [0, 1, 1, 2, 1, 2]);
    }
}
