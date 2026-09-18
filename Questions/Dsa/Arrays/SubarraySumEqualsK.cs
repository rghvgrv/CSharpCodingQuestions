namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_14, "Subarray Sum Equals K (Prefix Sums)", Medium,
"Count the contiguous subarrays whose sum equals k. Values may be negative.")]
public static class SubarraySumEqualsK
{
    // sum(i..j) = prefix[j] - prefix[i-1]. So for each prefix, count earlier prefixes equal to prefix - k.
    // Time O(n), Space O(n)
    public static int Solve(int[] nums, int k)
    {
        var seen = new Dictionary<int, int> { [0] = 1 };
        int prefix = 0, count = 0;
        foreach (int x in nums)
        {
            prefix += x;
            count += seen.GetValueOrDefault(prefix - k);
            seen[prefix] = seen.GetValueOrDefault(prefix) + 1;
        }
        return count;
    }

    public static void Run()
    {
        Check("[1,1,1], k=2", Solve([1, 1, 1], 2), 2);
        Check("[1,2,3], k=3", Solve([1, 2, 3], 3), 2);
        Check("[1,-1,0], k=0", Solve([1, -1, 0], 0), 3);
    }
}
