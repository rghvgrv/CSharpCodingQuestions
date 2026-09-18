namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_11, "Missing Number", Easy,
"An array holds n distinct numbers from the range 0..n. Find the one that is missing.")]
public static class MissingNumber
{
    // Math: expected sum n(n+1)/2 minus actual sum.
    public static int BySum(int[] nums) => nums.Length * (nums.Length + 1) / 2 - nums.Sum();

    // XOR: x ^ x = 0, so XOR of all indices and values leaves only the missing one. No overflow risk.
    public static int ByXor(int[] nums)
    {
        int result = nums.Length;
        for (int i = 0; i < nums.Length; i++) result ^= i ^ nums[i];
        return result;
    }

    public static void Run()
    {
        Check("BySum([3,0,1])", BySum([3, 0, 1]), 2);
        Check("ByXor([3,0,1])", ByXor([3, 0, 1]), 2);
        Check("ByXor([9,6,4,2,3,5,7,0,1])", ByXor([9, 6, 4, 2, 3, 5, 7, 0, 1]), 8);
        Check("ByXor([0,1])", ByXor([0, 1]), 2);
    }
}
