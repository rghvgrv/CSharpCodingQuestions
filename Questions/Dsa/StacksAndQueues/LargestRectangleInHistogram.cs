namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 9, Title = "Largest Rectangle in a Histogram", Level = Hard, Problem = """
    `heights` are bars of width 1 standing side by side. Find the area of the biggest rectangle that fits inside the bars.
    `[2, 1, 5, 6, 2, 3]` → `10` (height 5, over the bars 5 and 6).
    """)]
public static class LargestRectangleInHistogram
{
    [Approach(Name = "Try Every Range of Bars", Time = "O(n²)", Space = "O(1)", Idea = """
        For each start bar, extend to the right while tracking the lowest bar in the range.
        The rectangle over that range has area `lowest × width`.
        """)]
    public static int LargestBruteForce(int[] heights)
    {
        int best = 0;
        for (int start = 0; start < heights.Length; start++)
        {
            int lowest = int.MaxValue;
            for (int end = start; end < heights.Length; end++)
            {
                lowest = Math.Min(lowest, heights[end]);
                best = Math.Max(best, lowest * (end - start + 1));
            }
        }
        return best;
    }

    [Approach(Name = "Monotonic Stack", Time = "O(n)", Space = "O(n)", Idea = """
        Keep a stack of bar positions with **increasing** heights.
        When a shorter bar arrives, every taller bar on the stack can't stretch further right, so pop it and compute its rectangle:

        - height = the popped bar's height
        - width = from just after the new top of the stack, up to just before the current bar

        A pretend bar of height 0 at the end pops everything that's left.
        """)]
    public static int LargestWithStack(int[] heights)
    {
        var stack = new Stack<int>();
        int best = 0;

        for (int i = 0; i <= heights.Length; i++)
        {
            int currentHeight = i == heights.Length ? 0 : heights[i];
            while (stack.Count > 0 && heights[stack.Peek()] >= currentHeight)
            {
                int height = heights[stack.Pop()];
                int left = stack.Count == 0 ? -1 : stack.Peek();
                int width = i - left - 1;
                best = Math.Max(best, height * width);
            }
            stack.Push(i);
        }
        return best;
    }

    public static Example[] Examples =>
    [
        new([new[] { 2, 1, 5, 6, 2, 3 }], 10),
        new([new[] { 2, 4 }], 4),
        new([new[] { 6, 2, 5, 4, 5, 1, 6 }], 12),
    ];
}
