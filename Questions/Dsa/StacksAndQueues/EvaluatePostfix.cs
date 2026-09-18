namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 6, Title = "Evaluate a Postfix Expression", Level = Medium, Problem = """
    In **postfix** notation (also called Reverse Polish Notation), the operator comes after its two numbers:
    `3 4 +` means `3 + 4`, and `2 1 + 3 *` means `(2 + 1) × 3 = 9`. No brackets are needed.
    Evaluate an expression given as tokens. Division rounds toward zero.
    """)]
public static class EvaluatePostfix
{
    [Approach(Name = "Stack of Numbers", Time = "O(n)", Space = "O(n)", Idea = """
        Read the tokens from left to right:

        - A number → push it.
        - An operator → pop two numbers, apply the operator, push the result.
          Careful with the order: the **first** number popped is the **right** side (`a - b` pops `b` first).

        The last number left on the stack is the answer. Calculators and compilers work this way.
        """)]
    public static int Evaluate(string[] tokens)
    {
        var numbers = new Stack<int>();
        foreach (string token in tokens)
        {
            if (token is "+" or "-" or "*" or "/")
            {
                int right = numbers.Pop();
                int left = numbers.Pop();
                int result = token switch
                {
                    "+" => left + right,
                    "-" => left - right,
                    "*" => left * right,
                    _ => left / right,
                };
                numbers.Push(result);
            }
            else
            {
                numbers.Push(int.Parse(token));
            }
        }
        return numbers.Pop();
    }

    public static Example[] Examples =>
    [
        new([new[] { "2", "1", "+", "3", "*" }], 9),
        new([new[] { "4", "13", "5", "/", "+" }], 6),
        new([new[] { "10", "2", "-", "3", "-" }], 5),
    ];
}
