namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_03, "Valid Parentheses", Easy,
"Given a string of ()[]{} characters, check whether every bracket is closed by the same type, in the correct order.")]
public static class ValidParentheses
{
    // Push the expected closer for each opener. Each closer must match the top of the stack.
    public static bool Solve(string s)
    {
        var stack = new Stack<char>();
        foreach (char c in s)
        {
            switch (c)
            {
                case '(': stack.Push(')'); break;
                case '[': stack.Push(']'); break;
                case '{': stack.Push('}'); break;
                default:
                    if (stack.Count == 0 || stack.Pop() != c) return false;
                    break;
            }
        }
        return stack.Count == 0;
    }

    public static void Run()
    {
        Check("\"()[]{}\"", Solve("()[]{}"), true);
        Check("\"([{}])\"", Solve("([{}])"), true);
        Check("\"(]\"", Solve("(]"), false);
        Check("\"([)]\"", Solve("([)]"), false);
        Check("\"((\"", Solve("(("), false);
    }
}
