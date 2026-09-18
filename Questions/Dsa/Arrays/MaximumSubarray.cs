namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_08, "Maximum Subarray (Kadane's Algorithm)", Medium,
"Find the contiguous subarray with the largest sum and return the sum and the subarray itself.")]
public static class MaximumSubarray
{
    // Kadane, Time O(n): at each index either extend the current run or start fresh here.
    public static (int Sum, int[] Sub) Solve(int[] nums)
    {
        int best = nums[0], current = nums[0], start = 0, bestStart = 0, bestEnd = 0;
        for (int i = 1; i < nums.Length; i++)
        {
            if (current < 0) { current = nums[i]; start = i; }
            else current += nums[i];

            if (current > best) { best = current; bestStart = start; bestEnd = i; }
        }
        return (best, nums[bestStart..(bestEnd + 1)]);
    }

    public static void Run()
    {
        var (sum, sub) = Solve([-2, 1, -3, 4, -1, 2, 1, -5, 4]);
        Check("[-2,1,-3,4,-1,2,1,-5,4] sum", sum, 6);
        Check("subarray", sub, [4, -1, 2, 1]);
        Check("[5,4,-1,7,8] sum", Solve([5, 4, -1, 7, 8]).Sum, 23);
        Check("[-3,-1,-2] sum (all negative)", Solve([-3, -1, -2]).Sum, -1);
    }
}
