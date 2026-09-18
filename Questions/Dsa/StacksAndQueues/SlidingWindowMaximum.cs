namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_08, "Sliding Window Maximum (Monotonic Deque)", Hard,
"For every window of size k sliding over the array, return the maximum, in O(n) overall.")]
public static class SlidingWindowMaximum
{
    // Deque holds indices with decreasing values. Front = max of the window.
    // Drop the front when it leaves the window; drop smaller values from the back since they can never be a max.
    public static List<int> Solve(int[] nums, int k)
    {
        var dq = new LinkedList<int>();
        var result = new List<int>();
        for (int i = 0; i < nums.Length; i++)
        {
            if (dq.Count > 0 && dq.First!.Value <= i - k) dq.RemoveFirst();
            while (dq.Count > 0 && nums[dq.Last!.Value] <= nums[i]) dq.RemoveLast();
            dq.AddLast(i);
            if (i >= k - 1) result.Add(nums[dq.First!.Value]);
        }
        return result;
    }

    public static void Run()
    {
        Check("[1,3,-1,-3,5,3,6,7], k=3", Solve([1, 3, -1, -3, 5, 3, 6, 7], 3), [3, 3, 5, 5, 6, 7]);
        Check("[1], k=1", Solve([1], 1), [1]);
        Check("[9,8,7,6], k=2", Solve([9, 8, 7, 6], 2), [9, 8, 7]);
    }
}
