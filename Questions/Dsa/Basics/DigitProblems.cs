namespace CodingQuestions.Dsa.Basics;

[Q(1_01_08, "Digit Problems: Sum, Reverse, Armstrong", Easy,
"Using only % and /: sum the digits of a number, reverse an integer (return 0 on overflow), and check for Armstrong numbers (153 = 1³ + 5³ + 3³).")]
public static class DigitProblems
{
    public static int DigitSum(int n)
    {
        n = Math.Abs(n);
        int sum = 0;
        for (; n > 0; n /= 10) sum += n % 10;
        return sum;
    }

    // `checked` throws on overflow instead of silently wrapping around.
    public static int Reverse(int x)
    {
        try
        {
            int result = 0;
            for (; x != 0; x /= 10) result = checked(result * 10 + x % 10);
            return result;
        }
        catch (OverflowException) { return 0; }
    }

    public static bool IsArmstrong(int n)
    {
        int digits = n.ToString().Length, sum = 0;
        for (int x = n; x > 0; x /= 10) sum += (int)Math.Pow(x % 10, digits);
        return sum == n;
    }

    public static void Run()
    {
        Check("DigitSum(9875)", DigitSum(9875), 29);
        Check("Reverse(123)", Reverse(123), 321);
        Check("Reverse(-120)", Reverse(-120), -21);
        Check("Reverse(1534236469)", Reverse(1534236469), 0);
        Check("IsArmstrong(153)", IsArmstrong(153), true);
        Check("IsArmstrong(154)", IsArmstrong(154), false);
        Check("Armstrong numbers < 10000", Enumerable.Range(1, 9999).Where(IsArmstrong), [1, 2, 3, 4, 5, 6, 7, 8, 9, 153, 370, 371, 407, 1634, 8208, 9474]);
    }
}
