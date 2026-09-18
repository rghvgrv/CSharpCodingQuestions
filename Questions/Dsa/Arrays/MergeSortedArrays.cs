namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_21, "Merge Two Sorted Arrays In Place", Easy,
"nums1 has length m + n, with m real values followed by n zeros. Merge sorted nums2 (length n) into nums1 so the result is sorted.")]
public static class MergeSortedArrays
{
    // Fill from the back so nothing is overwritten before it's used. Time O(m + n), Space O(1)
    public static int[] Solve(int[] nums1, int m, int[] nums2, int n)
    {
        int i = m - 1, j = n - 1, write = m + n - 1;
        while (j >= 0)
            nums1[write--] = i >= 0 && nums1[i] > nums2[j] ? nums1[i--] : nums2[j--];
        return nums1;
    }

    public static void Run()
    {
        Check("[1,2,3,0,0,0] + [2,5,6]", Solve([1, 2, 3, 0, 0, 0], 3, [2, 5, 6], 3), [1, 2, 2, 3, 5, 6]);
        Check("[0] + [1]", Solve([0], 0, [1], 1), [1]);
    }
}
