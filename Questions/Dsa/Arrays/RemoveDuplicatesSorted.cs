namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_04, "Remove Duplicates from Sorted Array", Easy,
"Remove duplicates in place from a sorted array so each value appears once. Return the new length k; the first k elements hold the unique values.")]
public static class RemoveDuplicatesSorted
{
    // Time O(n), Space O(1): slow pointer = next write position, fast pointer scans.
    public static int Solve(int[] nums)
    {
        if (nums.Length == 0) return 0;
        int write = 1;
        for (int read = 1; read < nums.Length; read++)
            if (nums[read] != nums[write - 1]) nums[write++] = nums[read];
        return write;
    }

    public static void Run()
    {
        int[] a = [0, 0, 1, 1, 1, 2, 2, 3, 3, 4];
        int k = Solve(a);
        Check("[0,0,1,1,1,2,2,3,3,4] → k", k, 5);
        Check("first k", a[..k], [0, 1, 2, 3, 4]);
        Check("[1,1,2] → k", Solve([1, 1, 2]), 2);
    }
}
