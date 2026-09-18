namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_16, "3Sum", Medium,
"Return all unique triplets [a, b, c] in the array with a + b + c = 0.")]
public static class ThreeSum
{
    // Sort, fix one number, then two-pointer for the other two. Skip equal values to avoid duplicates. Time O(n²)
    public static List<List<int>> Solve(int[] nums)
    {
        Array.Sort(nums);
        var result = new List<List<int>>();
        for (int i = 0; i < nums.Length - 2; i++)
        {
            if (i > 0 && nums[i] == nums[i - 1]) continue;
            int lo = i + 1, hi = nums.Length - 1;
            while (lo < hi)
            {
                int sum = nums[i] + nums[lo] + nums[hi];
                if (sum < 0) lo++;
                else if (sum > 0) hi--;
                else
                {
                    result.Add([nums[i], nums[lo], nums[hi]]);
                    while (lo < hi && nums[lo] == nums[lo + 1]) lo++;
                    while (lo < hi && nums[hi] == nums[hi - 1]) hi--;
                    lo++; hi--;
                }
            }
        }
        return result;
    }

    public static void Run()
    {
        Check("[-1,0,1,2,-1,-4]", Solve([-1, 0, 1, 2, -1, -4]), [[-1, -1, 2], [-1, 0, 1]]);
        Check("[0,0,0,0]", Solve([0, 0, 0, 0]), [[0, 0, 0]]);
        Check("[0,1,1]", Solve([0, 1, 1]), []);
    }
}
