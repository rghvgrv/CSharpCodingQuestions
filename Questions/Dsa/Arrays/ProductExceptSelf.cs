namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_09, "Product of Array Except Self", Medium,
"Return an array where answer[i] is the product of every element except nums[i], without using division, in O(n).")]
public static class ProductExceptSelf
{
    // answer[i] = (product of everything left of i) × (product of everything right of i)
    // First pass fills the left products, second pass multiplies in the right products. Space O(1) extra.
    public static int[] Solve(int[] nums)
    {
        int n = nums.Length;
        var answer = new int[n];
        answer[0] = 1;
        for (int i = 1; i < n; i++) answer[i] = answer[i - 1] * nums[i - 1];

        int right = 1;
        for (int i = n - 1; i >= 0; i--)
        {
            answer[i] *= right;
            right *= nums[i];
        }
        return answer;
    }

    public static void Run()
    {
        Check("[1,2,3,4]", Solve([1, 2, 3, 4]), [24, 12, 8, 6]);
        Check("[-1,1,0,-3,3]", Solve([-1, 1, 0, -3, 3]), [0, 0, 9, 0, 0]);
    }
}
