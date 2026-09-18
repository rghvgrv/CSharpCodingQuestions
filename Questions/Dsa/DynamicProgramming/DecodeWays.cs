namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 11, Title = "Decode Ways", Level = Medium, Problem = """
    Letters are coded as numbers: `A = 1`, `B = 2`, …, `Z = 26`. Given a string of digits, count the ways to decode it.
    `"226"` → `3`: "BZ" (2 26), "VF" (22 6), "BBF" (2 2 6). A `0` can't be decoded on its own.
    """)]
public static class DecodeWays
{
    [Approach(Name = "Plain Recursion", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        At each position, either decode **one digit** (1–9) or **two digits** (10–26), and count the ways for the rest.
        """)]
    public static int CountRecursive(string digits)
    {
        return CountFrom(digits, 0);
    }

    private static int CountFrom(string digits, int index)
    {
        if (index == digits.Length)
        {
            return 1;
        }
        if (digits[index] == '0')
        {
            return 0;
        }

        int ways = CountFrom(digits, index + 1);
        if (index + 1 < digits.Length && int.Parse(digits.Substring(index, 2)) <= 26)
        {
            ways += CountFrom(digits, index + 2);
        }
        return ways;
    }

    [Approach(Name = "Bottom-Up With Two Variables", Time = "O(n)", Space = "O(1)", Idea = """
        Ways to decode the first `i` digits =
        (ways for `i - 1` digits, if the last digit is 1–9) + (ways for `i - 2` digits, if the last two digits form 10–26).
        Like Fibonacci, each step needs only the previous two answers.
        """)]
    public static int CountBottomUp(string digits)
    {
        int twoBack = 1;
        int oneBack = digits[0] == '0' ? 0 : 1;

        for (int i = 2; i <= digits.Length; i++)
        {
            int current = 0;
            if (digits[i - 1] != '0')
            {
                current += oneBack;
            }
            int lastTwo = int.Parse(digits.Substring(i - 2, 2));
            if (lastTwo >= 10 && lastTwo <= 26)
            {
                current += twoBack;
            }
            twoBack = oneBack;
            oneBack = current;
        }
        return oneBack;
    }

    public static Example[] Examples =>
    [
        new(["12"], 2),
        new(["226"], 3),
        new(["06"], 0),
        new(["11106"], 2),
    ];
}
