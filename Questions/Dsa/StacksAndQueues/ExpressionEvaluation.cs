namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_06, "Infix → Postfix & Evaluate (Shunting Yard)", Medium,
"Convert an infix expression like \"3 + 4 * (2 - 1)\" to postfix (Reverse Polish Notation), then evaluate the postfix with a stack.")]
public static class ExpressionEvaluation
{
    static int Precedence(string op) => op is "*" or "/" ? 2 : op is "+" or "-" ? 1 : 0;

    // Shunting yard: numbers go straight to output; operators wait on a stack until
    // an operator of lower precedence (or a ')') arrives.
    public static List<string> ToPostfix(string expr)
    {
        var output = new List<string>();
        var ops = new Stack<string>();
        var tokens = System.Text.RegularExpressions.Regex.Matches(expr, @"\d+|[-+*/()]").Select(m => m.Value);
        foreach (var t in tokens)
        {
            if (char.IsDigit(t[0])) output.Add(t);
            else if (t == "(") ops.Push(t);
            else if (t == ")")
            {
                while (ops.Peek() != "(") output.Add(ops.Pop());
                ops.Pop();
            }
            else
            {
                while (ops.Count > 0 && Precedence(ops.Peek()) >= Precedence(t)) output.Add(ops.Pop());
                ops.Push(t);
            }
        }
        while (ops.Count > 0) output.Add(ops.Pop());
        return output;
    }

    // Evaluate RPN: push numbers; an operator pops two, applies, pushes the result.
    public static int EvalRpn(IEnumerable<string> tokens)
    {
        var stack = new Stack<int>();
        foreach (var t in tokens)
        {
            if (int.TryParse(t, out int n)) { stack.Push(n); continue; }
            int b = stack.Pop(), a = stack.Pop();
            stack.Push(t switch { "+" => a + b, "-" => a - b, "*" => a * b, _ => a / b });
        }
        return stack.Pop();
    }

    public static void Run()
    {
        Check("ToPostfix(\"3 + 4 * (2 - 1)\")", string.Join(' ', ToPostfix("3 + 4 * (2 - 1)")), "3 4 2 1 - * +");
        Check("EvalRpn(3 4 2 1 - * +)", EvalRpn(ToPostfix("3 + 4 * (2 - 1)")), 7);
        Check("EvalRpn([2,1,+,3,*])", EvalRpn(["2", "1", "+", "3", "*"]), 9);
        Check("EvalRpn([4,13,5,/,+])", EvalRpn(["4", "13", "5", "/", "+"]), 6);
        Check("Evaluate \"10 - 2 - 3\" (left-assoc)", EvalRpn(ToPostfix("10 - 2 - 3")), 5);
    }
}
