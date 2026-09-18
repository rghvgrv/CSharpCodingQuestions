namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 3, Title = "Valid Parentheses", Level = Easy, Problem = """
    The text contains only `( ) [ ] { }`. It's valid when every bracket is closed by the same type, in the right order.
    `"([]{})"` → `true`. `"([)]"` → `false`.
    """)]
public static class ValidParentheses
{
    [Approach(Name = "Remove Pairs Until Nothing Changes", Time = "O(n²)", Space = "O(n)", Idea = """
        A valid text always has an empty pair like `()`, `[]` or `{}` somewhere. Remove those pairs again and again.
        If the text ends up empty, it was valid.
        """)]
    public static bool IsValidByRemoving(string text)
    {
        while (text.Contains("()") || text.Contains("[]") || text.Contains("{}"))
        {
            text = text.Replace("()", "").Replace("[]", "").Replace("{}", "");
        }
        return text.Length == 0;
    }

    [Approach(Name = "Stack", Time = "O(n)", Space = "O(n)", Idea = """
        Read left to right:

        - An **opening** bracket → push the closing bracket we now expect.
        - A **closing** bracket → it must match the top of the stack (the most recent unclosed bracket). Pop it.

        At the end, the stack must be empty, meaning nothing was left open.
        """)]
    public static bool IsValidWithStack(string text)
    {
        var expected = new Stack<char>();
        foreach (char bracket in text)
        {
            if (bracket == '(')
            {
                expected.Push(')');
            }
            else if (bracket == '[')
            {
                expected.Push(']');
            }
            else if (bracket == '{')
            {
                expected.Push('}');
            }
            else if (expected.Count == 0 || expected.Pop() != bracket)
            {
                return false;
            }
        }
        return expected.Count == 0;
    }

    public static Example[] Examples =>
    [
        new(["([]{})"], true),
        new(["([)]"], false),
        new(["(("], false),
        new(["{[()()]}"], true),
    ];
}
