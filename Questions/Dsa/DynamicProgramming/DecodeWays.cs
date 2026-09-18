namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_11, "Decode Ways", Medium,
"Letters are encoded A=1 … Z=26. Given a digit string, count the ways to decode it. \"226\" → BZ, VF, BBF = 3.")]
public static class DecodeWays
{
    // ways(i) = ways(i-1) if the last digit is 1–9, plus ways(i-2) if the last two digits form 10–26.
    public static int Solve(string s)
    {
        int prev2 = 1, prev1 = s[0] == '0' ? 0 : 1;
        for (int i = 2; i <= s.Length; i++)
        {
            int cur = 0;
            if (s[i - 1] != '0') cur += prev1;
            int two = int.Parse(s.AsSpan(i - 2, 2));
            if (two is >= 10 and <= 26) cur += prev2;
            (prev2, prev1) = (prev1, cur);
        }
        return prev1;
    }

    public static void Run()
    {
        Check("\"12\"", Solve("12"), 2);
        Check("\"226\"", Solve("226"), 3);
        Check("\"06\"", Solve("06"), 0);
        Check("\"11106\"", Solve("11106"), 2);
    }
}
