namespace CodingQuestions.Dsa.BitManipulation;

[Q(1_09_01, "Bit Basics: Get, Set, Clear, Toggle", Easy,
"Using bitwise operators, read the i-th bit, set it, clear it, toggle it, and check whether a number is even.")]
public static class BitBasics
{
    public static bool Get(int n, int i) => (n & (1 << i)) != 0;
    public static int Set(int n, int i) => n | (1 << i);
    public static int Clear(int n, int i) => n & ~(1 << i);
    public static int Toggle(int n, int i) => n ^ (1 << i);
    public static bool IsEven(int n) => (n & 1) == 0;
    static string Bin(int n) => Convert.ToString(n, 2).PadLeft(4, '0');

    public static void Run()
    {
        int n = 0b1010; // 10
        Check($"Get({Bin(n)}, 1)", Get(n, 1), true);
        Check($"Set({Bin(n)}, 0)", Bin(Set(n, 0)), "1011");
        Check($"Clear({Bin(n)}, 3)", Bin(Clear(n, 3)), "0010");
        Check($"Toggle({Bin(n)}, 2)", Bin(Toggle(n, 2)), "1110");
        Check("IsEven(10)", IsEven(10), true);
        Check("x << 3 = x * 8", 5 << 3, 40);
        Check("x >> 1 = x / 2", 41 >> 1, 20);
        Check("swap via XOR", Swap(3, 9), (9, 3));
    }

    static (int, int) Swap(int a, int b)
    {
        a ^= b; b ^= a; a ^= b;
        return (a, b);
    }
}
