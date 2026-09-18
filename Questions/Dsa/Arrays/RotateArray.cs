namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_03, "Rotate Array by K", Medium,
"Rotate an array to the right by k steps, in place with O(1) extra space.")]
public static class RotateArray
{
    // Reversal trick: reverse all, then reverse the first k and the rest.
    // [1,2,3,4,5,6,7], k=3 → [7,6,5,4,3,2,1] → [5,6,7 | 1,2,3,4]
    public static int[] Solve(int[] nums, int k)
    {
        k %= nums.Length;
        Reverse(nums, 0, nums.Length - 1);
        Reverse(nums, 0, k - 1);
        Reverse(nums, k, nums.Length - 1);
        return nums;
    }

    static void Reverse(int[] a, int i, int j)
    {
        for (; i < j; i++, j--) (a[i], a[j]) = (a[j], a[i]);
    }

    public static void Run()
    {
        Check("[1,2,3,4,5,6,7], k=3", Solve([1, 2, 3, 4, 5, 6, 7], 3), [5, 6, 7, 1, 2, 3, 4]);
        Check("[-1,-100,3,99], k=2", Solve([-1, -100, 3, 99], 2), [3, 99, -1, -100]);
        Check("[1,2], k=5", Solve([1, 2], 5), [2, 1]);
    }
}
