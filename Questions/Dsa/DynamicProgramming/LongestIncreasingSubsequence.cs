namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_05, "Longest Increasing Subsequence", Medium,
"Find the length of the longest strictly increasing subsequence. Do it in O(n²) with DP, then O(n log n) with patience sorting.")]
public static class LongestIncreasingSubsequence
{
    // dp[i] = LIS ending at i = 1 + max(dp[j]) for j < i with nums[j] < nums[i].
    public static int Quadratic(int[] nums)
    {
        var dp = new int[nums.Length];
        for (int i = 0; i < nums.Length; i++)
        {
            dp[i] = 1;
            for (int j = 0; j < i; j++)
                if (nums[j] < nums[i]) dp[i] = Math.Max(dp[i], dp[j] + 1);
        }
        return nums.Length == 0 ? 0 : dp.Max();
    }

    // tails[k] = smallest possible tail of an increasing subsequence of length k+1.
    // For each x, replace the first tail >= x (binary search), or append if x is bigger than all.
    public static int NLogN(int[] nums)
    {
        var tails = new List<int>();
        foreach (int x in nums)
        {
            int i = tails.BinarySearch(x);
            if (i < 0) i = ~i; // BinarySearch returns the bitwise complement of the insert position
            if (i == tails.Count) tails.Add(x); else tails[i] = x;
        }
        return tails.Count;
    }

    public static void Run()
    {
        Check("Quadratic([10,9,2,5,3,7,101,18])", Quadratic([10, 9, 2, 5, 3, 7, 101, 18]), 4);
        Check("NLogN([10,9,2,5,3,7,101,18])", NLogN([10, 9, 2, 5, 3, 7, 101, 18]), 4);
        Check("NLogN([0,1,0,3,2,3])", NLogN([0, 1, 0, 3, 2, 3]), 4);
        Check("NLogN([7,7,7,7])", NLogN([7, 7, 7, 7]), 1);
    }
}
