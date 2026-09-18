namespace CSharpCodingQuestions.Questions.Dsa.RecursionAndBacktracking;

[Question(Order = 5, Title = "Generate Valid Parentheses", Level = Medium, Problem = """
    Return every valid way to arrange `n` pairs of parentheses.
    `n = 3` → `["((()))", "(()())", "(())()", "()(())", "()()()"]`.
    """)]
public static class GenerateParentheses
{
    [Approach(Name = "Generate Everything, Keep the Valid Ones", Time = "O(2²ⁿ · n)", Space = "O(n) + output", Idea = """
        Build **every** string of length `2n` made of `(` and `)` (there are 2²ⁿ of them),
        and keep only the balanced ones. Most of the work is thrown away.
        """)]
    public static List<string> GenerateAllThenFilter(int n)
    {
        var result = new List<string>();
        GenerateAll("", 2 * n, result);
        return result;
    }

    private static void GenerateAll(string current, int length, List<string> result)
    {
        if (current.Length == length)
        {
            if (IsBalanced(current))
            {
                result.Add(current);
            }
            return;
        }
        GenerateAll(current + "(", length, result);
        GenerateAll(current + ")", length, result);
    }

    private static bool IsBalanced(string text)
    {
        int open = 0;
        foreach (char bracket in text)
        {
            open += bracket == '(' ? 1 : -1;
            if (open < 0)
            {
                return false;
            }
        }
        return open == 0;
    }

    [Approach(Name = "Backtracking With Counts", Time = "O(4ⁿ / √n)", Space = "O(n) + output", Idea = """
        Only ever build strings that can still become valid:

        - Add `(` if fewer than `n` have been opened.
        - Add `)` only if it would close something (`close < open`).

        No invalid string is ever built, so no work is wasted.
        """)]
    public static List<string> GenerateWithBacktracking(int n)
    {
        var result = new List<string>();
        Build("", 0, 0, n, result);
        return result;
    }

    private static void Build(string current, int open, int close, int n, List<string> result)
    {
        if (current.Length == 2 * n)
        {
            result.Add(current);
            return;
        }
        if (open < n)
        {
            Build(current + "(", open + 1, close, n, result);
        }
        if (close < open)
        {
            Build(current + ")", open, close + 1, n, result);
        }
    }

    public static Example[] Examples =>
    [
        new([3], new[] { "((()))", "(()())", "(())()", "()(())", "()()()" }),
        new([1], new[] { "()" }),
    ];
}
