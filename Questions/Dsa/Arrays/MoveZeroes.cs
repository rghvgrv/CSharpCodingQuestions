namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_05, "Move Zeroes", Easy,
"Move all 0s to the end of the array in place, keeping the order of the non-zero elements.")]
public static class MoveZeroes
{
    // Time O(n), Space O(1): swap each non-zero into the next write slot.
    public static int[] Solve(int[] nums)
    {
        int write = 0;
        for (int i = 0; i < nums.Length; i++)
            if (nums[i] != 0) (nums[write], nums[i]) = (nums[i], nums[write++]);
        return nums;
    }

    public static void Run()
    {
        Check("[0,1,0,3,12]", Solve([0, 1, 0, 3, 12]), [1, 3, 12, 0, 0]);
        Check("[0]", Solve([0]), [0]);
        Check("[4,2,0,0,1]", Solve([4, 2, 0, 0, 1]), [4, 2, 1, 0, 0]);
    }
}
