namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_02, "House Robber", Medium,
"Rob houses along a street for maximum money, but never two adjacent houses. Bonus: the houses are in a circle.")]
public static class HouseRobber
{
    // best(i) = max(best(i-1) [skip house i], best(i-2) + nums[i] [rob house i])
    public static int Line(IEnumerable<int> nums)
    {
        int skip = 0, take = 0; // best up to i-2, best up to i-1
        foreach (int x in nums) (skip, take) = (take, Math.Max(take, skip + x));
        return take;
    }

    // Circle: first and last are adjacent → answer = max(rob without last, rob without first).
    public static int Circle(int[] nums) =>
        nums.Length == 1 ? nums[0] : Math.Max(Line(nums[..^1]), Line(nums[1..]));

    public static void Run()
    {
        Check("Line([1,2,3,1])", Line([1, 2, 3, 1]), 4);
        Check("Line([2,7,9,3,1])", Line([2, 7, 9, 3, 1]), 12);
        Check("Circle([2,3,2])", Circle([2, 3, 2]), 3);
        Check("Circle([1,2,3,1])", Circle([1, 2, 3, 1]), 4);
    }
}
