namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_18, "Next Permutation", Medium,
"Rearrange the numbers into the next lexicographically greater permutation, in place. If none exists, return the lowest order (sorted ascending).")]
public static class NextPermutation
{
    // 1. From the right, find the first i with a[i] < a[i+1] (the pivot).
    // 2. Swap it with the smallest value to its right that is bigger than it.
    // 3. Reverse the suffix after i (it was descending, now ascending). Time O(n)
    public static int[] Solve(int[] a)
    {
        int i = a.Length - 2;
        while (i >= 0 && a[i] >= a[i + 1]) i--;
        if (i >= 0)
        {
            int j = a.Length - 1;
            while (a[j] <= a[i]) j--;
            (a[i], a[j]) = (a[j], a[i]);
        }
        Array.Reverse(a, i + 1, a.Length - i - 1);
        return a;
    }

    public static void Run()
    {
        Check("[1,2,3]", Solve([1, 2, 3]), [1, 3, 2]);
        Check("[3,2,1]", Solve([3, 2, 1]), [1, 2, 3]);
        Check("[1,1,5]", Solve([1, 1, 5]), [1, 5, 1]);
        Check("[1,3,2]", Solve([1, 3, 2]), [2, 1, 3]);
    }
}
