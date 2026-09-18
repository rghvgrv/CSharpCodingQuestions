namespace CodingQuestions.Dsa.Basics;

[Q(1_01_10, "Number Base Conversion", Easy,
"Convert a decimal number to binary (and any base 2–16) by hand, then back to decimal.")]
public static class BaseConversion
{
    const string Digits = "0123456789ABCDEF";

    // Repeatedly divide by the base; the remainders are the digits in reverse order.
    public static string ToBase(int n, int b)
    {
        if (n == 0) return "0";
        var sb = new StringBuilder();
        for (; n > 0; n /= b) sb.Insert(0, Digits[n % b]);
        return sb.ToString();
    }

    // Horner's rule: value = value * base + digit
    public static int FromBase(string s, int b) => s.Aggregate(0, (value, c) => value * b + Digits.IndexOf(char.ToUpper(c)));

    public static void Run()
    {
        Check("ToBase(10, 2)", ToBase(10, 2), "1010");
        Check("ToBase(255, 16)", ToBase(255, 16), "FF");
        Check("ToBase(64, 8)", ToBase(64, 8), "100");
        Check("FromBase(\"1010\", 2)", FromBase("1010", 2), 10);
        Check("FromBase(\"ff\", 16)", FromBase("ff", 16), 255);
        Check("Built-in Convert.ToString(10, 2)", Convert.ToString(10, 2), "1010");
    }
}
