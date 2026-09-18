namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_05, "Next Greater Element & Daily Temperatures (Monotonic Stack)", Medium,
"(1) For each element, find the next element to its right that is greater (-1 if none). (2) For daily temperatures, find how many days until a warmer one.")]
public static class NextGreaterElement
{
    // Keep a stack of indices whose answer is still unknown, with values decreasing from bottom to top.
    // A bigger value pops (and answers) everything smaller on the stack. Each index is pushed and popped once → O(n).
    public static int[] NextGreater(int[] nums)
    {
        var result = Enumerable.Repeat(-1, nums.Length).ToArray();
        var stack = new Stack<int>();
        for (int i = 0; i < nums.Length; i++)
        {
            while (stack.Count > 0 && nums[stack.Peek()] < nums[i]) result[stack.Pop()] = nums[i];
            stack.Push(i);
        }
        return result;
    }

    public static int[] DailyTemperatures(int[] t)
    {
        var wait = new int[t.Length];
        var stack = new Stack<int>();
        for (int i = 0; i < t.Length; i++)
        {
            while (stack.Count > 0 && t[stack.Peek()] < t[i])
            {
                int j = stack.Pop();
                wait[j] = i - j;
            }
            stack.Push(i);
        }
        return wait;
    }

    public static void Run()
    {
        Check("NextGreater([4,5,2,25])", NextGreater([4, 5, 2, 25]), [5, 25, 25, -1]);
        Check("NextGreater([13,7,6,12])", NextGreater([13, 7, 6, 12]), [-1, 12, 12, -1]);
        Check("DailyTemperatures([73,74,75,71,69,72,76,73])", DailyTemperatures([73, 74, 75, 71, 69, 72, 76, 73]), [1, 1, 4, 2, 1, 1, 0, 0]);
    }
}
