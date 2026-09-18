namespace CodingQuestions.Dsa.RecursionAndBacktracking;

[Q(1_08_05, "Generate Parentheses", Medium,
"Generate every well-formed combination of n pairs of parentheses.")]
public static class GenerateParentheses
{
    // Add '(' while we have some left; add ')' only if it would close an open one.
    public static List<string> Solve(int n)
    {
        var result = new List<string>();
        void Build(string s, int open, int close)
        {
            if (s.Length == 2 * n) { result.Add(s); return; }
            if (open < n) Build(s + "(", open + 1, close);
            if (close < open) Build(s + ")", open, close + 1);
        }
        Build("", 0, 0);
        return result;
    }

    public static void Run()
    {
        Check("n=3", Solve(3), ["((()))", "(()())", "(())()", "()(())", "()()()"]);
        Check("n=1", Solve(1), ["()"]);
        Check("n=5 count (Catalan number)", Solve(5).Count, 42);
    }
}
