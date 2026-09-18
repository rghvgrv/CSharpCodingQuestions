namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 5, Title = "Next Greater Element", Level = Medium, Problem = """
    For each number, find the first number to its **right** that is bigger. Use `-1` if there is none.
    `[4, 5, 2, 25]` → `[5, 25, 25, -1]`.
    """)]
public static class NextGreaterElement
{
    [Approach(Name = "Look Right From Each Number", Time = "O(n²)", Space = "O(1) extra", Idea = """
        For each position, scan to the right until you find a bigger number.
        """)]
    public static int[] NextGreaterBruteForce(int[] numbers)
    {
        int[] result = new int[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            result[i] = -1;
            for (int j = i + 1; j < numbers.Length; j++)
            {
                if (numbers[j] > numbers[i])
                {
                    result[i] = numbers[j];
                    break;
                }
            }
        }
        return result;
    }

    [Approach(Name = "Monotonic Stack", Time = "O(n)", Space = "O(n)", Idea = """
        Keep a stack of positions that are **still waiting** for a bigger number. Their values decrease from bottom to top.

        For each new number: while it's bigger than the number at the top of the stack, that waiting position has found its answer, so pop it.
        Then push the new position, since it's now waiting too.

        Each position is pushed once and popped once, so the whole thing is linear.
        """)]
    public static int[] NextGreaterWithStack(int[] numbers)
    {
        int[] result = new int[numbers.Length];
        Array.Fill(result, -1);
        var waiting = new Stack<int>();

        for (int i = 0; i < numbers.Length; i++)
        {
            while (waiting.Count > 0 && numbers[waiting.Peek()] < numbers[i])
            {
                result[waiting.Pop()] = numbers[i];
            }
            waiting.Push(i);
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([new[] { 4, 5, 2, 25 }], new[] { 5, 25, 25, -1 }),
        new([new[] { 13, 7, 6, 12 }], new[] { -1, 12, 12, -1 }),
        new([new[] { 73, 74, 75, 71, 69, 72, 76, 73 }], new[] { 74, 75, 76, 72, 72, 76, -1, -1 }),
    ];
}
