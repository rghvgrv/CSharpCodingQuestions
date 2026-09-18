namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_09, "Largest Rectangle in Histogram", Hard,
"Given bar heights of width 1, find the area of the largest rectangle inside the histogram.")]
public static class LargestRectangleHistogram
{
    // Stack of indices with increasing heights. When a shorter bar arrives, each taller bar popped
    // can't extend further right: its width spans from the new stack top + 1 to i - 1.
    // A sentinel height 0 at the end flushes the stack. Time O(n)
    public static int Solve(int[] h)
    {
        var stack = new Stack<int>();
        int best = 0;
        for (int i = 0; i <= h.Length; i++)
        {
            int cur = i == h.Length ? 0 : h[i];
            while (stack.Count > 0 && h[stack.Peek()] >= cur)
            {
                int height = h[stack.Pop()];
                int width = stack.Count == 0 ? i : i - stack.Peek() - 1;
                best = Math.Max(best, height * width);
            }
            stack.Push(i);
        }
        return best;
    }

    public static void Run()
    {
        Check("[2,1,5,6,2,3]", Solve([2, 1, 5, 6, 2, 3]), 10);
        Check("[2,4]", Solve([2, 4]), 4);
        Check("[6,2,5,4,5,1,6]", Solve([6, 2, 5, 4, 5, 1, 6]), 12);
    }
}
